using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    public string fbMoveAxisName = "Vertical";
    public string rlMoveAxisName = "Horizontal";
    public string fireButtonName = "Fire1";
    public string useButtonName = "Use";
    public string interactButtonName = "Interact";
    public string crouchButtonName = "Crouch";
    public string mouseXAxisName = "Mouse X";
    public string mouseYAxisName = "Mouse Y";
    public string jumpButtonName = "Jump";
    public string inventoryButtonName = "Inventory";
    public int attackClickName = 0;

    public float fbMove { get; private set; }
    public float rlMove { get; private set; }
    public bool fire { get; private set; }
    public bool use { get; private set; }
    public bool interact { get; private set; }

    public bool crouch { get; private set; }

    public float mouseX { get; private set; }

    public float mouseY { get; private set; }

    public bool jump { get; private set; }
    public bool inventory { get; private set; }

    public bool attack { get; private set; }

    private void Start()
    {
        fbMove = 0;
        rlMove = 0;
        fire = false;
        use = false;
        interact = false;
        crouch = false;
        mouseX = 0;
        mouseY = 0;
        jump = false;
        inventory = false;
        attack = false;
    }
    // Update is called once per frame
    private void Update()
    {
        //if(GameManager.instance != null && GameManager.instance.isGameover)
        //{
        //    move = 0;
        //    rotate = 0;
        //    fire = false;
        //    use = false;
        //    interact = false;
        //    crouch = false;
        //    mouseX = 0;
        //    mouseY = 0;
        //    jump = false;
        //}

        fbMove = Input.GetAxis(fbMoveAxisName);
        rlMove = Input.GetAxis(rlMoveAxisName);
        fire = Input.GetButtonDown(fireButtonName);
        use = Input.GetButtonDown(useButtonName);
        interact = Input.GetButtonDown(interactButtonName);
        crouch = Input.GetButton(crouchButtonName);
        mouseX = Input.GetAxis(mouseXAxisName);
        mouseY = Input.GetAxis(mouseYAxisName);
        jump = Input.GetButtonDown(jumpButtonName);
        inventory = Input.GetButtonDown(inventoryButtonName);
        attack = Input.GetMouseButtonDown(attackClickName);
    }

}
