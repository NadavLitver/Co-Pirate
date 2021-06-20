using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSetter : MonoBehaviour
{
    public bool iSetTeam1;
  
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("EnterTeamSetter");
        PlayerLobbyController lobbyPlayer = other.gameObject.GetComponent<PlayerLobbyController>();
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
