using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Photon.Pun.Demo.PunBasics { 
public class LobbyManager : MonoBehaviourPunCallbacks
{
        public Player[] SortedPlayers = null;

        private void Awake()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                SortedPlayers = new Player[4];
                SortedPlayers[0] = PhotonNetwork.LocalPlayer;
            }
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            SortedPlayers[PhotonNetwork.CountOfPlayers - 1] = other;

            if (PhotonNetwork.CountOfPlayers == 4)
                photonView.RPC("SendPlayers", RpcTarget.All, SortedPlayers);
        }
        [PunRPC]
        void SendPlayers(Player[] players)
        {
            SortedPlayers = players;
            if (PhotonNetwork.IsMasterClient)
                PhotonNetwork.LoadLevel("Room For 4");

            

        }
    }
}