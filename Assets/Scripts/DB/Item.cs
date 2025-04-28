using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum ItemType  // ������ ����
{
    /// <summary>
    /// NONE Type�� �������� �����ϱ����� EŰ�� �������, �κ��丮�� ������ �ʴ´�.
    /// Ư���� ��ȣ�ۿ��� �ִ� ������Ʈ�� ����Ѵ�.
    /// </summary>
    NONE = 0b0, //0
    SKILL = 0b1, //1

    //��� ������ ����
    //��� ������ Ÿ�Կ��� �߰��Ǵ°��, �����ϴ� ������ �߰��Ѵ�.
    Equipment_HELMET = 0b10, //2
    Equipment_ARMORPLATE = 0b100, //4
    Equipment_GLOVE = 0b1000, //8
    Equipment_PANTS = 0b10000, //16
    Equipment_SHOES = 0b100000, //32

    //��� �������� �ƴ� �����۵�(�Ҹ�, ��Ÿ, ���, ����Ʈ������ ���)
    Etc = 0b1000000, //64
    Consumable = 0b10000000, //128
    Ingredient = 0b100000000, //256
    Quest = 0b1000000000, //512
}


public enum EntityCode  // ������ �ڵ�
{
    ITEM_NONE = 0,
    ITEM_AXE = 1,
    ITEM_PAINKILLER = 2,
    ITEM_APPLE = 3,
    ITEM_INGOT = 4,
    ITEM_COIN = 5,
    ITEM_BOOK = 6,
    ITEM_RING = 7,
}


[CreateAssetMenu(fileName = "Item", menuName = "Add Item/Item")]
public class Item : ScriptableObject  // ���� ������Ʈ�� ���� �ʿ� X 
{


    [Header("������ �������� ID(�ߺ��Ұ�)")]
    [SerializeField] private int mItemID;
    /// <summary>
    /// �������� ���� ��ȣ
    /// </summary>
    /// <value></value>
    public int ItemID
    {
        get
        {
            return mItemID;
        }
    }

    [Header("������ �������� �ڵ� [ ID�� ���� ��]")]
    [SerializeField] private EntityCode mID;
    /// <summary>
    /// �������� ���� ��ȣ
    /// </summary>
    /// <value></value>
    public EntityCode ID
    {
        get
        {
            return mID;
        }

        set
        {
            mID = value;
        }

    }

    [Header("�������� ��ø�� �����Ѱ�?")]
    [SerializeField] private bool mCanOverlap;
    /// <summary>
    /// �������� ��ø�� �����Ѱ�?
    /// </summary>
    /// <value></value>
    public bool CanOverlap
    {
        get
        {
            return mCanOverlap;
        }
    }

    [Header("���(��ȣ�ۿ�)�� ������ �������ΰ�?")]
    [SerializeField] private bool mIsInteractivity;
    /// <summary>
    /// ���(��ȣ�ۿ�)�� ������ �������ΰ�?
    /// </summary>
    /// <value></value>
    public bool IsInteractivity
    {
        get
        {
            return mIsInteractivity;
        }
    }

    [Header("�������� ����ϸ� ������°�?")]
    [SerializeField] private bool mIsConsumable;
    /// <summary>
    /// �������� ����ϸ� �Ѱ��� ������°�?
    /// </summary>
    /// <value></value>
    public bool IsConsumable
    {
        get
        {
            return mIsConsumable;
        }
    }

    [Header("�������� ���� ��Ÿ��")]
    [SerializeField] private float mItemCooltime = -1;
    /// <summary>
    /// �������� ��Ÿ��
    /// </summary>
    /// <value></value>
    public float Cooltime
    {
        get
        {
            return mItemCooltime;
        }
    }

    [Header("�������� Ÿ��")]
    [SerializeField] private ItemType mItemType;
    /// <summary>
    /// �������� ����
    /// </summary>
    /// <value></value>
    public ItemType Type
    {
        get
        {
            return mItemType;
        }
    }

    [Header("�κ��丮���� ������ �������� �̹���")]
    [SerializeField] private Sprite mItemImage;
    public Sprite Image
    {
        get
        {
            return mItemImage;
        }
    }
    [Header("�������� �̸�")]
    [SerializeField] private string mItemName;
    public string ItemName
    {
        get
        {
            return mItemName;
        }
    }

    [Header("�������� ����")]
    [SerializeField] private string mItemDescription;
    public string ItemDescription
    {
        get
        {
            return mItemDescription;
        }
    }

    [Header("�������� ���Ű���")]
    [SerializeField] private int mBuyPrice;
    public int BuyPrice
    {
        get
        {
            return mBuyPrice;
        }   
    }

    [Header("�������� �ǸŰ���")]
    [SerializeField] private int mSellPrice;
    public int SellPrice
    {
        get
        {
            return mSellPrice;
        }
    }
}
