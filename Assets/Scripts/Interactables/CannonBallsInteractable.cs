using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class CannonBallsInteractable : BaseInteractable
{
    public override void OnBecomingTarget(PlayerController ctrl)
    {

    }

    public override void OnInteract_Start(PlayerController ctrl)
    {
        var view = ctrl.photonView;
        Debug.Log(view);
        view.RPC("CollectedCannonBall", RpcTarget.All, view.Owner.GetPlayerNum());
    }

    public override void OnUnbecomingTarget(PlayerController ctrl)
    {
        
    }

    [PunRPC]
    public void CollectedCannonBall(int playerNum)
    {
        Debug.Log(playerNum);
        PlayerInformation.players[playerNum].playerinstance.GetComponent<PlayerController>().cannonBall.SetActive(true);
    }
}
