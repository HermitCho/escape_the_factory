using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [SerializeField] private List<Item> items; // ��� �����۵�

    private Dictionary<EntityCode, Item> itemDictionary;
    public static ItemDatabase Instance;

    private void Awake()
    {
        if (Instance != null) { Destroy(Instance); } // ���ϼ��� �����ϱ� ����

        Instance = this;
        //DontDestroyOnLoad(gameObject);


        // Dictionary �ʱ�ȭ
        itemDictionary = new Dictionary<EntityCode, Item>();
        foreach (Item item in items)
        {
            if (!itemDictionary.ContainsKey(item.ID))
            {
                itemDictionary.Add(item.ID, item);
            }
        }
        
    }

    /// <summary>
    /// EntityCode�� ���� �������� �������� �޼���
    /// </summary>
    /// <param name="code">������ �ڵ�</param>
    /// <returns>������ ����</returns>
    public Item GetItemByCode(EntityCode code)
    {
        itemDictionary.TryGetValue(code, out Item item);
        return item; // ������ null ��ȯ
    }


    public string GetName(EntityCode code)
    {

        itemDictionary.TryGetValue(code, out Item item);
        return item.ItemName;
    }
}