using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTest : MonoBehaviour
{
    private float damage;
    private float updateDamage;
    private float totalDamage = 0f;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) { damage += 20f; }
        
        updateDamage = Mathf.Lerp(0, damage, Time.deltaTime * 0.6f);
        totalDamage += updateDamage;
        if (totalDamage >= damage) { damage = 0f; totalDamage = 0f; }
        GetDamage(updateDamage);



    }

    public void GetDamage(float damage)
    {
        HealthManager.Instance.GetDamage(damage);
    }   

    public void GetHeal(float heal)
    {
        HealthManager.Instance.GetHeal(heal);
    }

}
