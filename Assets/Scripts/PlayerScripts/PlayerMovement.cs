using ChestSystem;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float fbMoveSpeed = 3f;
    public float rlMoveSpeed = 2f;
    public float turnSpeed = 1.5f;
    private float jumpForce = 1.5f;
    private float xRotate = 0.0f;
    private bool isCrouch;
    private bool isGround;
    private float runRadius = 7f;
    private float walkRadius = 3f;
    public float currentRadius;
    private float lastSoundPlayTime;
    private bool isCouchSound = false;


    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;
    private Camera cam;
    private AudioSource playerMovAudio;
    private AudioSource playerJumpAudio;
    private Coroutine soundCoroutine;

    private enum MovementState
    {
        Idle,
        Crouch,
        Walking,
        Running

    }
    private MovementState currentMovementState = MovementState.Idle;

   

    // Start is called before the first frame update
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        playerMovAudio = GetComponent<AudioSource>();
        playerJumpAudio = GameObject.Find("Sound:Jump").GetComponent<AudioSource>();
        playerAnimator.SetBool("Crouch", false);
        playerAnimator.SetBool("RLWithFB", true);
        playerAnimator.SetBool("SetAxe", false);

        playerRigidbody.freezeRotation = true;

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        playerRigidbody.freezeRotation = true;

        cam = Camera.main;


        isCrouch = true;
        isGround = false;

    }

    private void Update()
    {

        if (playerInput.crouch == true)
        {
            currentRadius = 1f;
        }
        else if (playerInput.jump && !isGround)
        {
            currentRadius = 10f;
        }
        else
        {
            currentRadius = Input.GetKey(KeyCode.LeftShift) ? runRadius : walkRadius;
        }

        CheckGround();
        if (ChestDialogManager.IsDialogActive || InventoryMain.IsInventoryActive || InventoryMain.IsEquipmentActive || PauseManager.Instance.isPaused) { return; }
        Jump();
        FBMove();
        RLMove();


        if (playerInput.fbMove == 0 && playerInput.rlMove != 0)
        {
            playerAnimator.SetBool("RLWithFB", false);
            playerAnimator.SetFloat("RLMove", playerInput.rlMove);
        }
        else
        {
            playerAnimator.SetBool("RLWithFB", true);
        }


        if (playerInput.crouch == true)
        {
            playerAnimator.SetBool("Crouch", true);
        }
        else
        {
            playerAnimator.SetBool("Crouch", false);
        }

        playerAnimator.SetFloat("FBMove", playerInput.fbMove);

    }

    private void LateUpdate()
    {
        if (ChestDialogManager.IsDialogActive || InventoryMain.IsInventoryActive || InventoryMain.IsEquipmentActive || PauseManager.Instance.isPaused) { return; }
        MouseRotation();
    }

    private void FBMove()
    {
        bool isMoving = playerInput.fbMove != 0 || playerInput.rlMove != 0;
        bool run = Input.GetKey(KeyCode.LeftShift);


        if (playerInput.crouch && isGround && !playerInput.jump)
        {
            Vector3 crouchFbMoveDistance = playerInput.fbMove * transform.forward * fbMoveSpeed * Time.deltaTime * 0.4f;
            playerRigidbody.MovePosition(playerRigidbody.position + crouchFbMoveDistance);

            

            if (currentMovementState != MovementState.Crouch)
            {
                StopSoundCoroutine();
                
                currentMovementState = MovementState.Crouch;
               
            }   

            if(isMoving && !isCouchSound)
            {
                soundCoroutine = StartCoroutine(CrouchSoundLoop());
                isCouchSound = true;
            }

            if (!isMoving)
            {
                StopSoundCoroutine();
                isCouchSound = false;
            }

        }
        else if (isMoving && run)
        {
            Vector3 fbMoveDistance = playerInput.fbMove * transform.forward * fbMoveSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(playerRigidbody.position + fbMoveDistance);
            isCouchSound = false;

            // 현재 상태가 달리기가 아니면 소리 변경
            if ((currentMovementState != MovementState.Running && isGround))
            {
                StopSoundCoroutine();
                soundCoroutine = StartCoroutine(RunSoundLoop());
                currentMovementState = MovementState.Running;

            }
        }
        else if (isMoving && !run)
        {
            Vector3 fbMoveDistance = playerInput.fbMove * transform.forward * fbMoveSpeed * Time.deltaTime * 0.7f;
            playerRigidbody.MovePosition(playerRigidbody.position + fbMoveDistance);
            isCouchSound = false;

            // 현재 상태가 걷기가 아니면 소리 변경
            if ((currentMovementState != MovementState.Walking && isGround) )
            {
                StopSoundCoroutine();
                soundCoroutine = StartCoroutine(WalkSoundLoop());
                currentMovementState = MovementState.Walking;

 
            }
        }
        else
        {
            StopSoundCoroutine();
            currentMovementState = MovementState.Idle;
            isCouchSound = false;
        }


    }

    private IEnumerator WalkSoundLoop()
    {
       
        while (true)
        {
            if(isGround == true && InventoryMain.IsInventoryActive == false) { SoundManager.Instance.GetSound(playerMovAudio, 3,0.4f); }
            

            yield return new WaitForSeconds(0.5f); // 지정된 간격으로 대기
        }
    }

    private IEnumerator RunSoundLoop()
    {
     
        while (true)
        {
            if (isGround == true && InventoryMain.IsInventoryActive == false) { SoundManager.Instance.GetSound(playerMovAudio, 3, 0.5f, 1, 1.2f); }

            yield return new WaitForSeconds(0.3f); // 지정된 간격으로 대기
        }
    }

    private IEnumerator CrouchSoundLoop()
    {
     
        while (true)
        {
            if (isGround == true && InventoryMain.IsInventoryActive == false) { SoundManager.Instance.GetSound(playerMovAudio, 3, 0.4f, 0.2f); }

            yield return new WaitForSeconds(0.5f); // 지정된 간격으로 대기
        }
    }

    // 현재 실행 중인 소리 재생 코루틴을 중지
    public void StopSoundCoroutine()
    {

        if (soundCoroutine != null)
        {
            StopCoroutine(soundCoroutine);
            soundCoroutine = null;
          
        }
    }



    private void RLMove()
    {
        if (playerInput.crouch == true && isGround && !playerInput.jump) 
        {
            Vector3 CrouchRlMoveDistance = playerInput.rlMove * transform.right * rlMoveSpeed * Time.deltaTime * 0.4f;
            playerRigidbody.MovePosition(playerRigidbody.position + CrouchRlMoveDistance);
        }
        else if (playerInput.crouch == false && Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 rlMoveDistance = playerInput.rlMove * transform.right * rlMoveSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(playerRigidbody.position + rlMoveDistance);
        }
        else if (playerInput.crouch == false)
        {
            Vector3 rlMoveDistance = playerInput.rlMove * transform.right * rlMoveSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(playerRigidbody.position + rlMoveDistance);
        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, currentRadius);
    }

    private void Jump()
    {
        
        if (playerInput.jump && isGround && !playerInput.crouch)
        {
           
            SoundManager.Instance.GetSound(playerJumpAudio, 16, 0.4f);

            Vector3 jumpVelocity = Vector3.up * Mathf.Sqrt(jumpForce * -Physics.gravity.y);
            playerRigidbody.AddForce(jumpVelocity, ForceMode.Impulse);

        }


           
        
    }

    private void CheckGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.3f))
        {
            if (hit.transform.tag != null)
            {
                isGround = true;
                return;
            }
        }
        isGround = false;
    }


    private void MouseRotation()
    {

        float yRotateSize = playerInput.mouseX * turnSpeed;

        float yRotate = transform.eulerAngles.y + yRotateSize;


        float xRotateSize = -playerInput.mouseY * turnSpeed;

        xRotate = Mathf.Clamp(xRotate + xRotateSize, -45, 60);



        cam.transform.eulerAngles = new Vector3(xRotate, yRotate, 0);
        transform.eulerAngles = new Vector3(0, cam.transform.eulerAngles.y, 0);

        if (isCrouch == true && playerInput.crouch == true)
        {
            cam.transform.position += new Vector3(0f, -0.8f, 0f);
            isCrouch = false;
        }
        else if (isCrouch == false && playerInput.crouch == false)
        {
            cam.transform.position += new Vector3(0f, +0.8f, 0f);
            isCrouch = true;
        }

    }
}
