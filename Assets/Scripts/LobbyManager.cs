using Photon.Realtime;
using UnityEngine;

namespace Photon.Pun.Demo.PunBasics
{
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
            if (PhotonNetwork.IsMasterClient)
            {
                SortedPlayers[PhotonNetwork.CurrentRoom.PlayerCount - 1] = other;
                
                Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);

                if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
                {
                    photonView.RPC("SendPlayers", RpcTarget.All, SortedPlayers);
                    Debug.Log("SENT PLAYERS!!!!");
                }
            }
        }
        [PunRPC]
        void SendPlayers(Player[] players)
        {
            Debug.Log("Recieved RPC");

            SortedPlayers = players;
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Room For 4");
                Debug.Log("Trying to load game scene.");
            }



        }
    }
}