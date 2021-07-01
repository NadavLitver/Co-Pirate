using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndgameScreen : MonoBehaviour, IOnEventCallback
{
    [SerializeField]
    private UnityEvent<int> OnPlayerReadyEvent;
    private const byte PlayerReadyPE = 1;
    private Dictionary<Player, bool> _playersReady = new Dictionary<Player, bool>();
    private int _readyCount = 0;

    public int ReadyCount 
    {
        get => _readyCount;
        set
        {
            if (_readyCount == value)
                return;

            _readyCount = value;

            OnPlayerReadyEvent?.Invoke(_readyCount);
        }
    }

    void OnEnable()
    {
        foreach (var player in PlayerInformation.players)
            _playersReady.Add(player.player, false);
    }
    public void Ready()
    {
        var eventOptions = new RaiseEventOptions() { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(PlayerReadyPE, GameManager.instance.localPlayer, eventOptions, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case PlayerReadyPE:
                if (_playersReady[(Player)photonEvent.CustomData] == false)
                {
                    _playersReady[(Player)photonEvent.CustomData] = true;
                    ReadyCount++;
                }
                break;
        }
    }
}
