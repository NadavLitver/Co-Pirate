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
        ctrl.PickedUpCannonball();

    }

    public override void OnUnbecomingTarget(PlayerController ctrl)
    {
        
    }
}
