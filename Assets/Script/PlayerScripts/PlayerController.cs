// Przypisz do obiektu: LocalPlayer
using Assets.Class;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void FixedUpdate()
    {
        SendInputToServer();
    }

    private void SendInputToServer()
    {
        if (!UIManager.instance.isGUIOpen)
        {
            bool[] _inputs = new bool[]
            {
                Input.GetKey(BindList.FindBind("Forward").BindKeyCode),
                Input.GetKey(BindList.FindBind("Backward").BindKeyCode),
                Input.GetKey(BindList.FindBind("Left").BindKeyCode),
                Input.GetKey(BindList.FindBind("Right").BindKeyCode),
                Input.GetKey(BindList.FindBind("Jump").BindKeyCode),
            };

            ClientSend.PlayerMovement(_inputs);
        }
    }
}
