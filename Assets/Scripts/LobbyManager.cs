using Photon.Realtime;
using System;
using UnityEngine;

namespace Photon.Pun.Demo.PunBasics
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [SerializeField, Min(1)]
        private int _numOfPlayers;
        private void Awake()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PlayerInformation.players = new PlayerData[_numOfPlayers];
                PlayerInformation.players[0] = new PlayerData(PhotonNetwork.LocalPlayer, 0);
            }
        }
        private void Start()
        {
            //if (_numOfPlayers == 1 && PhotonNetwork.IsMasterClient)
                //SendPlayers(PlayerInformation.players);
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                int playerNum = PhotonNetwork.CurrentRoom.PlayerCount - 1;

                PlayerInformation.players[playerNum] = new PlayerData(other, playerNum);
                
                Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);

                if (PhotonNetwork.CurrentRoom.PlayerCount == _numOfPlayers)
                {
                    photonView.RPC("SendPlayers", RpcTarget.All, PlayerInformation.players as object);
                    Debug.Log("SENT PLAYERS!!!!");
                }
            }
        }
        [PunRPC]
        void SendPlayers(object[] players)
        {
            Debug.Log("Recieved RPC");

            PlayerInformation.players = Array.ConvertAll(players, (x) => (PlayerData)x);
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Room For 4");
                Debug.Log("Trying to load game scene.");
            }
        }
    }
}