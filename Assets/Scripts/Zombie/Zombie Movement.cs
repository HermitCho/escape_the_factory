using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class ZombieMovement : MonoBehaviour
{
    [Header("레이캐스트 거리")]
    [SerializeField] private float mRayDistance;
    [SerializeField] private Collider attackCollider;

    private bool isAttacking;
    private RaycastHit mHit;

    [Header("Navi Target")]
    private Collider[] targetCol; //감지 대상 저장
    private float playerRadius = 20f; //감지 원
    private float soundRadius = 3f; //감지 원
    [HideInInspector] GameObject target_; //레이어 타겟
    private LayerMask targetLayer; // 레이어만 설정
    private NavMeshAgent nav; //내비메쉬 컴포넌트
    [HideInInspector] public float followTime = 0;
    private bool targetingPlayer;

    [Header("Dead Check")]
    ZombieControl zombieControl;
    private float health = 0f;
    private bool dead = false;
    float timer = 0;
    bool timer_start = false;
    bool setTrigger = false;

    private bool isGround;
    private Animator animator;

    private AudioSource zombieMoveAudio;


    [HideInInspector] PlayerMovement playerMovement;


    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        targetLayer = LayerMask.GetMask("Player");
        zombieControl = GetComponent<ZombieControl>();

        attackCollider.enabled = false; // 기본적으로 비활성화
        targetingPlayer = false;
        isAttacking = false;
        health = 100f;

        zombieMoveAudio = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        CheckGround();
        findPlayer();
        CheckHealth();
        // Math.Clamp(health += (3 * Time.deltaTime), 0, 100);
    }

    public void GetDamage()
    {
        health -= 20f;
        animator.SetTrigger("Hit");
        StopCoroutine(zombieControl.ZombieSound());
        SoundManager.Instance.GetSound(zombieMoveAudio, 13, 0.3f);

    }

    public void CheckHealth()
    {
        if (health <= 0f)
        {
            if (!setTrigger)
            {
                animator.SetTrigger("Die");
                setTrigger = true;
                SoundManager.Instance.GetSound(zombieMoveAudio, 12, 0.3f);
            }

            dead = true;
            StopCoroutine(zombieControl.ZombieSound());
            timer_start = true;
        }
        if (timer_start == true)
        {
            timer += Time.deltaTime;
        }
        if (timer >= 4.0f)
        {
            Destroy(gameObject);
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

            }
            else isGround = false;

        }

    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("tag:" + other.transform.tag);
        if (other.transform.CompareTag("Player"))
        {
            GameObject hurt = GameObject.Find("HurtSound");
            //other.transform.GetComponent<PlayerMovement>().StopSoundCoroutine();
            SoundManager.Instance.GetSound(hurt.GetComponent<AudioSource>(), 4, 0.5f);
            HealthManager.Instance.damage += 20;
        }
    }

    void findPlayer()
    {
        playerMovement = null;
        targetCol = Physics.OverlapSphere(transform.position, playerRadius, targetLayer);
        transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        target_ = null;
        foreach (Collider col in targetCol)
        {
            if (col.CompareTag("Player"))
            {
                target_ = col.gameObject;
                playerMovement = target_.GetComponent<PlayerMovement>();
                break;
            }
        }
        if (playerMovement == null && dead == false)
        {

            targetingPlayer = false;
            animator.SetFloat("Move", 0f);
            nav.isStopped = true;
            followTime = 0;
            return; // 함수 종료
        }


        float playerRadius2 = playerMovement.currentRadius;
        float distanceToTarget = Vector3.Distance(transform.position, target_.transform.position);
        //Debug.Log(followTime);


        // 두 원이 겹치는지 확인
        if (distanceToTarget <= soundRadius + playerRadius2 && dead == false)
        {
            followTime = 8.0f;
            targetingPlayer = true;
            nav.isStopped = false;
            nav.SetDestination(target_.transform.position);
            animator.SetFloat("Move", 1f);


            if (distanceToTarget <= 1.55f)
            {
                Vector3 direction = (target_.transform.position - transform.position).normalized;
                direction.y = 0;
                transform.forward = direction;

                transform.Translate(Vector3.zero);
                nav.velocity = Vector3.zero; // 속도 초기화
                animator.SetTrigger("Attack");

            }
        }
        else if (distanceToTarget > (soundRadius + playerRadius2) && targetingPlayer == true && dead == false)
        {
            if (followTime >= 0f)
            {
                nav.isStopped = false;
                nav.SetDestination(target_.transform.position);
                animator.SetFloat("Move", 1f);
                Debug.Log("Player detected by zombie!");
                followTime -= Time.deltaTime;
            }

            else if (followTime < 0f)
            {
                targetingPlayer = false;
                animator.SetFloat("Move", 0f);
                nav.isStopped = true;
                followTime = 0;
            }
        }

    }


}


