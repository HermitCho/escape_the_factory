using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [SerializeField] private List<Item> items; // 모든 아이템들

    private Dictionary<EntityCode, Item> itemDictionary;
    public static ItemDatabase Instance;

    private void Awake()
    {
        if (Instance != null) { Destroy(Instance); } // 유일성을 보장하기 위해

        Instance = this;
        //DontDestroyOnLoad(gameObject);


        // Dictionary 초기화
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
    /// EntityCode를 통해 아이템을 가져오는 메서드
    /// </summary>
    /// <param name="code">아이템 코드</param>
    /// <returns>아이템 정보</returns>
    public Item GetItemByCode(EntityCode code)
    {
        itemDictionary.TryGetValue(code, out Item item);
        return item; // 없으면 null 반환
    }


    public string GetName(EntityCode code)
    {

        itemDictionary.TryGetValue(code, out Item item);
        return item.ItemName;
    }
}