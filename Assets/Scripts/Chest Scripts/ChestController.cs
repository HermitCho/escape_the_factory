using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ChestSystem
{
    [System.Serializable]
    public struct ChestSlotItem
    {
        [Header("아이템 코드")]
        [SerializeField] public EntityCode itemCode;

        [HideInInspector] public int itemCount;
        [HideInInspector] public int itemPositionIndex; // 아이템의 슬롯 위치
    }

    [System.Serializable]
    public struct ChestItemSpawnConfig
    {
        [Header("\n초기화 시 아이템 스폰 설정")]

        [Header("스폰을 시도할 아이템의 코드")]
        [SerializeField] public EntityCode itemCode;

        [Header("아이템의 스폰율 (0~1)")]
        [Range(0.0f, 1.0f)][SerializeField] public float spawnRate;

        [Header("아이템의 최소 스폰 개수")]
        [SerializeField] public int minItemCount;

        [Header("아이템의 최대 스폰 개수")]
        [SerializeField] public int maxItemCount;
    }

    [System.Serializable]
    public class ChestInfo
    {
        [Header("상자의 고유 ID")]
        [SerializeField] public int chestUniqueId;

        [Header("상자 인벤토리의 가로 셀 개수")]
        [Range(1, 10)][SerializeField] public int row;

        [Header("상자 인벤토리의 세로 셀 개수")]
        [Range(1, 10)][SerializeField] public int col;

        [Header("초기화 시 상자를 초기화 할 설정값")]
        [SerializeField] public ChestItemSpawnConfig[] chestItemSpawnConfig;

        // 상자에 있는 아이템의 정보
        [HideInInspector] public List<ChestSlotItem> chestSlotItems;
    }

    public class ChestController : MonoBehaviour
    {
        [Header("초기화 시 상자의 정보")]
        [SerializeField] public ChestInfo ChestInfo;

/*        [field: Header("상자의 타입")]
        [field: SerializeField] public ChestType ChestType { private set; get; } = ChestType.HEAVY_WOOD;

        private Animator mChestAnimator; // 상자의 애니메이션 컨트롤러*/

        private bool mIsInitReady = true; // 현재 초기화 대기상태인가?
        public bool IsInitReady
        {
            get
            {
                return mIsInitReady;
            }
        }

        public int ChestUniqueId
        {
            get
            {
                return ChestInfo.chestUniqueId;
            }
        }

        private void Awake()
        {
            //mChestAnimator = GetComponentInChildren<Animator>();

                
            
        }

        public void TryOpenDialog()
        {
            // 만약 초기화 대기 상태라면?
            if (mIsInitReady == true)
                Init();

            // 상자 다이얼로그 열기
            ChestDialogManager.Instance.TryOpenDialog(this);
        }

        // 최초로 해당 상자를 여는경우 무작위 초기화 설정
        private void Init()
        {
            // 최초 1회 상자를 열었음을 설정
            mIsInitReady = false;

            // 무작위 unique 값을 리턴하기위한 리스트
            List<int> randPosGen = new List<int>();
            for (int i = 0; i < ChestInfo.row * ChestInfo.col; ++i)
                randPosGen.Add(i);

            // 상자 아이템 리스트 생성
            ChestInfo.chestSlotItems = new List<ChestSlotItem>();

            for (int i = 0; i < ChestInfo.chestItemSpawnConfig.Length; ++i)
            {
                // 확률을 계산하여 확률에 미치지 못하는경우 리턴
                if (Random.value > ChestInfo.chestItemSpawnConfig[i].spawnRate)
                    continue;

                // 아이템 한개 생성
                ChestSlotItem slotItem = new ChestSlotItem();
                {
                    // 해당 아이템의 개수를 범위 내 랜덤으로 지정
                    slotItem.itemCount = Random.Range(ChestInfo.chestItemSpawnConfig[i].minItemCount, ChestInfo.chestItemSpawnConfig[i].maxItemCount + 1);

                    // 무작위 위치를 지정
                    int randIndex = Random.Range(0, randPosGen.Count);

                    // 고유한 무작위 값 가져오기
                    int randPos = randPosGen[randIndex];

                    // 해당 위치의 값 요소 제거 (고유성 보장)
                    randPosGen.RemoveAt(randIndex);

                    // 값 지정
                    slotItem.itemPositionIndex = randPos;

                    // 아이템코드 가져오기
                    slotItem.itemCode = ChestInfo.chestItemSpawnConfig[i].itemCode;

                    // 아이템을 리스트에 삽입
                    ChestInfo.chestSlotItems.Add(slotItem);
                }
            }
        }
    }
}

