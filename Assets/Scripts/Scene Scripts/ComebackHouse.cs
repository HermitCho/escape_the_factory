using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComebackHouse : MonoBehaviour
{
    private float minSec;
    private float initSec;
    private float textSec;
    
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject mTMProParent;
    // Start is called before the first frame update
    void Start()
    { 
        minSec = 5f;
        initSec = 0f;
        textSec = 0;
        mTMProParent.SetActive(false);
        
        SaveLoadManager.Instance.LoadInventory();
        SaveLoadManager.Instance.LoadMoney();
        SaveLoadManager.Instance.LoadEquipment();
    }

    // Update is called once per frame
    void Update()
    {
        if (HealthManager.Instance != null && HealthManager.Instance.IsDie())
        {
            InventoryMain.Instance.GetAllClear();
            SaveLoadManager.Instance.SaveInventory();
            SaveLoadManager.Instance.SaveEquipment();

            SceneManager.LoadScene("House");


        }

    }

    private void OnTriggerStay(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("탈출시도");
            mTMProParent.SetActive(true);
            initSec += Time.deltaTime;
            textSec = minSec - initSec;
            DrawText(1+(int)textSec);
            Debug.Log("시간: " + (int)textSec);

        }

        if(initSec >= minSec)
        {
            mTMProParent.SetActive(false);
            
            SaveLoadManager.Instance.SaveInventory();
            SaveLoadManager.Instance.SaveMoney();
            SaveLoadManager.Instance.SaveEquipment();
            SceneManager.LoadScene("House");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        mTMProParent.SetActive(false);
        initSec = 0;
        textSec = 0;
    }



    public void DrawText(int textSec)
    {
        text.text = textSec + "초 후 이동합니다.";
    }
}
