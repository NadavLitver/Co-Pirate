using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSetter : MonoBehaviour
{
    public bool iSetTeam1;
    private int playerLayer = 9;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("EnterTeamSetter");
        if(other.gameObject.layer == playerLayer)
        {
            PlayerLobbyController lobbyPlayer = other.gameObject.GetComponent<PlayerLobbyController>();
            
            if (LiveLobbyGameManager.instance.isReady && lobbyPlayer.PlayerData.player == PhotonNetwork.LocalPlayer)
                return;

            if (iSetTeam1)
            {
                lobbyPlayer.isTeam1 = true;
                lobbyPlayer.OnTeam1.Invoke();

            }
            else
            {
                lobbyPlayer.isTeam1 = false;
                lobbyPlayer.OnTeam2.Invoke();

            }
        }
       
    }
}
