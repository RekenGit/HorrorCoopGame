using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.connectUsername.text);

            SendTCPData(_packet);
        }
    }
    public static void PlayerMovement(bool[] _inputs)
    {
        using(Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach(bool _input in _inputs)
            {
                _packet.Write(_input);
            }
            _packet.Write(GameManager.players[Client.instance.myId].transform.rotation);

            SendUDPData(_packet);
        }
    }
    public static void PlayerFlash()
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerFlashSend))
        {
            _packet.Write(!GameManager.players[Client.instance.myId].flashLight.enabled);

            SendUDPData(_packet);
        }
    }
    public static void PlayerFlashRotation()
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerFlashRotationSend))
        {
            _packet.Write(GameManager.players[Client.instance.myId].PlayerCamera.rotation.x);

            SendUDPData(_packet);
        }
    }
    public static void DoorsState(string doorName)
    {
        using (Packet _packet = new Packet((int)ClientPackets.doorState))
        {
            _packet.Write(doorName);

            SendUDPData(_packet);
        }
    }
    public static void LobbyReadyButton(string _top, string _pants, string _shoes)
    {
        using (Packet _packet = new Packet((int)ClientPackets.lobbyReadyButton))
        {
            _packet.Write(_top);
            _packet.Write(_pants);
            _packet.Write(_shoes);

            SendUDPData(_packet);
        }
    }
    public static void StartGame()
    {
        using (Packet _packet = new Packet((int)ClientPackets.startGame))
        {
            SendUDPData(_packet);
        }
    }
    public static void PlayerMadeNoise()
    {
        using (Packet _packet = new Packet((int)ClientPackets.PlayerMadeNoise))
        {
            SendUDPData(_packet);
        }
    }
    #endregion
}
