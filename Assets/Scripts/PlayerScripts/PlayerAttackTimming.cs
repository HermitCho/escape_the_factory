using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAttackTimming : MonoBehaviour
{
    BoxCollider boxCollider;


    // Start is called before the first frame update
    void Start()
    {
        if (InventoryMain.Instance.mEquipmentSlot.Item != null && InventoryMain.Instance.mEquipmentSlot.Item.ID == EntityCode.ITEM_AXE)
        {
            boxCollider = GetComponentInChildren<BoxCollider>();           
        }
    }

    private void Update()
    {
        if(InventoryMain.Instance.mEquipmentSlot.Item != null && InventoryMain.Instance.mEquipmentSlot.Item.ID == EntityCode.ITEM_AXE )
        {
            boxCollider = GetComponentInChildren<BoxCollider>();
        }
    }
    public void PAttack()
    {
        StartCoroutine(DisableTemporarily(boxCollider));
    }
    private IEnumerator DisableTemporarily(BoxCollider box)
    {
        box.enabled = true;
        yield return new WaitForSeconds(0.3f);
        box.enabled = false;
    }


/*    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player Tag: "+ other.transform.tag);
        if (other.transform.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<ZombieMovement>().GetDamage();
        }
    }*/
}
