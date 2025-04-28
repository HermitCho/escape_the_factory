using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private Item mItem;
   // [SerializeField] private Money mMoney; 
    [HideInInspector] private int buyPrice;
    [HideInInspector] private int sellPrice;

    public int GetItemID()
    {
        return mItem.ItemID;
    }

    public Item GetItem()
    {
        return mItem;
    }

    public int GetBuyPrice()
    {
        buyPrice = mItem.BuyPrice;

        return buyPrice;
    }

    public int GetSellPrice()
    {
        sellPrice = mItem.SellPrice;

        return sellPrice;
    }


}
