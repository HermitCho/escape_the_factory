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

    // HpBar Slider�� �����ϱ� ���� Slider ��ü
    [SerializeField] private Slider _hpBar;

    // �÷��̾��� HP
    private float _hp;

    public float Hp
    {
        get => _hp;
        // HP�� HealthManager ���� �ϵ��� private���� ó��
        // Math.Clamp �Լ��� ����ؼ� hp�� 0���� �Ʒ��� �������� �ʰ� �մϴ�.
        private set => _hp = Math.Clamp(value, 0, 100);
    }

    private void Awake()
    {
                if (Instance != null) { Destroy(Instance); } // ���ϼ��� �����ϱ� ����

        Instance = this;
        //DontDestroyOnLoad(gameObject);


        // �÷��̾��� HP ���� �ʱ� �����մϴ�.
        _hp = 100;
        // MaxValue�� �����ϴ� �Լ��Դϴ�.
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

    // �÷��̾ ������� ������ ����� ���� ���� �޾� HP�� �ݿ�
    public void GetDamage(float damage)
    {
       
        float getDamagedHp = Hp - damage;
        Hp = getDamagedHp;
        _hpBar.value = Hp;
    }

    //�÷��̾ ��Ŷ�� ����ϸ� �� ���� ���� �޾� HP�� �ݿ�
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