using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : BaseInteractable
{
    public override void OnBecomingTarget(PlayerController ctrl)
    {
        Debug.Log("I'm now the target!");
    }

    public override void OnInteract_Start(PlayerController ctrl)
    {
        Debug.Log("INTERACTED");
    }

    public override void OnUnbecomingTarget(PlayerController ctrl)
    {
        Debug.Log("I'm no longer the target!");
    }
}
