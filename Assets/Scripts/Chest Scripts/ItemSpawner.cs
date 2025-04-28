using ChestSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{

    [Header("스폰 아이템 설정 ( 앞에 설정한 아이템 먼저 확률 계산이 이루어집니다.)")]
    [SerializeField] public SapwnItemInfo[] spawnItem;
   
    [System.Serializable]
    public class SapwnItemInfo
    {
        [Header("각 스폰할 아이템")]
        [SerializeField] public Transform itemPrefab;                

        [Header("해당 아이템의 스폰율 (0~1)")]
        [Range(0.0f, 1.0f)][SerializeField] public float spawnRate;
    }


    // Start is called before the first frame update
    void Start()
    {
        init();
    }




    private void init()
    {
        for (int i = 0; i < spawnItem.Length; ++i)
        {
            if (Random.value > spawnItem[i].spawnRate)
                continue;

            Instantiate(spawnItem[i].itemPrefab, transform.position, Quaternion.identity);
            break;
        }

    }

}
