// Przypisz do obiektu: ClientManager
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Client;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private GameObject lastOpennedMenu;

    public GameObject debugUI;
    public GameObject mainGUI;
    public GameObject connectGUI;
    public GameObject optionGUI;
    public GameObject gameGUI;
    public GameObject lobbyGUI;

    public InputField connectUsername;
    public InputField connectIP;
    public Slider sensivitySlider;
    public Slider flashlightSlider;
    public GameObject MainGUI_InGame;
    public GameObject MainGUI_NotInGame;

    public GameObject lobbyMenuScenne; //GameObject -LobbySelect-
    public GameObject lobbyCam;
    public TextMeshProUGUI lobbyMenu_Players;
    public TextMeshProUGUI lobbyMenu_ReadyPlayers;
    public GameObject ownerMenu;
    public GameObject playerMenu;
    public Button readyButton;
    public Button startGameButton;
    public int maxPlayers;

    public bool isGUIOpen = true;

    private void Awake()
    {
        lastOpennedMenu = mainGUI;
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this);
        debugUI.SetActive(Debug.isDebugBuild);
    }
    public void OpenMenu(GameObject menuToOpen)
    {
        lastOpennedMenu.SetActive(false);
        lastOpennedMenu = menuToOpen;
        menuToOpen.SetActive(true);
        isGUIOpen = true;

        if (menuToOpen == mainGUI)
        {
            lobbyCam.SetActive(true);
            connectUsername.interactable = connectIP.interactable = sensivitySlider.interactable = false;
            sensivitySlider.interactable = false;
        }
        else if (menuToOpen == connectGUI)
        {
            MainGUI_InGame.SetActive(false);
            MainGUI_NotInGame.SetActive(true);
            lobbyCam.SetActive(true);
            lobbyMenuScenne.SetActive(false);
            connectUsername.interactable = connectIP.interactable = true;
        }
        else if (menuToOpen.transform.parent.name == "Options")
        {
            sensivitySlider.interactable = true;
        }
        else if (menuToOpen == gameGUI)
        {
            isGUIOpen = false;
            MainGUI_InGame.SetActive(true);
            MainGUI_NotInGame.SetActive(false);
            lobbyMenuScenne.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            GameManager.players[Client.instance.myId].PlayerCamera.gameObject.SetActive(true);
            connectUsername.interactable = connectIP.interactable = false;
        }
        else if (menuToOpen == lobbyGUI)
        {
            lobbyCam.SetActive(true);
            lobbyMenuScenne.SetActive(true);
            connectUsername.interactable = connectIP.interactable = false;
            LoadPlayerCustomizations();
        }
        Cursor.visible = isGUIOpen;
    }
    public void ConnectToServer(bool isLocal = false)
    {
        Client.instance.ConnectToServer(isLocal);
        OpenMenu(lobbyGUI);
    }
    public void LeaveServer()
    {
        Client.instance.Disconnect();
        OpenMenu(lobbyGUI);
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
        startGameButton.interactable = (inLobbyPlayers - 1) == inLobbyReadyPlayers;
    }
    public void LobbyOwner(bool isOwner)
    {
        ownerMenu.SetActive(isOwner);
        playerMenu.SetActive(!isOwner);
    }
    public void Ready()
    {
        SendCustomPlayerInfo();
        readyButton.interactable = false;
    }
    public void StartGame()
    {
        SendCustomPlayerInfo();
        ClientSend.StartGame();
    }
    void SendCustomPlayerInfo()
    {
        string top = ColorUtility.ToHtmlStringRGBA(ColorPicker_Lobby.instance.ColorOfTop.GetComponent<Renderer>().material.color);
        string pants = ColorUtility.ToHtmlStringRGBA(ColorPicker_Lobby.instance.ColorOfPants.GetComponent<Renderer>().material.color);
        string shoes = ColorUtility.ToHtmlStringRGBA(ColorPicker_Lobby.instance.ColorOfShoes.GetComponent<Renderer>().material.color);

        PlayerPrefs.SetString("SkinTop", "#" + top);
        PlayerPrefs.SetString("SkinPants", "#" + pants);
        PlayerPrefs.SetString("SkinShoes", "#" + shoes);

        /*List<string> lines = new()
        {
            "top:" + top,
            "pants:" + pants,
            "shoes:" + shoes
        };*/
        ClientSend.LobbyReadyButton(top, pants, shoes);
        //File.WriteAllLines("PlayerCustomization.txt", lines);
    }
    void LoadPlayerCustomizations()
    {
        //List<string> lines = File.ReadAllLines("PlayerCustomization.txt").ToList();
        Color top, pants, shoes;

        ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("SkinTop"), out top);
        ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("SkinPants"), out pants);
        ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("SkinShoes"), out shoes);

        //ColorUtility.TryParseHtmlString("#" + lines[0].Substring(4, lines[0].Length - 4), out top);
        //ColorUtility.TryParseHtmlString("#" + lines[1].Substring(6, lines[1].Length - 6), out pants);
        //ColorUtility.TryParseHtmlString("#" + lines[2].Substring(6, lines[2].Length - 6), out shoes);
        ColorPicker_Lobby.instance.ColorOfTop.GetComponent<Renderer>().material.color = top;
        ColorPicker_Lobby.instance.ColorOfPants.GetComponent<Renderer>().material.color = pants;
        ColorPicker_Lobby.instance.ColorOfShoes.GetComponent<Renderer>().material.color = shoes;
    }
    #endregion
}
