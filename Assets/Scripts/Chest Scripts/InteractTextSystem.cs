using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTextSystem : MonoBehaviour
{
    [Header("ItemRaycast ��ũ��Ʈ")]
    [SerializeField] private ItemRaycast mItemRaycast;
    [Header("Interact text�� �θ� , On/Off�뵵")]
    [SerializeField] private GameObject mInteractSystem;
    [Header("Interact text�� �θ� , On/Off�뵵")]
    [SerializeField] private GameObject mPickUpSystem;
    [Header("PlayerInput ��ũ��Ʈ")]
    [SerializeField] private PlayerInput mPlayerInput;
    // Start is called before the first frame update
    void Awake()
    {
        mInteractSystem.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ChestTextActive();
        PickUpTextActive();

    }

    private void ChestTextActive()
    {
        if (mItemRaycast.mIsBoxInteract ) { mInteractSystem.SetActive(true); }

        else if (mPlayerInput.interact) { mInteractSystem.SetActive(false); }

        else { mInteractSystem.SetActive(false); }

    }

    private void PickUpTextActive()
    {
        if (mItemRaycast.mIsPickupActive && !mItemRaycast.mIsEquipment) { mPickUpSystem.SetActive(true); }

        else if (mPlayerInput.interact) { mPickUpSystem.SetActive(false); }

        else { mPickUpSystem.SetActive(false); }
    }
}
