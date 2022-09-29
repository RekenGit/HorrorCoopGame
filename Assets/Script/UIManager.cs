using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Client;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject cam;
    public GameObject startMenu;
    public GameObject connectMenu;
    public GameObject escMenu;
    public GameObject optionMenu;
    public GameObject gameGUI;
    public GameObject lobbyGUI;

    public InputField connectUsername;
    public InputField connectIP;
    public Slider optionSlider;
    public Slider gameFlashlightSlider;

    public GameObject lobbyMenu;
    public TextMeshProUGUI lobbyMenu_Players;
    public TextMeshProUGUI lobbyMenu_ReadyPlayers;
    public GameObject ownerMenu;
    public GameObject playerMenu;
    public GameObject readyButton;
    public GameObject startLobbyButton;
    public int maxPlayers;

    bool lastMenuIsStart = true;
    bool isInGame = false;
    public bool isInMenu = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if (isInGame) cam.SetActive(false);
        else cam.SetActive(true);

        if (lobbyGUI.active == true) lobbyMenu.SetActive(true);
        else lobbyMenu.SetActive(false);
    }

    #region OpenMenus
    public void OpenOptionsMenu()
    {
        startMenu.SetActive(false);
        escMenu.SetActive(false);

        isInMenu = true;
        optionMenu.SetActive(true);
        optionSlider.interactable = true;
        Cursor.visible = true;
    }
    public void OpenConnectMenu()
    {
        startMenu.SetActive(false);

        isInMenu = true;
        connectMenu.SetActive(true);
        connectUsername.interactable = true;
        connectIP.interactable = true;
    }
    public void OpenStartMenu()
    {
        connectMenu.SetActive(false);
        escMenu.SetActive(false);
        optionMenu.SetActive(false);
        connectUsername.interactable = false;
        connectIP.interactable = false;
        optionSlider.interactable = false;

        isInMenu = true;
        lastMenuIsStart = true;
        startMenu.SetActive(true);
        isInGame = false;
        Cursor.visible = true;
    }
    public void OpenEscMenu()
    {
        optionMenu.SetActive(false);
        gameGUI.SetActive(false);
        optionSlider.interactable = false;

        isInMenu = true;
        lastMenuIsStart = false;
        escMenu.SetActive(true);
    }
    public void OpenGameGUI()
    {
        GameManager.players[Client.instance.myId].PlayerCamera.gameObject.SetActive(true);
        lobbyGUI.SetActive(false);
        connectMenu.SetActive(false);
        escMenu.SetActive(false);
        connectUsername.interactable = false;
        connectIP.interactable = false;

        isInMenu = false;
        isInGame = true;
        gameGUI.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void OpenLobbyGUI()
    {
        connectMenu.SetActive(false);
        connectUsername.interactable = false;
        connectIP.interactable = false;

        isInMenu = true;
        lobbyGUI.SetActive(true);
        LoadPlayerCustomizations();
    }
    #endregion
    public void GoBackFromOptions()
    {
        optionMenu.SetActive(false);
        if(lastMenuIsStart) startMenu.SetActive(true);
        else escMenu.SetActive(true);
    }
    public void ConnectToServer()
    {
        Client.instance.ConnectToServer(false);
        OpenLobbyGUI();
    }
    public void ConnectToServerLocal()
    {
        Client.instance.ConnectToServer(true);
        OpenLobbyGUI();
    }
    public void LeaveServer()
    {
        Client.instance.Disconnect();
        OpenStartMenu();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    #region Lobby
    public void LobbyInfoSet(int inLobbyPlayers, int inLobbyReadyPlayers)
    {
        lobbyMenu_Players.text = inLobbyPlayers + " / " + maxPlayers;
        lobbyMenu_ReadyPlayers.text = inLobbyReadyPlayers + " / " + (inLobbyPlayers - 1);

        if ((inLobbyPlayers - 1) == inLobbyReadyPlayers) startLobbyButton.GetComponent<Button>().interactable = true;
        else startLobbyButton.GetComponent<Button>().interactable = false;
    }
    public void LobbyOwner(bool isOwner)
    {
        if (isOwner)
        {
            ownerMenu.SetActive(true);
            playerMenu.SetActive(false);
        }
        else
        {
            ownerMenu.SetActive(false);
            playerMenu.SetActive(true);
        }
    }
    public void Ready()
    {
        SendCustomPlayerInfo();
        readyButton.GetComponent<Button>().interactable = false;
    }
    public void StartGame()
    {
        SendCustomPlayerInfo();
        ClientSend.StartGame();
    }
    void SendCustomPlayerInfo()
    {
        Color _top, _pants, _shoes;
        string top, pants, shoes;
        _top = ColorPicker_Lobby.instance.ColorOfTop.GetComponent<Renderer>().material.color;
        _pants = ColorPicker_Lobby.instance.ColorOfPants.GetComponent<Renderer>().material.color;
        _shoes = ColorPicker_Lobby.instance.ColorOfShoes.GetComponent<Renderer>().material.color;
        top = ColorUtility.ToHtmlStringRGBA(_top);
        pants = ColorUtility.ToHtmlStringRGBA(_pants);
        shoes = ColorUtility.ToHtmlStringRGBA(_shoes);

        ClientSend.LobbyReadyButton(top, pants, shoes);

        List<string> lines = new List<string>();
        lines.Add("top:"+top);
        lines.Add("pants:"+pants);
        lines.Add("shoes:"+shoes);
        File.WriteAllLines("PlayerCustomization.txt", lines);
    }
    #endregion

    void LoadPlayerCustomizations()
    {
        string _top, _pants, _shoes;
        List<string> lines = File.ReadAllLines("PlayerCustomization.txt").ToList();
        _top = "#" + lines[0].Substring(4, lines[0].Length-4);
        _pants = "#" + lines[1].Substring(6, lines[1].Length-6);
        _shoes = "#" + lines[2].Substring(6, lines[2].Length-6);

        Color top, pants, shoes;
        ColorUtility.TryParseHtmlString(_top, out top);
        ColorUtility.TryParseHtmlString(_pants, out pants);
        ColorUtility.TryParseHtmlString(_shoes, out shoes);

        ColorPicker_Lobby.instance.ColorOfTop.GetComponent<Renderer>().material.color = top;
        ColorPicker_Lobby.instance.ColorOfPants.GetComponent<Renderer>().material.color = pants;
        ColorPicker_Lobby.instance.ColorOfShoes.GetComponent<Renderer>().material.color = shoes;
    }
}
