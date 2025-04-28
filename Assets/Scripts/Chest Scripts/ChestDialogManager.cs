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


        [Header("���̾�α� �ֻ��� ���ӿ�����Ʈ")]
        [SerializeField] public GameObject mDialogGo;

        [Header("���̾�α��� �׸��� ���̾ƿ� ������Ʈ")]
        [SerializeField] private GridLayoutGroup mGridLayout;

        [Header("�κ��丮 ���� ������Ʈ ������")]
        [SerializeField] private GameObject mSlotPrefab;

        private RectTransform mDialogRectTransform;
        private ChestController mCurrentChest; // ���� ���̾�α׹ڽ��� ǥ�õ� ������ ����

        private void Awake()
        {
            //�̱��� 
            if (Instance != null) { Destroy(Instance); } // ���ϼ��� �����ϱ� ����
            Instance = this;
            //DontDestroyOnLoad(gameObject);

            // �ʱ�ȭ�� ���� Ȱ��ȭ���� ����
            ChestDialogManager.mIsDialogActive = false;

            mDialogRectTransform = mDialogGo.GetComponent<RectTransform>();

            mDialogGo.SetActive(false);


            mAudioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
        }

        private void Update()
        {
            // ���̾�αװ� Ȱ�� ���¿��� ESC�� ������ ?
            if (mIsDialogActive && Input.GetKeyDown(KeyCode.Escape))
            {
                SoundManager.Instance.GetSound(mAudioSource, 9, 0.1f);

                CloseDialog();


                //Ŀ�� ��Ȱ��ȭ
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;
            }
                
        }

        public void TryOpenDialog(ChestController chestController)
        {
            // �̹� ���̾�αװ� Ȱ��ȭ �Ǿ��ִٸ�?
            if (mIsDialogActive)
                return;

            // ���� ������ ���
            mCurrentChest = chestController;

            // ���̾�α� ũ�� �� ���� �ʱ�ȭ
            InitDialog(mCurrentChest.ChestInfo.row, mCurrentChest.ChestInfo.col);

            foreach (ChestSlotItem slotItem in mCurrentChest.ChestInfo.chestSlotItems)
            {
                InventorySlot slot = mGridLayout.transform.GetChild(slotItem.itemPositionIndex).GetComponent<InventorySlot>();
                InventoryMain.Instance.AcquireItem(slotItem.itemCode, slot, slotItem.itemCount);
            }

            mDialogGo.SetActive(true);
            mIsDialogActive = true;

            InventoryMain.Instance.OpenInventory(); //�������� , OpenInventory -> ���� private

            //Ŀ�� Ȱ��ȭ
            UnityEngine.Cursor.lockState = CursorLockMode.Confined;
            UnityEngine.Cursor.visible = true;




            // mCurrentChest.PlayChestAnim(true);
        }

        public void CloseDialog()
        {
            // ���� ���� �κ��丮�� ��� ������ ������
            InventorySlot[] inventorySlotItems = mGridLayout.transform.GetComponentsInChildren<InventorySlot>(false);

            // ������ ���� ������ ����Ʈ�� �� ������ ������ 0�� �̻��� �����۵鸸 ����
            List<ChestSlotItem> slotItems = new List<ChestSlotItem>();
            for (int i = 0; i < inventorySlotItems.Length; ++i)
                if (inventorySlotItems[i].ItemCount > 0)
                    slotItems.Add(new ChestSlotItem()
                    {
                        itemCode = inventorySlotItems[i].Item.ID,
                        itemCount = inventorySlotItems[i].ItemCount,
                        itemPositionIndex = i,
                    });

            // ���� �������� ������ ��ġ
            mCurrentChest.ChestInfo.chestSlotItems.Clear();
            mCurrentChest.ChestInfo.chestSlotItems = slotItems;

            // ������Ʈ ��Ȱ��ȭ
            mDialogGo.SetActive(false);
            mIsDialogActive = false;

            InventoryMain.Instance.CloseInventory(); //�������� , ���� private���� ����Ǿ��־���

            //Ŀ�� ��Ȱ��ȭ
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;

            // ������ ���� ��Ȱ��ȭ
            // ItemDescription.Instance.DisableToolTip();

            // mCurrentChest.PlayChestAnim(false);
        }

        /// <summary>
        /// ���̾�α׸� �ʱ�ȭ�ϰ� �κ��丮 ���԰� ũ�⸦ �ڵ����� ����
        /// </summary>
        private void InitDialog(int row, int col)
        {
            // ���� �ڽ�(�κ��丮 ����) ���� ���ϱ�
            int currentSlotCount = mGridLayout.transform.childCount;

            // ������ �κ��丮 ���� �ν��Ͻ�
            for (int i = currentSlotCount; i < row * col; ++i)
                Instantiate(mSlotPrefab, Vector3.zero, Quaternion.identity, mGridLayout.transform);

            // ù��° �ε������� �� �������� ���� Ȱ��ȭ �� ���� �ʱ�ȭ
            for (int i = 0; i < row * col; ++i)
            {
                InventorySlot slot = mGridLayout.transform.GetChild(i).GetComponent<InventorySlot>();
                slot.ClearSlot();
                slot.gameObject.SetActive(true);
            }

            // �ʰ��ϴ� �κ��丮 ���� ��Ȱ��ȭ
            for (int i = row * col; i < currentSlotCount; ++i)
                mGridLayout.transform.GetChild(i).gameObject.SetActive(false);

            // �� �Ѱ��� ������ ���ϱ�
            float cellWidth = mGridLayout.cellSize.x + mGridLayout.spacing.x;
            float cellHeight = mGridLayout.cellSize.y + mGridLayout.spacing.y;

            // Ʈ�������� ����, ���� ���� ��� �� ����
            float width = row * cellWidth - mGridLayout.spacing.x;
            float height = col * cellHeight - mGridLayout.spacing.y;
            mDialogRectTransform.sizeDelta = new Vector2(width + mGridLayout.padding.left + mGridLayout.padding.right, height + mGridLayout.padding.top + mGridLayout.padding.bottom);
        }
    }
}