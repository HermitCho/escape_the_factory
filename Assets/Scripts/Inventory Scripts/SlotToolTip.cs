using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    [SerializeField]
    private GameObject go_Base;

    [SerializeField]
    private Text txt_ItemName;
    [SerializeField]
    private Text txt_ItemDesc;
    [SerializeField]
    private Text txt_ItemHowtoUsed;

    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        go_Base.SetActive(true);
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f,
                            -go_Base.GetComponent<RectTransform>().rect.height * 0.5f,
                            0);
        go_Base.transform.position = _pos;

        txt_ItemName.text = _item.ItemName;
        txt_ItemDesc.text = _item.ItemDescription;

        if (_item.Type == ItemType.Equipment_HELMET)
            txt_ItemHowtoUsed.text = "ÀåÂø °¡´É";
        else if (_item.Type == ItemType.Consumable)
            txt_ItemHowtoUsed.text = "¿ì Å¬¸¯ - ¸Ô±â";
        else
            txt_ItemHowtoUsed.text = "";
  

    }

    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}