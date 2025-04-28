using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        SaveLoadManager.Instance.LoadInventory();
        SaveLoadManager.Instance.LoadMoney();
        SaveLoadManager.Instance.LoadEquipment();
       // SaveLoadManager.Instance.LoadChest();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            
            SaveLoadManager.Instance.SaveInventory();
            SaveLoadManager.Instance.SaveMoney();
            SaveLoadManager.Instance.SaveEquipment();
         //  SaveLoadManager.Instance.SaveChest();
           SceneManager.LoadScene("SampleScene");



        }
       
    }
}
