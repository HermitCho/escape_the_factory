using ChestSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    [SerializeField] GameObject ShopBackground;

    public static ShopSystem Instance;
   [HideInInspector] public  bool mIsShopActive = false;
    private PlayerInput playerInput = null;
    public void Awake()
    {
        if (Instance != null) { Destroy(Instance); } // 유일성을 보장하기 위해

        Instance = this;
        // DontDestroyOnLoad(gameObject);

        mIsShopActive = false;
        playerInput = GetComponent<PlayerInput>();
        //SetActiveFalse();

    }

    public void Update()
    {

        // 다이얼로그가 활성 상태에서 ESC를 누르면 ?
        if (mIsShopActive && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("상점 닫기 시도");
            SetActiveFalse();
            Debug.Log("상점 닫기 성공");
        }
    }

    public void SetActiveFalse()
    {
        ShopBackground.SetActive(false);
        mIsShopActive = false;
        InventoryMain.Instance.CloseInventory();
        //커서 비활성화
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

    }

    public void SetActiveTrue()
    {
        ShopBackground.SetActive(true);
        mIsShopActive = true;
        InventoryMain.Instance.OpenInventory();
        //커서 활성화
        UnityEngine.Cursor.lockState = CursorLockMode.Confined;
        UnityEngine.Cursor.visible = true;
    }

    public void TryOpenShop()
    {
        if (mIsShopActive ) {  return; }
        //&& Input.GetKeyDown(KeyCode.Escape)
        SetActiveTrue();

    }

}
