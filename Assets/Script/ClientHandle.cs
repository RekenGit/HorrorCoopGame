using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _maxPlayers = _packet.ReadInt();
        int _myId = _packet.ReadInt();

        UIManager.instance.maxPlayers = _maxPlayers;

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }
    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }
    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.players[_id].transform.position = _position;
    }
    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[_id].transform.rotation = _rotation;
    }
    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }
    public static void PlayerFlash(Packet _packet)
    {
        int _id = _packet.ReadInt();
        bool _flash = _packet.ReadBool();
        float _flashRotation = _packet.ReadFloat();

        Quaternion _rotation = GameManager.players[_id].PlayerFlashRotation.transform.rotation;
        _rotation.x = _flashRotation;

        GameManager.players[_id].flashLight.enabled = _flash;
        GameManager.players[_id].PlayerFlashRotation.transform.rotation = _rotation;
    }
    public static void OpenDoors(Packet _packet)
    {
        string _doorName = _packet.ReadString();
        bool _state = _packet.ReadBool();

        GameObject door = GameObject.Find(_doorName);
        Animator doorAnim = door.GetComponent<Animator>();
        doorAnim.SetBool("Opened", _state);

        AudioSource x = GameObject.Find(_doorName).GetComponent<AudioSource>();
        x.Play();
    }
    public static void Enemy(Packet _packet)
    {
        string _name = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameObject npc = GameObject.Find(_name);
        npc.transform.position = _position;
        npc.transform.rotation = _rotation;
    }
    public static void LobbyInfo(Packet _packet)
    {
        bool _inLobby = _packet.ReadBool();
        int _inLobbyPlayers = _packet.ReadInt();
        int _inLobbyReadyPlayers = _packet.ReadInt();
        int _lobbyOwner = _packet.ReadInt();

        //if (_inLobby) Debug.Log("Tak serwer jest w Lobby!");
        UIManager.instance.LobbyInfoSet(_inLobbyPlayers, _inLobbyReadyPlayers);
        if (Client.instance.myId == _lobbyOwner) UIManager.instance.LobbyOwner(true);
        else UIManager.instance.LobbyOwner(false);
    }
    public static void PlayerCustomInfo(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _topS = _packet.ReadString();
        string _pantsS = _packet.ReadString();
        string _shoesS = _packet.ReadString();

        Color _top, _pants, _shoes;
        ColorUtility.TryParseHtmlString("#" + _topS, out _top);
        ColorUtility.TryParseHtmlString("#" + _pantsS, out _pants);
        ColorUtility.TryParseHtmlString("#" + _shoesS, out _shoes);

        GameManager.players[_id].top = _top;
        GameManager.players[_id].pants = _pants;
        GameManager.players[_id].shoes = _shoes;
    }
    public static void GameStarted(Packet _packet)
    {
        UIManager.instance.OpenMenu(UIManager.instance.gameGUI);
    }
}
