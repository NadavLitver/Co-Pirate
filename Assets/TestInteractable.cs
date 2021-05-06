using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteractable : BaseInteractable
{
    public override void OnBecomingTarget()
    {
        Debug.Log("I'm now the target!");
    }

    public override void OnInteract()
    {
        Debug.Log("INTERACTED");
    }

    public override void OnUnbecomingTarget()
    {
        Debug.Log("I'm no longer the target!");
    }
}
