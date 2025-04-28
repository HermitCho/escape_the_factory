using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleScript : MonoBehaviour
{
    [Header("¿Œ∫•≈‰∏Æ ∏ﬁ¿Œ")]
    [SerializeField] private InventoryMain mInventoryMain;

    [Header("»πµÊ«“ æ∆¿Ã≈€")]
    [SerializeField] private Item Axe;


    private void OnGUI()
    {
        if (GUI.Button(new Rect(20, 20, 300, 40), "µµ≥¢ »πµÊ"))
        {
            mInventoryMain.AcquireItem(Axe);
        }

    }
}
