using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace ChestSystem
{
    public class ChestDialogManager : MonoBehaviour
    {
        public static ChestDialogManager Instance;
        private static bool mIsDialogActive = false;
        private AudioSource mAudioSource;
        public static bool IsDialogActive
        {
            get
            {
                return mIsDialogActive;
            }
        }


        [Header("다이얼로그 최상위 게임오브젝트")]
        [SerializeField] public GameObject mDialogGo;

        [Header("다이얼로그의 그리드 레이아웃 컴포넌트")]
        [SerializeField] private GridLayoutGroup mGridLayout;

        [Header("인벤토리 슬롯 오브젝트 프리팹")]
        [SerializeField] private GameObject mSlotPrefab;

        private RectTransform mDialogRectTransform;
        private ChestController mCurrentChest; // 현재 다이얼로그박스에 표시된 상자의 정보

        private void Awake()
        {
            //싱글턴 
            if (Instance != null) { Destroy(Instance); } // 유일성을 보장하기 위해
            Instance = this;
            //DontDestroyOnLoad(gameObject);

            // 초기화시 전역 활성화상태 해제
            ChestDialogManager.mIsDialogActive = false;

            mDialogRectTransform = mDialogGo.GetComponent<RectTransform>();

            mDialogGo.SetActive(false);


            mAudioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
        }

        private void Update()
        {
            // 다이얼로그가 활성 상태에서 ESC를 누르면 ?
            if (mIsDialogActive && Input.GetKeyDown(KeyCode.Escape))
            {
                SoundManager.Instance.GetSound(mAudioSource, 9, 0.1f);

                CloseDialog();


                //커서 비활성화
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
            }
                
        }

        public void TryOpenDialog(ChestController chestController)
        {
            // 이미 다이얼로그가 활성화 되어있다면?
            if (mIsDialogActive)
                return;

            // 현재 정보를 등록
            mCurrentChest = chestController;

            // 다이얼로그 크기 및 슬롯 초기화
            InitDialog(mCurrentChest.ChestInfo.row, mCurrentChest.ChestInfo.col);

            foreach (ChestSlotItem slotItem in mCurrentChest.ChestInfo.chestSlotItems)
            {
                InventorySlot slot = mGridLayout.transform.GetChild(slotItem.itemPositionIndex).GetComponent<InventorySlot>();
                InventoryMain.Instance.AcquireItem(slotItem.itemCode, slot, slotItem.itemCount);
            }

            mDialogGo.SetActive(true);
            mIsDialogActive = true;

            InventoryMain.Instance.OpenInventory(); //수정사항 , OpenInventory -> 원래 private

            //커서 활성화
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
            UnityEngine.Cursor.visible = true;




            // mCurrentChest.PlayChestAnim(true);
        }

        public void CloseDialog()
        {
            // 현재 상자 인벤토리의 모든 슬롯을 가져옴
            InventorySlot[] inventorySlotItems = mGridLayout.transform.GetComponentsInChildren<InventorySlot>(false);

            // 아이템 슬롯 데이터 리스트를 를 생성후 개수가 0개 이상의 아이템들만 갱신
            List<ChestSlotItem> slotItems = new List<ChestSlotItem>();
            for (int i = 0; i < inventorySlotItems.Length; ++i)
                if (inventorySlotItems[i].ItemCount > 0)
                    slotItems.Add(new ChestSlotItem()
                    {
                        itemCode = inventorySlotItems[i].Item.ID,
                        itemCount = inventorySlotItems[i].ItemCount,
                        itemPositionIndex = i,
                    });

            // 슬롯 아이템의 정보를 대치
            mCurrentChest.ChestInfo.chestSlotItems.Clear();
            mCurrentChest.ChestInfo.chestSlotItems = slotItems;

            // 오브젝트 비활성화
            mDialogGo.SetActive(false);
            mIsDialogActive = false;

            InventoryMain.Instance.CloseInventory(); //수정사항 , 원래 private으로 선언되어있었음

            //커서 비활성화
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;

            // 아이템 설명 비활성화
            // ItemDescription.Instance.DisableToolTip();

            // mCurrentChest.PlayChestAnim(false);
        }

        /// <summary>
        /// 다이얼로그를 초기화하고 인벤토리 슬롯과 크기를 자동으로 조정
        /// </summary>
        private void InitDialog(int row, int col)
        {
            // 현재 자식(인벤토리 슬롯) 개수 구하기
            int currentSlotCount = mGridLayout.transform.childCount;

            // 부족한 인벤토리 슬롯 인스턴스
            for (int i = currentSlotCount; i < row * col; ++i)
                Instantiate(mSlotPrefab, Vector3.zero, Quaternion.identity, mGridLayout.transform);

            // 첫번째 인덱스부터 총 개수까지 슬롯 활성화 및 슬롯 초기화
            for (int i = 0; i < row * col; ++i)
            {
                InventorySlot slot = mGridLayout.transform.GetChild(i).GetComponent<InventorySlot>();
                slot.ClearSlot();
                slot.gameObject.SetActive(true);
            }

            // 초과하는 인벤토리 슬롯 비활성화
            for (int i = row * col; i < currentSlotCount; ++i)
                mGridLayout.transform.GetChild(i).gameObject.SetActive(false);

            // 셀 한개의 사이즈 구하기
            float cellWidth = mGridLayout.cellSize.x + mGridLayout.spacing.x;
            float cellHeight = mGridLayout.cellSize.y + mGridLayout.spacing.y;

            // 트랜스폼의 가로, 세로 길이 계산 및 적용
            float width = row * cellWidth - mGridLayout.spacing.x;
            float height = col * cellHeight - mGridLayout.spacing.y;
            mDialogRectTransform.sizeDelta = new Vector2(width + mGridLayout.padding.left + mGridLayout.padding.right, height + mGridLayout.padding.top + mGridLayout.padding.bottom);
        }
    }
}