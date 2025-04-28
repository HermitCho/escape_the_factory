using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using ChestSystem;
using System.Security.Cryptography;



/// <summary>
/// �κ��丮 ���� �ϳ��� ���
/// </summary>
/// 
[System.Serializable]
public class InventorySlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Item mItem; //���� ������ �ν��Ͻ�
    public ItemActionManager mItemActionManager;
    public SlotToolTip mSlotToolTip;
    private AudioSource mAudioSource;
    public Item Item
    {
        get
        {
            return mItem;
        }
    }



    [Header("�ش� ���Կ� ��� Ÿ�Ը� ���� �� �ִ��� Ÿ�� ����ũ")]
    [SerializeField] private ItemType mSlotMask;

    private int mItemCount; //ȹ���� �������� ����
    public int ItemCount
    {
        get
        {
            return mItemCount;
        }
        set
        {
            mItemCount = value;
        }
    }

    [Header("������ ���Կ� �ִ� UI ������Ʈ")]
    [SerializeField] private Image mItemImage; //�������� �̹���
    [SerializeField] private Image mCooltimeImage; //������ ��Ÿ�� �̹���
    [SerializeField] public Text mTextCount; //�������� ���� �ؽ�Ʈ




    // ������ �̹����� ���� ����
    private void SetColor(float _alpha)
    {
        Color color = mItemImage.color;
        color.a = _alpha;
        mItemImage.color = color;
    }

    /// <summary>
    /// mSlotMask���� ������ ���� ���� ��Ʈ�������Ѵ�.
    /// ���� ����ũ���� ��Ʈ�������� 0�� ���´ٸ� ���� ���Կ� ����ũ�� ��ġ���� �ʴ´ٴ� ��.
    /// 0�� �ƴ� ���� ���� ��Ʈ��ġ(10������ 1, 2, 4, 8)�� ���� ���´�.
    /// </summary>
    public bool IsMask(Item item)
    {
        return ((int)item.Type & (int)mSlotMask) == 0 ? false : true;
    }

    // �κ��丮�� ���ο� ������ ���� �߰�
    public void AddItem(Item item, int count = 1)
    {
        mItem = item;
        mItemCount = count;
        mItemImage.sprite = mItem.Image;

        if (mItem.Type <= ItemType.Equipment_SHOES)
        {
            mTextCount.text = "";
        }
        else
        {
            mTextCount.text = mItemCount.ToString();
        }

        SetColor(1);
    }

    // �ش� ������ ������ ���� ������Ʈ
    public void UpdateSlotCount(int count)
    {
        mItemCount += count;
        mTextCount.text = mItemCount.ToString();

        if (mItemCount <= 0)
            ClearSlot();
    }

    // �ش� ���� �ϳ� ����
    public void ClearSlot()
    {
        mItem = null;
        mItemCount = 0;
        mItemImage.sprite = null;
        SetColor(0);

        mTextCount.text = "";
        
    }


    /// <summary>
    /// ���콺 Ŭ�� �������̵峪 �ܺο��� �ش� ������ ������� ���� ����ϵ��� ȣ��
    /// </summary>
    public void UseItem()
    {
        if (mItem != null) //�ش� ������ �������� null�̶�� return
        {
            //��ȣ�ۿ��� �Ұ����� (����� �Ұ�����) �������̶�� ����
            if (!mItem.IsInteractivity) { return; }

            //��Ÿ���� 0���� ū��� (���� ��Ÿ���� �����ִ°��)��� �����Ѵ�.
           // if (ItemCooltimeManager.Instance.GetCurrentCooltime(mItem.ItemID) > 0) { return; }

            //������ ��� �Լ� ȣ��
            //���� ������ �Լ� ȣ���� ���¿��� false�� ���ϵǸ�, ���� ��� �Ұ��� �����̱⿡ �����Ѵ�.
           if (!mItemActionManager.UseItem(mItem)) { return; }

            //�������� ��Ÿ���� �����Ǿ������� ��Ÿ�� ����
         //   if (mItem.Cooltime > 0f) { ItemCooltimeManager.Instance.AddCooltimeQueue(mItem.ItemID, mItem.Cooltime); }

            //��ȣ�ۿ��� ������(���� ������) ��� �������� ����Ѱ��?
          //  if (mItem.Type >= ItemType.Equipment_HELMET && mItem.Type <= ItemType.Equipment_SHOES) { ChangeEquipmentSlot(); }

            //�������� �Ҹ��̸� �Ѱ��� ������ ����
            if (mItem != null && mItem.IsConsumable) {  UpdateSlotCount(-1); }

            //�������� �پ����, UpdateSlotCount�� ���� mItem�� null�� �Ǵ� ��쿡 UI�� ����.
        //   if (mItem == null) { mItemDescription.CloseUI(); }
        }
    }





    private void ChangeSlot()
    {
        if (DragSlot.Instance.CurrentSlot.Item.Type >= ItemType.Etc)
        {
            //����Ʈ ��� ���� ���� �Ͼ �� �ִ� ���
            //���ο� ���԰� ���� ������ ������ID�� ������� ������ ��ģ��.
            if (mItem != null && mItem.ItemID == DragSlot.Instance.CurrentSlot.Item.ItemID)
            {
                int changedSlotCount;
                if (DragSlot.Instance.IsShiftMode) { changedSlotCount = (int)(DragSlot.Instance.CurrentSlot.mItemCount * 0.5f); }
                else { changedSlotCount = DragSlot.Instance.CurrentSlot.mItemCount; }

                UpdateSlotCount(changedSlotCount);
                DragSlot.Instance.CurrentSlot.UpdateSlotCount(-changedSlotCount);
                return;
            }

            //����Ʈ ����ΰ�� ������ ������ ������.
            if (DragSlot.Instance.IsShiftMode)
            {
                //changedSlotCount ������ŭ ���ϰ� �����̴� (+�� -�� ���̰� 0�̾�� �������� ����, ���ǵ��� �ʴ´�.)
                int changedSlotCount = (int)(DragSlot.Instance.CurrentSlot.mItemCount * 0.5f);

                //(int)���� ����ȯ���� ���� 0�� �Ǵ� ���� (int)(1 * 0.5f)�̱⿡ �ܼ��� ���ο� �������� �ű��.
                if (changedSlotCount == 0)
                {
                    AddItem(DragSlot.Instance.CurrentSlot.Item, 1);
                    DragSlot.Instance.CurrentSlot.ClearSlot();
                    return;
                }

                //�� ��� ��찡 �ƴѰ�� ���ο� ���Կ� ���ο� �������� �����Ѵ�.
                AddItem(DragSlot.Instance.CurrentSlot.Item, changedSlotCount);
                DragSlot.Instance.CurrentSlot.UpdateSlotCount(-changedSlotCount);
                return;
            }
        }

        //���� ���� ��ȯ�ϱ�
        Item tempItem = mItem;
        int tempItemCount = mItemCount;

        AddItem(DragSlot.Instance.CurrentSlot.mItem, DragSlot.Instance.CurrentSlot.mItemCount);

        if (tempItem != null) { DragSlot.Instance.CurrentSlot.AddItem(tempItem, tempItemCount); }
        else { DragSlot.Instance.CurrentSlot.ClearSlot(); }
    }



    // ���콺 �巡�� ���� �������̵�
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (mItem != null)
        {
            SoundManager.Instance.GetSound(mAudioSource, 7, 0.3f, 0.5f, 1f);

            //����Ʈ (������ ������ ��� Ȱ��ȭ)
            if (Input.GetKey(KeyCode.LeftShift)) { DragSlot.Instance.IsShiftMode = true; }
            else { DragSlot.Instance.IsShiftMode = false; }

            //���� �������� ���
            DragSlot.Instance.CurrentSlot = this;
            DragSlot.Instance.DragSetImage(mItemImage);
            DragSlot.Instance.transform.position = eventData.position;
        }
    }

    // ���콺 �巡�� �� �� �����Ӹ��� ȣ��Ǵ� �������̵�
    public void OnDrag(PointerEventData eventData)
    {

        if (mItem != null)
        {  
            DragSlot.Instance.transform.position = eventData.position;
            

        }
    
            
    }

    // ���콺 �巡�� ���� �������̵�
    public void OnEndDrag(PointerEventData eventData)
    {
        

        DragSlot.Instance.SetColor(0);
        DragSlot.Instance.CurrentSlot = null;
        if (mItem != null)
        {
            mSlotToolTip.ShowToolTip(mItem, eventData.position);
        }
           
        
    }

    // �ش� ���Կ� ���𰡰� ���콺 ��� ���� �� �߻��ϴ� �̺�Ʈ
    public void OnDrop(PointerEventData eventData)
    {
        //����Ʈ ����� ��Ȳ���� �ش� ��ġ�� �������� �ִ°��, ������ ���� �� ���⿡ �����Ѵ�.
        if (DragSlot.Instance.IsShiftMode && mItem != null) { return; }

        //�巡�� ���Կ� ������ �����۰�, �ٲ��� �������� ����ũ�� ��� ����Ǹ� �ٲ۴�.
        if (!IsMask(DragSlot.Instance.CurrentSlot.Item)) { return; }

        //Ÿ�� �巡�� ���Կ� �̹� �������� �ִ°��, �ش� �������� ������ ������ ���Կ��� ����ũ�� üũ�Ѵ�.
        if (mItem != null && !DragSlot.Instance.CurrentSlot.IsMask(mItem)) { return; }

        SoundManager.Instance.GetSound(mAudioSource, 7, 0.3f);
        ChangeSlot();

        mItemActionManager.SlotOnDropEvent(this); //��� �̺�Ʈ ȣ��
        if (mItem != null)
        {
            mSlotToolTip.ShowToolTip(mItem, eventData.position);
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)  //��ư ��Ŭ����
        {
            if (mSlotMask == ItemType.SKILL) { return; }
            
            UseItem();
           
        }
    }

    // ���콺 Ŀ���� ���Կ� �� �� �ߵ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (mItem != null)
        {
            
            mSlotToolTip.ShowToolTip(mItem, eventData.position);
        }

    }

    // ���콺 Ŀ���� ���Կ��� ���� �� �ߵ�
    public void OnPointerExit(PointerEventData eventData)
    {

            mSlotToolTip.HideToolTip();
    }

    private void Awake()
    {
        mSlotToolTip.HideToolTip();
        mAudioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.I))
        {
            mSlotToolTip.HideToolTip();
        }
    }

}
