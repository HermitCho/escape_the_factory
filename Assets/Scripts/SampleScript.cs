using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleScript : MonoBehaviour
{
    [Header("�κ��丮 ����")]
    [SerializeField] private InventoryMain mInventoryMain;

    [Header("ȹ���� ������")]
    [SerializeField] private Item Axe;


    private void OnGUI()
    {
        if (GUI.Button(new Rect(20, 20, 300, 40), "���� ȹ��"))
        {
            mInventoryMain.AcquireItem(Axe);
        }

    }
}
