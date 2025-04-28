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
        if (Instance != null) { Destroy(Instance); } // ���ϼ��� �����ϱ� ����

        Instance = this;
        // DontDestroyOnLoad(gameObject);

        mIsShopActive = false;
        playerInput = GetComponent<PlayerInput>();
        //SetActiveFalse();

    }

    public void Update()
    {

        // ���̾�αװ� Ȱ�� ���¿��� ESC�� ������ ?
        if (mIsShopActive && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("���� �ݱ� �õ�");
            SetActiveFalse();
            Debug.Log("���� �ݱ� ����");
        }
    }

    public void SetActiveFalse()
    {
        ShopBackground.SetActive(false);
        mIsShopActive = false;
        InventoryMain.Instance.CloseInventory();
        //Ŀ�� ��Ȱ��ȭ
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

    }

    public void SetActiveTrue()
    {
        ShopBackground.SetActive(true);
        mIsShopActive = true;
        InventoryMain.Instance.OpenInventory();
        //Ŀ�� Ȱ��ȭ
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
