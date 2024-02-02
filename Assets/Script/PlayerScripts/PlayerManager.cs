// Przypisz do obiektu: LocalPlayer, Player
using Assets.Class;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public Transform PlayerCamera;
    public GameObject PlayerFlashRotation;
    public Light flashLight;

    public bool isLocalPlayer;
    public Color top;
    public Color pants;
    public Color shoes;

    // Wybierz null jesli jest to Local player
    public GameObject topObject;
    public GameObject pantsObject;
    public GameObject shoesObject;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
    }
    void Update()
    {
        if (!isLocalPlayer && topObject.GetComponent<Renderer>() != null)
        {
            topObject.GetComponent<Renderer>().material.color = top;
            pantsObject.GetComponent<Renderer>().material.color = pants;
            shoesObject.GetComponent<Renderer>().material.color = shoes;
        }

        //wysyla dane na serwer że chce włączyć/wyłączyć latarkę
        if (Input.GetKeyDown(BindList.FindBind("Flashlight").BindKeyCode) && !UIManager.instance.isGUIOpen) ClientSend.PlayerFlash();
        if (flashLight.enabled == true) ClientSend.PlayerFlashRotation();
    }
}
