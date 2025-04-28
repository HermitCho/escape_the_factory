using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ChestSystem
{
    [System.Serializable]
    public struct ChestSlotItem
    {
        [Header("������ �ڵ�")]
        [SerializeField] public EntityCode itemCode;

        [HideInInspector] public int itemCount;
        [HideInInspector] public int itemPositionIndex; // �������� ���� ��ġ
    }

    [System.Serializable]
    public struct ChestItemSpawnConfig
    {
        [Header("\n�ʱ�ȭ �� ������ ���� ����")]

        [Header("������ �õ��� �������� �ڵ�")]
        [SerializeField] public EntityCode itemCode;

        [Header("�������� ������ (0~1)")]
        [Range(0.0f, 1.0f)][SerializeField] public float spawnRate;

        [Header("�������� �ּ� ���� ����")]
        [SerializeField] public int minItemCount;

        [Header("�������� �ִ� ���� ����")]
        [SerializeField] public int maxItemCount;
    }

    [System.Serializable]
    public class ChestInfo
    {
        [Header("������ ���� ID")]
        [SerializeField] public int chestUniqueId;

        [Header("���� �κ��丮�� ���� �� ����")]
        [Range(1, 10)][SerializeField] public int row;

        [Header("���� �κ��丮�� ���� �� ����")]
        [Range(1, 10)][SerializeField] public int col;

        [Header("�ʱ�ȭ �� ���ڸ� �ʱ�ȭ �� ������")]
        [SerializeField] public ChestItemSpawnConfig[] chestItemSpawnConfig;

        // ���ڿ� �ִ� �������� ����
        [HideInInspector] public List<ChestSlotItem> chestSlotItems;
    }

    public class ChestController : MonoBehaviour
    {
        [Header("�ʱ�ȭ �� ������ ����")]
        [SerializeField] public ChestInfo ChestInfo;

/*        [field: Header("������ Ÿ��")]
        [field: SerializeField] public ChestType ChestType { private set; get; } = ChestType.HEAVY_WOOD;

        private Animator mChestAnimator; // ������ �ִϸ��̼� ��Ʈ�ѷ�*/

        private bool mIsInitReady = true; // ���� �ʱ�ȭ �������ΰ�?
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
            // ���� �ʱ�ȭ ��� ���¶��?
            if (mIsInitReady == true)
                Init();

            // ���� ���̾�α� ����
            ChestDialogManager.Instance.TryOpenDialog(this);
        }

        // ���ʷ� �ش� ���ڸ� ���°�� ������ �ʱ�ȭ ����
        private void Init()
        {
            // ���� 1ȸ ���ڸ� �������� ����
            mIsInitReady = false;

            // ������ unique ���� �����ϱ����� ����Ʈ
            List<int> randPosGen = new List<int>();
            for (int i = 0; i < ChestInfo.row * ChestInfo.col; ++i)
                randPosGen.Add(i);

            // ���� ������ ����Ʈ ����
            ChestInfo.chestSlotItems = new List<ChestSlotItem>();

            for (int i = 0; i < ChestInfo.chestItemSpawnConfig.Length; ++i)
            {
                // Ȯ���� ����Ͽ� Ȯ���� ��ġ�� ���ϴ°�� ����
                if (Random.value > ChestInfo.chestItemSpawnConfig[i].spawnRate)
                    continue;

                // ������ �Ѱ� ����
                ChestSlotItem slotItem = new ChestSlotItem();
                {
                    // �ش� �������� ������ ���� �� �������� ����
                    slotItem.itemCount = Random.Range(ChestInfo.chestItemSpawnConfig[i].minItemCount, ChestInfo.chestItemSpawnConfig[i].maxItemCount + 1);

                    // ������ ��ġ�� ����
                    int randIndex = Random.Range(0, randPosGen.Count);

                    // ������ ������ �� ��������
                    int randPos = randPosGen[randIndex];

                    // �ش� ��ġ�� �� ��� ���� (������ ����)
                    randPosGen.RemoveAt(randIndex);

                    // �� ����
                    slotItem.itemPositionIndex = randPos;

                    // �������ڵ� ��������
                    slotItem.itemCode = ChestInfo.chestItemSpawnConfig[i].itemCode;

                    // �������� ����Ʈ�� ����
                    ChestInfo.chestSlotItems.Add(slotItem);
                }
            }
        }
    }
}

