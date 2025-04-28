using ChestSystem;
using UnityEngine;


/// <summary>
/// ���� �������� ���� ���� �⺻���� �κ��丮
/// </summary>
[System.Serializable]
public class InventoryMain : InventoryBase
{
    public static bool IsInventoryActive = false;  // �κ��丮 Ȱ��ȭ �Ǿ��°�?

    public static bool IsEquipmentActive = false; // ���â Ȱ��ȭ �Ǿ��°�?

    private PlayerInput playerInput = null;
    public static InventoryMain Instance = null;


    new void Awake()
    {
        if (Instance != null) { Destroy(Instance); } // ���ϼ��� �����ϱ� ����

        Instance = this;
        //DontDestroyOnLoad(gameObject);

        playerInput = GetComponent<PlayerInput>();
        base.Awake();
    }

    void Update()
    {
        TryOpenInventory();
    }



    /// <summary>
    /// �κ��丮�� IŰ�� ���� ���ų� �ݴ´�.
    /// </summary>
    public void TryOpenInventory()
    {
        //�ɼ��� �����ִ°�� ��Ȱ��ȭ
        // if (GameMenuManager.IsOptionActive) { return; }

        if (playerInput.inventory && ChestDialogManager.IsDialogActive == false && ShopSystem.Instance.mIsShopActive == false)
        {
            if (!IsInventoryActive)
                OpenInventory();
            else
                CloseInventory();

            if (!IsEquipmentActive)
                OpenEquipment();
            else
                CloseEquipment();
        }

    }

    /// <summary>
    /// �κ��丮�� ����.
    /// </summary>
    public void OpenInventory()
    {
        mInventoryBase.SetActive(true);
        IsInventoryActive = true;

        //Ŀ�� Ȱ��ȭ
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        UnityEngine.Cursor.visible = true;
    }

    /// <summary>
    /// �κ��丮�� �ݴ´�.
    /// </summary>
    public void CloseInventory()
    {
        mInventoryBase.SetActive(false);
        IsInventoryActive = false;

        //Ŀ�� ��Ȱ��ȭ
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }

    /// <summary>
    /// ���â�� ����.
    /// </summary>
    private void OpenEquipment()
    {
        mEquipmentBase.SetActive(true);
        IsEquipmentActive = true;

        //Ŀ�� Ȱ��ȭ
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        UnityEngine.Cursor.visible = true;
    }

    public void CloseEquipment()
    {
        mEquipmentBase.SetActive(false);
        IsEquipmentActive = false;

        //Ŀ�� ��Ȱ��ȭ
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }
    public void GetAllClear()
    {
        mEquipmentSlot.ClearSlot();
        foreach (var mSlot in mSlots)
        {
            mSlot.ClearSlot();
        }
        

    }

    /// <summary>
    /// Ư�� ������ ���Կ� �������� ��Ͻ�Ų��
    /// </summary>
    /// <param name="item">� ������?</param>
    /// <param name="targetSlot">��� ���Կ�?</param>
    /// <param name="count">������?></param>
    public void AcquireItem(Item item, InventorySlot targetSlot, int count = 1)
    {
        //��ø�� �����ϴٸ�?
        if (item.CanOverlap)
        {
            //����ũ�� ����Ͽ� �ش� ������ ����ũ�� ���Ǵ� ��ġ�ΰ�쿡�� �������� ����ֵ��� �Ѵ�.
            if (targetSlot.Item != null && targetSlot.IsMask(item))
            {
                if (targetSlot.Item.ItemID == item.ItemID)
                {
                    //���� ������ ������ ����(Count)�� �����Ѵ�.
                    targetSlot.UpdateSlotCount(count);
                }
            }
        }
        else
        {
            targetSlot.AddItem(item, count);
        }
    }

    public void AcquireItem(EntityCode itemCode, InventorySlot targetSlot, int count = 1)
    {
        // EntityCode�� �������� ã�ƿ´�.
        Item item = ItemDatabase.Instance.GetItemByCode(itemCode);
        if (item == null)
        {
            Debug.LogError("�������� ã�� �� �����ϴ�.");
            return;
        }

        // ��ø�� �����ϴٸ�
        if (item.CanOverlap)
        {
            // ����ũ�� ����Ͽ� �ش� ������ ����ũ�� ���Ǵ� ��ġ�� ��쿡�� �������� ����ֵ��� �Ѵ�.
            if (targetSlot.Item != null && targetSlot.IsMask(item))
            {
                if (targetSlot.Item.ItemID == item.ItemID)
                {
                    // ���� ������ ������ ����(Count)�� �����Ѵ�.
                    targetSlot.UpdateSlotCount(count);
                }
            }
        }
        else
        {
            targetSlot.AddItem(item, count);
        }
    }

    public void AcquireItem(Item item, int count = 1)
    {
        //��ø�� �����ϴٸ�?
        if (item.CanOverlap)
        {
            for (int i = 0; i < mSlots.Length; i++)
            {
                //����ũ�� ����Ͽ� �ش� ������ ����ũ�� ���Ǵ� ��ġ�ΰ�쿡�� �������� ����ֵ��� �Ѵ�.
                if (mSlots[i].Item != null && mSlots[i].IsMask(item))
                {
                    if (mSlots[i].Item.ItemID == item.ItemID)
                    {
                        //���� ������ ������ ����(Count)�� �����Ѵ�.
                        mSlots[i].UpdateSlotCount(count);
                        return;
                    }
                }
            }
        }

        //��� �������� �ƴѰ�� ���ο� ���Կ� ���´�.
        for (int i = 0; i < mSlots.Length; i++)
        {
            if (mSlots[i].Item == null && mSlots[i].IsMask(item))
            {
                mSlots[i].AddItem(item, count);
                return;
            }
        }
    }
    public InventorySlot[] GetAllItems()
    {
        return mSlots;
    }


    public void RefreshLabels()
    {
        // ���� �κ��丮 ���Ե��� ��ȸ�ϸ� ������ ����
        foreach (InventorySlot slot in mSlots)
        {
            // ���Կ� �������� �ִ� ��� �ش� UI�� ����
            if (slot.Item != null)
            {
                // ������ �̸�, ����, �̹��� ���� ����
                slot.mTextCount.text = slot.ItemCount.ToString(); // ���� ����
            }
            else
            {
                // �������� ���� ��� UI �ʱ�ȭ
                slot.mTextCount.text = string.Empty;
            }
        }

    }
}
