using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    private ShopItem mItem;
    private InventorySlot[] mSlots;
    private  AudioSource mAudioSource;
    // Start is called before the first frame update
    void Awake()
    {
        mItem = GetComponent<ShopItem>();

        mSlots = InventoryMain.Instance.GetAllItems();
        mAudioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
    }




    public void BuyClick()
    {
        if(Money.Instance.GetMoney() >= mItem.GetBuyPrice()) 
        {
            Money.Instance.SetMoney(Money.Instance.GetMoney() - mItem.GetBuyPrice());
            InventoryMain.Instance.AcquireItem(mItem.GetItem());
            SaveLoadManager.Instance.SaveMoney();
            SoundManager.Instance.GetSound(mAudioSource, 10, 0.1f);
        }
        else 
        { 
            Debug.Log("��ȭ ����");
        } 

        Debug.Log("���� ����: " + mItem.GetBuyPrice());
        Debug.Log("������ �ڵ�: " + mItem.GetItemID());
        Debug.Log("���� �� ����: " + Money.Instance.GetMoney());
    }



    public void SellClick()
    {
        mSlots = InventoryMain.Instance.GetAllItems();


        for (int i = 0; i < mSlots.Length; i++)
        {
            if (mSlots[i].Item != null && mSlots[i].Item.ItemID == mItem.GetItemID())
            {
                SoundManager.Instance.GetSound(mAudioSource, 14, 0.3f);
                mSlots[i].UpdateSlotCount(-1);
                Money.Instance.SetMoney(Money.Instance.GetMoney() + mItem.GetSellPrice());
                SaveLoadManager.Instance.SaveMoney();
                break;
            }
            else
            {
                Debug.Log("�ش� �������� �����ϴ�.");
            }
        }


        Debug.Log("���� ����: " + mItem.GetBuyPrice());
        Debug.Log("������ �ڵ�: " + mItem.GetItemID());
        Debug.Log("���� �� ����: " + Money.Instance.GetMoney());
    }





}
