using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class CannonBallsInteractable : BaseInteractable
{
    public override bool IsInteracting => false;

    public override bool InteractableCondition(PlayerController ctrl)
    {
        return ctrl != null && !ctrl.HoldingCannonBall;
    }

    public override void OnInteract_Start(PlayerController ctrl)
    {
        ctrl.PickedUpCannonball();
    }
}
