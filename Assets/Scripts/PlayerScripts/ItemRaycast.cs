using ChestSystem;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;



/// <summary>
/// 씬 내의 아이템(또는 정적 물체)에 다가가면 해당 아이템을 줍거나, 상호작용 할 수 있도록 해주는 스크립트
/// 플레이어의 오브젝트에 자식으로 넣은 EmptyObject에 Trigger Collider을 추가하여 사용
/// </summary>
public class ItemRaycast : MonoBehaviour
{

    private AudioSource UIAudio;

    /// <summary>
    /// 레이캐스트 된 아이템
    /// </summary>
    private RaycastHit mHit;

    /// <summary>
    /// 레이캐스트 거리
    /// </summary>
    [SerializeField] private float mRayDistance;


    private PlayerInput playerInput;

    [HideInInspector] public bool mIsPickupActive = false;  //아이템 습득이 가능한가?
    [HideInInspector] public bool mIsBoxInteract= false; //박스 상호작용이 가능한가?
    [HideInInspector] public bool mIsShopInteract = false;//상점 상호작용이 가능한가?
    [HideInInspector] public bool mIsEquipment = false;

    private ItemPickUp mCurrentItem; //활성화시 현재 등록된 아이템
    private ChestController mChestController; //활성화시 현재 등록된 박스 
    private ShopSystem mItemShop; //활성화시 현재 등록된 상점 

    [Header("레이캐스트를 쏠 카메라")]
    [SerializeField] private Camera mRayCamera; //레이를 쏠 카메라 (메인카메라)

    [SerializeField] private InventoryMain mInventory; //인벤토리 메인


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        UIAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        CheckItem();
        CheckChest();
        CheckShop();

        if (mIsPickupActive) { TryPickItem(); }

        else if (mIsBoxInteract && ShopSystem.Instance.mIsShopActive == false && InventoryMain.IsInventoryActive == false)
        { 
            TryOpenChest();

            InventoryMain.Instance.TryOpenInventory();
        }
        else if (mIsShopInteract && ChestDialogManager.IsDialogActive == false && InventoryMain.IsInventoryActive == false)
        {
            TryOpenShop();

            InventoryMain.Instance.TryOpenInventory();

        }
        else { return; }

        if(mCurrentItem.IndicatorHeight <= 0f)
        {
            mIsEquipment = true;
        }
        else
        {
            mIsEquipment = false;
        }
    }


   

    private void CheckShop()
    {
        if (Physics.Raycast(mRayCamera.transform.position, mRayCamera.transform.forward, out mHit, mRayDistance))
        {
            //레이캐스트 결과의 태그가 상점이라면?
            if (mHit.transform.tag == "Shop")
            {
                //현재 레이캐스트된 상점
                ShopSystem rayCastedShop = mHit.transform.GetComponent<ShopSystem>();
                //레이캐스트 결과가 현재 아이템과 같으면 무시 (중복호출 방지)
                if (mItemShop == rayCastedShop) { return; }

                mItemShop = mHit.transform.GetComponent<ShopSystem>();


                Debug.LogFormat("상점 열기 가능");

                mIsShopInteract = true;
                return;
            }
            else
            {
                ShopInfoDisappear();
            }
        }
        else
        {
            ShopInfoDisappear();
        }
    }

    /// <summary>
    /// 상점을 열 수 있는지 확인한다.
    /// </summary>
    private void TryOpenShop()
    {
        if (playerInput.interact)
        {
            SoundManager.Instance.GetSound(UIAudio, 10, 0.4f, 2f, 1f);
            mItemShop.TryOpenShop();
            Debug.Log("상점 열림");
        }
    }

    /// <summary>
    /// 상자를 열 수 있는지 확인한다.
    /// </summary>
    private void TryOpenChest()
    {
        if (playerInput.interact)
        {
            SoundManager.Instance.GetSound(UIAudio, 8, 0.3f);
            mChestController.TryOpenDialog();
            
        }
    }

    /// <summary>
    /// 아이템을 주울 수 있는지 확인한다.
    /// </summary>
    private void TryPickItem()
    {
        if (playerInput.interact)
        {
            SoundManager.Instance.GetSound(UIAudio, 7, 0.3f);
            //주울 수 있는 아이템이라면?
            if (mCurrentItem.Item.Type > ItemType.NONE && !mIsEquipment)
            {
                //현재 인벤토리 아이템 가져오기
                InventorySlot[] allitems = mInventory.GetAllItems();

                int count = 0;
                for (; count < allitems.Length; ++count)
                {
                    //현재 아이템 칸이 null이라면 주울 수 있는 상태
                    if (allitems[count].Item == null) { break; }

                    //현재 아이템칸이 null이 아니지만, 현재 아이템과 동일하면서 중첩이 가능한 아이템이라면 주울 수 있는 상태
                    if (allitems[count].Item.ItemID == mCurrentItem.Item.ItemID && allitems[count].Item.CanOverlap) { break; }
                }

                //모든 칸이 null이 아니고, 중첩이 불가능하면 주울 수 없음
                if (count == allitems.Length) { return; }

                //아이템 줍는 효과음 재생
                //SoundManager.Instance.PlaySound2D("GrabItem " + SoundManager.Range(1, 3));
            }

            TryPickUp();
            ItemInfoDisappear();
        }
    }

    /// <summary>
    /// 레이캐스트를 이용하여 상점을 확인한다.
    /// </summary>
    private void CheckChest()
    {
        if (Physics.Raycast(mRayCamera.transform.position, mRayCamera.transform.forward, out mHit, mRayDistance))
        {
            //레이캐스트 결과의 태그가 박스라면?
            if (mHit.transform.tag == "Box" || mHit.transform.tag == "NPC")
            {
                //현재 레이캐스트된 박스
                ChestController rayCastedBox = mHit.transform.GetComponent<ChestController>();
                //레이캐스트 결과가 현재 아이템과 같으면 무시 (중복호출 방지)
                if (mChestController == rayCastedBox) { return; }

                mChestController = mHit.transform.GetComponent<ChestController>();
                

                Debug.LogFormat("상자 열기 가능");

                mIsBoxInteract = true;
                return;
            }
            else
            {
                BoxInfoDisappear();
            }
        }
        else
        {
            BoxInfoDisappear();
        }
    }

    /// <summary>
    /// 레이캐스트를 이용하여 아이템을 확인한다.
    /// </summary>
    private void CheckItem()
    {
        if (Physics.Raycast(mRayCamera.transform.position, mRayCamera.transform.forward, out mHit, mRayDistance))
        {
            //레이캐스트 결과의 태그가 아이템이라면?
            if (mHit.transform.tag == "Item" || mHit.transform.tag == "NPC")
            {
                //현재 레이캐스트된 아이템
                ItemPickUp rayCastedItem = mHit.transform.GetComponent<ItemPickUp>();

                //레이캐스트 결과가 현재 아이템과 같으면 무시 (중복호출 방지)
                if (mCurrentItem == rayCastedItem) { return; }

                //아이템 얻어오기 및 정보 호출
                mCurrentItem = mHit.transform.GetComponent<ItemPickUp>();
                // mItemRaycastInfoText.EnableText(mHit.transform.position + Vector3.up * rayCastedItem.IndicatorHeight, mCurrentItem.Item); (이 글에서는 설명 X)

                Debug.LogFormat("아이템: {0} 획득 가능", mCurrentItem.Item.name);

                mIsPickupActive = true;

                return;
            }
            //레이캐스트 닿았을 때, 아이템이 아닌경우에는 비활성화
            else
            {
                ItemInfoDisappear();
            }
        }
        //레이캐스트 결과가 없으면 비활성화
        else
        {
            ItemInfoDisappear();
        }
    }



    /// <summary>
    /// 아이템 정보 보여주기를 비활성화 한다.
    /// </summary>
    private void ItemInfoDisappear()
    {
        //픽업 비활성화
        mIsPickupActive = false;

        //텍스트 비활성화
        // mItemRaycastInfoText.DisableText(); (이 글에서는 설명 X)

        //현재 아이템은 null
        mCurrentItem = null;
    }

    /// <summary>
    /// 상자 정보 보여주기를 비활성화 한다.
    /// </summary>
    private void BoxInfoDisappear()
    {
        //박스 상호작용 비활성화
        mIsBoxInteract = false;

        mChestController = null;

    }

    /// <summary>
    /// 상점 정보 보여주기를 비활성화 한다.
    /// </summary>
    private void ShopInfoDisappear()
    {
        //상점 상호작용 비활성화
        mIsShopInteract = false;

        mItemShop = null;

    }


    /// <summary>
    /// 아이템을 습득한다.
    /// </summary>
    private void TryPickUp()
    {
        if (mIsPickupActive)
        {
            // mItemActionCustomFunc.InteractionItem(mCurrentItem.Item, mCurrentItem.gameObject); (이 글에서는 설명 X)

            if (mCurrentItem.Item.Type != ItemType.NONE)
            {
                mInventory.AcquireItem(mCurrentItem.Item);
                Destroy(mCurrentItem.gameObject);
            }

            ItemInfoDisappear();
        }
    }
}
