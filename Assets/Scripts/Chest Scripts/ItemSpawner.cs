using ChestSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{

    [Header("���� ������ ���� ( �տ� ������ ������ ���� Ȯ�� ����� �̷�����ϴ�.)")]
    [SerializeField] public SapwnItemInfo[] spawnItem;
   
    [System.Serializable]
    public class SapwnItemInfo
    {
        [Header("�� ������ ������")]
        [SerializeField] public Transform itemPrefab;                

        [Header("�ش� �������� ������ (0~1)")]
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
