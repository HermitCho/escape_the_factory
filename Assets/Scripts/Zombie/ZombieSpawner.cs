using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public Transform[] spawnPoints; // 스폰 위치 배열
    public GameObject monsterPrefab; // 하나의 몬스터 프리팹
    private int minSpawnCount = 20; // 최소 스폰 개수
    private int maxSpawnCount = 30; // 최대 스폰 개수

    private List<GameObject> monsterPool; // 몬스터 오브젝트 풀
    private List<int> availableSpawnPoints; // 스폰 가능한 위치 인덱스 리스트

    private void Start()
    {
        // 몬스터 오브젝트 풀 초기화
        monsterPool = new List<GameObject>();

        // 스폰 가능한 위치 인덱스 초기화
        availableSpawnPoints = new List<int>();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            availableSpawnPoints.Add(i);
        }

        // 몬스터를 초기화하고 랜덤하게 스폰
        InitializeMonsterPool();
        SpawnRandomZombies();
    }

    private void InitializeMonsterPool()
    {
        // 충분한 수의 몬스터를 오브젝트 풀에 추가
        int poolSize = maxSpawnCount; // 최대 개수만큼 생성
        for (int i = 0; i < poolSize; i++)
        {
            GameObject monster = Instantiate(monsterPrefab);
            monster.SetActive(false); // 초기에는 비활성화 상태
            monsterPool.Add(monster);
        }
    }

    private void SpawnRandomZombies()
    {
        // 랜덤 스폰 개수를 결정
        int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);

        for (int i = 0; i < spawnCount; i++)
        {
            if (availableSpawnPoints.Count == 0)
            {
                Debug.LogWarning("No more available spawn points.");
                break;
            }

            // 스폰 가능한 위치를 랜덤으로 선택
            int spawnIndex = Random.Range(0, availableSpawnPoints.Count);
            int spawnPointIndex = availableSpawnPoints[spawnIndex];

            // 몬스터를 오브젝트 풀에서 가져옴
            GameObject monster = GetOrCreateMonsterFromPool();
            monster.transform.position = spawnPoints[spawnPointIndex].position;
            monster.SetActive(true);

            // 해당 위치는 더 이상 사용하지 않음
            availableSpawnPoints.RemoveAt(spawnIndex);
        }
    }

    private GameObject GetOrCreateMonsterFromPool()
    {
        // 비활성화된 몬스터를 풀에서 찾음
        GameObject monster = monsterPool.Find(m => !m.activeSelf);
        if (monster == null)
        {
            // 새로운 몬스터를 생성하여 풀에 추가
            monster = Instantiate(monsterPrefab);
            monster.SetActive(false);
            monsterPool.Add(monster);
        }
        return monster;
    }
}