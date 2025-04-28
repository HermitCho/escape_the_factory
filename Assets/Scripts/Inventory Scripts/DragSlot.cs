using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 아이템을 드래그 할 경우 임시로 DragSlot에 아이템을 저장한다.
/// </summary>
public class DragSlot : MonoBehaviour
{
    [HideInInspector] public InventorySlot CurrentSlot;
    [HideInInspector] public bool IsShiftMode;
    [SerializeField] private Image mItemImage;
    public static DragSlot Instance = null;

    private void Awake()
    {
        if (Instance != null) { Destroy(Instance); } // 유일성을 보장하기 위해

        Instance = this;
       // DontDestroyOnLoad(gameObject);
    }

    public void DragSetImage(Image _itemImage)
    {
        mItemImage.sprite = _itemImage.sprite;
        SetColor(1);
        
    }

    public void SetColor(float alpha)
    {
        Color color = mItemImage.color;
        color.a = alpha;
        mItemImage.color = color;
    }
} 