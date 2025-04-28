using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    public static GameMenuManager Instance = null;
    static bool isActive = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null) { Destroy(Instance); } // ���ϼ��� �����ϱ� ����

        Instance = this;
        DontDestroyOnLoad(gameObject);

        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsOptionActive()
    {
        if (isActive)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
