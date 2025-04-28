using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player Tag: " + other.transform.tag);
        if (other.transform.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<ZombieMovement>().GetDamage();
        }
    }
}
