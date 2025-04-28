using ChestSystem;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;



/// <summary>
/// �� ���� ������(�Ǵ� ���� ��ü)�� �ٰ����� �ش� �������� �ݰų�, ��ȣ�ۿ� �� �� �ֵ��� ���ִ� ��ũ��Ʈ
/// �÷��̾��� ������Ʈ�� �ڽ����� ���� EmptyObject�� Trigger Collider�� �߰��Ͽ� ���
/// </summary>
public class ItemRaycast : MonoBehaviour
{

    private AudioSource UIAudio;

    /// <summary>
    /// ����ĳ��Ʈ �� ������
    /// </summary>
    private RaycastHit mHit;

    /// <summary>
    /// ����ĳ��Ʈ �Ÿ�
    /// </summary>
    [SerializeField] private float mRayDistance;


    private PlayerInput playerInput;

    [HideInInspector] public bool mIsPickupActive = false;  //������ ������ �����Ѱ�?
    [HideInInspector] public bool mIsBoxInteract= false; //�ڽ� ��ȣ�ۿ��� �����Ѱ�?
    [HideInInspector] public bool mIsShopInteract = false;//���� ��ȣ�ۿ��� �����Ѱ�?
    [HideInInspector] public bool mIsEquipment = false;

    private ItemPickUp mCurrentItem; //Ȱ��ȭ�� ���� ��ϵ� ������
    private ChestController mChestController; //Ȱ��ȭ�� ���� ��ϵ� �ڽ� 
    private ShopSystem mItemShop; //Ȱ��ȭ�� ���� ��ϵ� ���� 

    [Header("����ĳ��Ʈ�� �� ī�޶�")]
    [SerializeField] private Camera mRayCamera; //���̸� �� ī�޶� (����ī�޶�)

    [SerializeField] private InventoryMain mInventory; //�κ��丮 ����


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        UIAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        CheckItem();
        CheckChest();
        CheckShop();

        if (mIsPickupActive) { TryPickItem(); }

        else if (mIsBoxInteract && ShopSystem.Instance.mIsShopActive == false && InventoryMain.IsInventoryActive == false)
        { 
            TryOpenChest();

            InventoryMain.Instance.TryOpenInventory();
        }
        else if (mIsShopInteract && ChestDialogManager.IsDialogActive == false && InventoryMain.IsInventoryActive == false)
        {
            TryOpenShop();

            InventoryMain.Instance.TryOpenInventory();

        }
        else { return; }

        if(mCurrentItem.IndicatorHeight <= 0f)
        {
            mIsEquipment = true;
        }
        else
        {
            mIsEquipment = false;
        }
    }


   

    private void CheckShop()
    {
        if (Physics.Raycast(mRayCamera.transform.position, mRayCamera.transform.forward, out mHit, mRayDistance))
        {
            //����ĳ��Ʈ ����� �±װ� �����̶��?
            if (mHit.transform.tag == "Shop")
            {
                //���� ����ĳ��Ʈ�� ����
                ShopSystem rayCastedShop = mHit.transform.GetComponent<ShopSystem>();
                //����ĳ��Ʈ ����� ���� �����۰� ������ ���� (�ߺ�ȣ�� ����)
                if (mItemShop == rayCastedShop) { return; }

                mItemShop = mHit.transform.GetComponent<ShopSystem>();


                Debug.LogFormat("���� ���� ����");

                mIsShopInteract = true;
                return;
            }
            else
            {
                ShopInfoDisappear();
            }
        }
        else
        {
            ShopInfoDisappear();
        }
    }

    /// <summary>
    /// ������ �� �� �ִ��� Ȯ���Ѵ�.
    /// </summary>
    private void TryOpenShop()
    {
        if (playerInput.interact)
        {
            SoundManager.Instance.GetSound(UIAudio, 10, 0.4f, 2f, 1f);
            mItemShop.TryOpenShop();
            Debug.Log("���� ����");
        }
    }

    /// <summary>
    /// ���ڸ� �� �� �ִ��� Ȯ���Ѵ�.
    /// </summary>
    private void TryOpenChest()
    {
        if (playerInput.interact)
        {
            SoundManager.Instance.GetSound(UIAudio, 8, 0.3f);
            mChestController.TryOpenDialog();
            
        }
    }

    /// <summary>
    /// �������� �ֿ� �� �ִ��� Ȯ���Ѵ�.
    /// </summary>
    private void TryPickItem()
    {
        if (playerInput.interact)
        {
            SoundManager.Instance.GetSound(UIAudio, 7, 0.3f);
            //�ֿ� �� �ִ� �������̶��?
            if (mCurrentItem.Item.Type > ItemType.NONE && !mIsEquipment)
            {
                //���� �κ��丮 ������ ��������
                InventorySlot[] allitems = mInventory.GetAllItems();

                int count = 0;
                for (; count < allitems.Length; ++count)
                {
                    //���� ������ ĭ�� null�̶�� �ֿ� �� �ִ� ����
                    if (allitems[count].Item == null) { break; }

                    //���� ������ĭ�� null�� �ƴ�����, ���� �����۰� �����ϸ鼭 ��ø�� ������ �������̶�� �ֿ� �� �ִ� ����
                    if (allitems[count].Item.ItemID == mCurrentItem.Item.ItemID && allitems[count].Item.CanOverlap) { break; }
                }

                //��� ĭ�� null�� �ƴϰ�, ��ø�� �Ұ����ϸ� �ֿ� �� ����
                if (count == allitems.Length) { return; }

                //������ �ݴ� ȿ���� ���
                //SoundManager.Instance.PlaySound2D("GrabItem " + SoundManager.Range(1, 3));
            }

            TryPickUp();
            ItemInfoDisappear();
        }
    }

    /// <summary>
    /// ����ĳ��Ʈ�� �̿��Ͽ� ������ Ȯ���Ѵ�.
    /// </summary>
    private void CheckChest()
    {
        if (Physics.Raycast(mRayCamera.transform.position, mRayCamera.transform.forward, out mHit, mRayDistance))
        {
            //����ĳ��Ʈ ����� �±װ� �ڽ����?
            if (mHit.transform.tag == "Box" || mHit.transform.tag == "NPC")
            {
                //���� ����ĳ��Ʈ�� �ڽ�
                ChestController rayCastedBox = mHit.transform.GetComponent<ChestController>();
                //����ĳ��Ʈ ����� ���� �����۰� ������ ���� (�ߺ�ȣ�� ����)
                if (mChestController == rayCastedBox) { return; }

                mChestController = mHit.transform.GetComponent<ChestController>();
                

                Debug.LogFormat("���� ���� ����");

                mIsBoxInteract = true;
                return;
            }
            else
            {
                BoxInfoDisappear();
            }
        }
        else
        {
            BoxInfoDisappear();
        }
    }

    /// <summary>
    /// ����ĳ��Ʈ�� �̿��Ͽ� �������� Ȯ���Ѵ�.
    /// </summary>
    private void CheckItem()
    {
        if (Physics.Raycast(mRayCamera.transform.position, mRayCamera.transform.forward, out mHit, mRayDistance))
        {
            //����ĳ��Ʈ ����� �±װ� �������̶��?
            if (mHit.transform.tag == "Item" || mHit.transform.tag == "NPC")
            {
                //���� ����ĳ��Ʈ�� ������
                ItemPickUp rayCastedItem = mHit.transform.GetComponent<ItemPickUp>();

                //����ĳ��Ʈ ����� ���� �����۰� ������ ���� (�ߺ�ȣ�� ����)
                if (mCurrentItem == rayCastedItem) { return; }

                //������ ������ �� ���� ȣ��
                mCurrentItem = mHit.transform.GetComponent<ItemPickUp>();
                // mItemRaycastInfoText.EnableText(mHit.transform.position + Vector3.up * rayCastedItem.IndicatorHeight, mCurrentItem.Item); (�� �ۿ����� ���� X)

                Debug.LogFormat("������: {0} ȹ�� ����", mCurrentItem.Item.name);

                mIsPickupActive = true;

                return;
            }
            //����ĳ��Ʈ ����� ��, �������� �ƴѰ�쿡�� ��Ȱ��ȭ
            else
            {
                ItemInfoDisappear();
            }
        }
        //����ĳ��Ʈ ����� ������ ��Ȱ��ȭ
        else
        {
            ItemInfoDisappear();
        }
    }



    /// <summary>
    /// ������ ���� �����ֱ⸦ ��Ȱ��ȭ �Ѵ�.
    /// </summary>
    private void ItemInfoDisappear()
    {
        //�Ⱦ� ��Ȱ��ȭ
        mIsPickupActive = false;

        //�ؽ�Ʈ ��Ȱ��ȭ
        // mItemRaycastInfoText.DisableText(); (�� �ۿ����� ���� X)

        //���� �������� null
        mCurrentItem = null;
    }

    /// <summary>
    /// ���� ���� �����ֱ⸦ ��Ȱ��ȭ �Ѵ�.
    /// </summary>
    private void BoxInfoDisappear()
    {
        //�ڽ� ��ȣ�ۿ� ��Ȱ��ȭ
        mIsBoxInteract = false;

        mChestController = null;

    }

    /// <summary>
    /// ���� ���� �����ֱ⸦ ��Ȱ��ȭ �Ѵ�.
    /// </summary>
    private void ShopInfoDisappear()
    {
        //���� ��ȣ�ۿ� ��Ȱ��ȭ
        mIsShopInteract = false;

        mItemShop = null;

    }


    /// <summary>
    /// �������� �����Ѵ�.
    /// </summary>
    private void TryPickUp()
    {
        if (mIsPickupActive)
        {
            // mItemActionCustomFunc.InteractionItem(mCurrentItem.Item, mCurrentItem.gameObject); (�� �ۿ����� ���� X)

            if (mCurrentItem.Item.Type != ItemType.NONE)
            {
                mInventory.AcquireItem(mCurrentItem.Item);
                Destroy(mCurrentItem.gameObject);
            }

            ItemInfoDisappear();
        }
    }
}
