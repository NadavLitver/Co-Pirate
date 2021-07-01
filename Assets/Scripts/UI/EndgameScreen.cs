using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndgameScreen : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<int> OnPlayerReadyEvent;
    [SerializeField]
    private UnityEvent OnAllPlayersReadyEvent;

    private const byte PlayerReadyPE = 1;
    private Dictionary<Player, bool> _playersReady = new Dictionary<Player, bool>();
    private int _readyCount = 0;
    private bool _ready = false;

    public int ReadyCount
    {
        get => _readyCount;
        set
        {
            if (_readyCount == value)
                return;

            _readyCount = value;

            OnPlayerReadyEvent?.Invoke(_readyCount);

            if (_readyCount >= PhotonNetwork.CountOfPlayers)
            {
                OnAllPlayersReadyEvent?.Invoke();
                if (PhotonNetwork.IsMasterClient)
                    GameManager.instance.LoadLobby();
            }
        }
    }

    void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
        foreach (var player in PlayerInformation.players)
            _playersReady.Add(player.player, false);
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }
    private void NetworkingClient_EventReceived(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case PlayerReadyPE:
                var player = (Player)photonEvent.CustomData;
                if (_playersReady[player] == false)
                {
                    Debug.Log("player ready " + player.NickName);
                    _playersReady[player] = true;
                    ReadyCount++;
                }
                break;
        }
    }

    public void Ready()
    {
        if (_ready)
            return;

        Debug.Log("Ready ");

        PhotonNetwork.RaiseEvent(PlayerReadyPE, PhotonNetwork.LocalPlayer, RaiseEventOptions.Default, SendOptions.SendReliable);
        _ready = true;
    }
}
