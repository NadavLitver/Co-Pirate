using Photon.Realtime;
using UnityEngine;

namespace Photon.Pun.Demo.PunBasics
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        private void Awake()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PlayerInformation.players = new Player[4];
                PlayerInformation.players[0] = PhotonNetwork.LocalPlayer;
            }
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PlayerInformation.players[PhotonNetwork.CurrentRoom.PlayerCount - 1] = other;
                
                Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);

                if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
                {
                    photonView.RPC("SendPlayers", RpcTarget.All, PlayerInformation.players);
                    Debug.Log("SENT PLAYERS!!!!");
                }
            }
        }
        [PunRPC]
        void SendPlayers(Player[] players)
        {
            Debug.Log("Recieved RPC");

            PlayerInformation.players = players;
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Room For 4");
                Debug.Log("Trying to load game scene.");
            }



        }
    }
}