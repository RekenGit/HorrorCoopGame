// Przypisz do obiektu: LocalPlayer
using Assets.Class;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform PlayerCamera;
    [Header("MaxDistance you can open or close the door.")]
    public float MaxDistance = 5;

    void Update()
    {
        if (Input.GetKeyDown(BindList.FindBind("Interaction").BindKeyCode))
        {
            Pressed();
        }
    }

    void Pressed()
    {
        RaycastHit doorhit;

        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out doorhit, MaxDistance))
        {
            // if raycast hits, then it checks if it hit an object with the tag Door.
            if (doorhit.transform.tag == "Door")
            {
                ClientSend.DoorsState(doorhit.transform.parent.name);
            }
        }
    }
}