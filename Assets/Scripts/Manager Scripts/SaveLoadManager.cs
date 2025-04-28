using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using ChestSystem;
using System.Drawing.Text;
using UnityEngine.UI;
using System.Runtime.InteropServices.ComTypes;


public class SaveLoadManager : MonoBehaviour
{
    private ChestController mChestController;
    private GridLayoutGroup mGridLayout;
    public static SaveLoadManager Instance;
    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); } 

        Instance = this;
        DontDestroyOnLoad(gameObject);


        mGridLayout = GameObject.FindWithTag("Box").GetComponent<GridLayoutGroup>(); 
        mChestController = FindObjectOfType<ChestController>();
        

    }

    // Update is called once per frame
    void Update()
    {

    }

/*    public void LoadJson()
    {
        FileStream stream = new FileStream(Application.dataPath + "/test.json", FileMode.Open);
        byte[] data = new byte[stream.Length];
        stream.Read(data, 0, data.Length);
        stream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        InventoryMain jTest2 = JsonConvert.DeserializeObject<InventoryMain>(jsonData);

        InventoryMain.Instance = jTest2;
    }

    public void SaveJson()
    {

        FileStream stream = new FileStream(Application.dataPath + "/test.json", FileMode.OpenOrCreate);
        InventoryMain JTest1 = new InventoryMain();
        string JsonData2 = JsonConvert.SerializeObject(JTest1.mSlots);
        byte[] data = Encoding.UTF8.GetBytes(JsonData2);
        stream.Write(data, 0, data.Length);
        stream.Close();

    }*/
    public void SaveInventory()
    {
        List<InventorySlotData> slotDataList = new List<InventorySlotData>();

        foreach (var slot in InventoryMain.Instance.mSlots)
        {
            if (slot.Item != null) 
            {
                slotDataList.Add(new InventorySlotData
                {
                    itemID = slot.Item.ID,
                    itemCount = slot.ItemCount
                });
            }
        }

        string jsonData = JsonConvert.SerializeObject(slotDataList, Formatting.Indented);
        File.WriteAllText(Application.dataPath + "/test.json", jsonData);
    }



    public void LoadInventory()
    {
        string jsonData = File.ReadAllText(Application.dataPath + "/test.json");
        List<InventorySlotData> slotDataList = JsonConvert.DeserializeObject<List<InventorySlotData>>(jsonData);

        foreach (var slotData in slotDataList)
        {
            
            Item item = ItemDatabase.Instance.GetItemByCode(slotData.itemID);

           
            foreach (var slot in InventoryMain.Instance.mSlots)
            {
                if (slot.Item == null)
                {
                    slot.AddItem(item, slotData.itemCount);
                    break;
                }
            }
        }
    }

    public void SaveEquipment()
    {
        List<InventorySlotData> slotData = new List<InventorySlotData>();
        var slot = InventoryMain.Instance.mEquipmentSlot;   

        if (slot.Item != null)
        {
            slotData.Add(new InventorySlotData
            {
                itemID = slot.Item.ID,
                itemCount = slot.ItemCount
            });


            /*            slotData.itemID = slot.Item.ID;
                        slotData.itemCount = slot.ItemCount;*/
        }

        

        string jsonData = JsonConvert.SerializeObject(slotData, Formatting.Indented);
        File.WriteAllText(Application.dataPath + "/Equipment.json", jsonData);
    }


    public void LoadEquipment()
    {
        string jsonData = File.ReadAllText(Application.dataPath + "/Equipment.json");
        List<InventorySlotData> slotDataList = JsonConvert.DeserializeObject<List<InventorySlotData>>(jsonData);


        foreach (var slotData in slotDataList)
        {
            Item item = ItemDatabase.Instance.GetItemByCode(slotData.itemID);
            if (InventoryMain.Instance.mEquipmentSlot.Item == null)
            {
                InventoryMain.Instance.mEquipmentSlot.AddItem(item, slotData.itemCount);
                break;
            }     
        }
/*        var slot = slotData;
        Item item = ItemDatabase.Instance.GetItemByCode(slot.itemID);

        var slot2 = InventoryMain.Instance.mEquipmentSlot;
        if (slot2.Item == null)
        {
            slot2.AddItem(item, slotData.itemCount);
        }*/
    }

    public void SaveMoney()
    {

        FileStream stream = new FileStream(Application.dataPath + "/Money.json", FileMode.OpenOrCreate);
        int JTest1 = Money.MONEY;

        string JsonData2 = JsonConvert.SerializeObject(JTest1);
        byte[] data = Encoding.UTF8.GetBytes(JsonData2);
        stream.Write(data, 0, data.Length);
        stream.Close();
    }

    public void LoadMoney()
    {
        FileStream stream = new FileStream(Application.dataPath + "/Money.json", FileMode.Open);
        byte[] data = new byte[stream.Length];
        stream.Read(data, 0, data.Length);
        stream.Close();
        string jsonData = Encoding.UTF8.GetString(data);
        int jTest2 = JsonConvert.DeserializeObject<int>(jsonData);

        Money.MONEY = jTest2;

    }


    public void SaveChest()
    {
       
        List<ChestSlotData> slotDataList = new List<ChestSlotData>();

        foreach (var slot in mChestController.ChestInfo.chestSlotItems)
        {
            if (slot.itemCode != 0)
            {
                slotDataList.Add(new ChestSlotData
                {
                    itemCode = slot.itemCode,
                    itemCount = slot.itemCount,
                    itemPositionIndex = slot.itemPositionIndex,
                });
            }
        }

        string jsonData = JsonConvert.SerializeObject(slotDataList, Formatting.Indented);
        File.WriteAllText(Application.dataPath + "/Chest.json", jsonData);
    }



    public void LoadChest()
    {
        string jsonData = File.ReadAllText(Application.dataPath + "/Chest.json");
        List<ChestSlotData> slotDataList = JsonConvert.DeserializeObject<List<ChestSlotData>>(jsonData);
        List<ChestSlotItem> slotItems = new List<ChestSlotItem>();

        foreach (var slotData in slotDataList)
        {
            slotItems.Add(new ChestSlotItem()
            {
                itemCode = slotData.itemCode,
                itemCount = slotData.itemCount,
                itemPositionIndex = slotData.itemPositionIndex,
            });
           
        }

        foreach (ChestSlotItem slotItem in slotItems)
        {
            InventorySlot slot = mGridLayout.transform.GetChild(slotItem.itemPositionIndex).GetComponent<InventorySlot>();
            InventoryMain.Instance.AcquireItem(slotItem.itemCode, slot, slotItem.itemCount);
            
        }
    }

    [System.Serializable]
    public class InventorySlotData
    {
        public EntityCode itemID; 
        public int itemCount; 
    }


    [System.Serializable]
    public class ChestSlotData
    {
        public EntityCode itemCode;
        public int itemCount;
        public int itemPositionIndex;
    }

}
