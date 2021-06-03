using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonInteraction : BaseInteractable
{
    #region State
    private PlayerController ctrl = default;
    #endregion
    public override bool InteractableCondition()
    {

        return ctrl == null || ctrl.HoldingCannonBall;
    }

    public override void OnBecomingTarget(PlayerController ctrl)
    {
        Debug.Log("I'm the target! " + gameObject);
        this.ctrl = ctrl;
    }

    public override void OnInteract_Start(PlayerController ctrl)
    {
        throw new System.NotImplementedException();
    }

    public override void OnUnbecomingTarget(PlayerController ctrl) {
        Debug.Log("I'm no longer the target! " + gameObject);
        this.ctrl = null;
    }
}
