using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Money : MonoBehaviour
{
    

    public static Money Instance;

    private void Awake()
    {
        if (Instance != null) { Destroy(Instance); } // ���ϼ��� �����ϱ� ����

        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    public static int MONEY;

    
    public int GetMoney()
    {
        return MONEY;
    }

    public void SetMoney(int value)
    {
        MONEY = value;
    }

    public string GetString()
    {
        return MONEY.ToString();
    }
   

}
