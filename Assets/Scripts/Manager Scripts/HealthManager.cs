using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthManager : MonoBehaviour
{
   [HideInInspector] public float damage;
   [HideInInspector] public float heal;
    private float updateDamage;
    private float totalDamage = 0f;
    private float updateHeal;
    private float totalHeal = 0f;



    public static HealthManager Instance;

    // HpBar Slider를 연동하기 위한 Slider 객체
    [SerializeField] private Slider _hpBar;

    // 플레이어의 HP
    private float _hp;

    public float Hp
    {
        get => _hp;
        // HP는 HealthManager 수정 하도록 private으로 처리
        // Math.Clamp 함수를 사용해서 hp가 0보다 아래로 떨어지지 않게 합니다.
        private set => _hp = Math.Clamp(value, 0, 100);
    }

    private void Awake()
    {
                if (Instance != null) { Destroy(Instance); } // 유일성을 보장하기 위해

        Instance = this;
        //DontDestroyOnLoad(gameObject);


        // 플레이어의 HP 값을 초기 세팅합니다.
        _hp = 100;
        // MaxValue를 세팅하는 함수입니다.
        SetMaxHealth(_hp);
    }

    private void Update()
    {
        

        updateHeal = Mathf.Lerp(0, heal, Time.deltaTime * 0.6f);
        totalHeal += updateHeal;
        if (totalHeal >= heal) { heal = 0f; totalHeal = 0f; }
        GetHeal(updateHeal);


        updateDamage = Mathf.Lerp(0, damage, Time.deltaTime * 0.6f);
        totalDamage += updateDamage;
        if (totalDamage >= damage) { damage = 0f; totalDamage = 0f; }
        GetDamage(updateDamage);
    }

    public float SetMaxHealth(float health)
    {
        _hpBar.maxValue = health;
        _hpBar.value = health;
        return health;
    }

    // 플레이어가 대미지를 받으면 대미지 값을 전달 받아 HP에 반영
    public void GetDamage(float damage)
    {
       
        float getDamagedHp = Hp - damage;
        Hp = getDamagedHp;
        _hpBar.value = Hp;
    }

    //플레이어가 힐킷을 사용하면 힐 값을 전달 받아 HP에 반영
    public void GetHeal(float heal)
    {
        float getHealedHp = Hp + heal;
     
        Hp = Mathf.Clamp(getHealedHp, 0, 100);
        _hpBar.value = Hp;
    }

    public bool IsDie()
    {
        if (_hp <= 0f) { return true; }
        else { return false; }
    }

    
}