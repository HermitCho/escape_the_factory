using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{

    [SerializeField] private TMP_Text tmp;
    private bool mIsClick;
    // Start is called before the first frame update
    void Start()
    {
        tmp.text = (string)Money.Instance.GetString();
        mIsClick = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (mIsClick)
        {
            tmp.text = (string)Money.Instance.GetString();
            mIsClick = false;
        }
    }

    public void IsClick()
    {
        mIsClick = true;
    }
}
