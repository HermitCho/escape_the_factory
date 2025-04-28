using UnityEngine;
using TMPro;

public class EquipmentInventory : InventoryBase
{
    public static bool IsInventoryActive = false;  // 인벤토리 활성화 되었는가?
    private PlayerInput playerInput = null;

    [Header("현재 계산된 수치를 표현할 텍스트 라벨들")]
    [SerializeField] private TextMeshProUGUI mDamageLabel;
    [SerializeField] private TextMeshProUGUI mDefenseLabel;


    new private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        base.Awake();
    }

    private void Update()
    {
        //옵션이 켜져있는경우 비활성화
      //  if (GameMenuManager.IsOptionActive) { return; }

        if (playerInput.inventory)
        {
            if (mInventoryBase.activeInHierarchy)
            {
               /* mInventoryBase.SetActive(false);
                IsInventoryActive = false;

                UnityEngine.Cursor.lockState = CursorLockMode.Confined;
                UnityEngine.Cursor.visible = true;*/
            }
            else
            {
              /*  mInventoryBase.SetActive(true);
                IsInventoryActive = true;

                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                UnityEngine.Cursor.visible = false;*/
            }
        }
    }
}
