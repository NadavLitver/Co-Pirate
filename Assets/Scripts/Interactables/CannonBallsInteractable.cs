using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CannonBallsInteractable : BaseInteractable
{
    public override void OnBecomingTarget(PlayerController ctrl)
    {

    }

    public override void OnInteract_Start(PlayerController ctrl)
    {
        var view = PlayerController.localPlayerCtrl.photonView;
        view.RPC("CollectedCannonBall", RpcTarget.All, view);
    }

    public override void OnUnbecomingTarget(PlayerController ctrl)
    {
        
    }

    [PunRPC]
    public void CollectedCannonBall(PhotonView view)
    {
        view.GetComponent<PlayerController>().cannonBall.SetActive(true);
    }
}
