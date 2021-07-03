using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;

public class TeamSetter : MonoBehaviour
{
    public Team team;
    private int playerLayer = 9;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("EnterTeamSetter");
        if (other.gameObject.layer == playerLayer)
        {
            PlayerLobbyController lobbyPlayer = other.gameObject.GetComponent<PlayerLobbyController>();

            if (LiveLobbyGameManager.instance.isReady && lobbyPlayer.PlayerData.player == PhotonNetwork.LocalPlayer)
                return;

            lobbyPlayer.team = team;

            if (team == Team.Blue)
                lobbyPlayer.OnTeamBlue.Invoke();
            else
                lobbyPlayer.OnTeamRed.Invoke();
        }

    }
}
