using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private RaycastHit mHit;
    private Animator playerAnimator;
    private bool isSetAxe;
    private bool isAttacking;
    private PlayerAttackTimming playerAttackTiming;
    private AudioSource playerAudio;
    private AudioSource attackAudio;
    [Header("RayCast Distance")]
    [SerializeField] private float mRayDistance;

    private PlayerInput playerInput;
    private ZombieMovement rayCastZombie;


    [Header("Player Camrera")]
    [SerializeField] private Camera mRayCamera; 


    public Transform handTransform;

    [SerializeField] private GameObject axePrefab; 
    private GameObject axeInstance; 

    private void Awake()
    {
        playerAttackTiming = GetComponentInChildren<PlayerAttackTimming>();
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        isSetAxe = false;
        isAttacking = false;
        playerAudio = GetComponent<AudioSource>();
        attackAudio = GameObject.FindWithTag("Sound").GetComponent<AudioSource>();
    }
    private void Update()
    {
        IsEquipmentWeapon();
        IsAttack();
    }

    public void AttackFunction()  // Animation Event에서 호출되는 함수
    {
        if (playerAttackTiming != null)
        {
            playerAttackTiming.PAttack();  // 다른 오브젝트의 함수 호출
            SoundManager.Instance.GetSound(attackAudio, 5,0.3f);
        }
    }
    public void IsEquipmentWeapon()
    {
        if (InventoryMain.Instance.mEquipmentSlot.Item != null && InventoryMain.Instance.mEquipmentSlot.Item.ID == EntityCode.ITEM_AXE)
        {
            playerAnimator.SetBool("SetAxe", true);
            isSetAxe = true;

            if (axeInstance == null)
            {
                axeInstance = Instantiate(axePrefab, handTransform.position, handTransform.rotation);
                axeInstance.transform.SetParent(handTransform); 
                axeInstance.transform.localPosition = Vector3.zero; 
                axeInstance.transform.localRotation = Quaternion.identity; 

            }
        }
        else if (InventoryMain.Instance.mEquipmentSlot.Item == null || InventoryMain.Instance.mEquipmentSlot.Item.ID != EntityCode.ITEM_AXE)
        {
            playerAnimator.SetBool("SetAxe", false);
            isSetAxe = false;
            Destroy(axeInstance); 
            axeInstance = null;           
        }
        else 
        {
            playerAnimator.SetBool("SetAxe", false);
            isSetAxe = false;
            Destroy(axeInstance); 
            axeInstance = null;
        }
    }





    public void IsAttack()
    {
        if (ShopSystem.Instance.mIsShopActive == false && InventoryMain.IsInventoryActive == false && PauseManager.Instance.isPaused == false)
        {
            if (playerInput.attack && isSetAxe == true &&  !isAttacking)
            {
               // isAttacking = true;
                playerAnimator.SetTrigger("AxeSwing");
                

            }
        }
    }

 
}
