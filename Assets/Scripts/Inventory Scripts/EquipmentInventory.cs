using UnityEngine;
using TMPro;

public class EquipmentInventory : InventoryBase
{
    public static bool IsInventoryActive = false;  // �κ��丮 Ȱ��ȭ �Ǿ��°�?
    private PlayerInput playerInput = null;

    [Header("���� ���� ��ġ�� ǥ���� �ؽ�Ʈ �󺧵�")]
    [SerializeField] private TextMeshProUGUI mDamageLabel;
    [SerializeField] private TextMeshProUGUI mDefenseLabel;


    new private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        base.Awake();
    }

    private void Update()
    {
        //�ɼ��� �����ִ°�� ��Ȱ��ȭ
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
