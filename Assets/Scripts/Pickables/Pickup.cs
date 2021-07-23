using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Pickup : BaseInteractable
{
    public override bool IsInteracting => false;
    [SerializeField]
    protected UnityEvent OnPickUp;
    public override void OnInteract_Start(PlayerController ctrl) => PickedUp(ctrl);
    protected virtual void PickedUp(PlayerController playerRef)
    {
        OnPickUp?.Invoke();
        Destroy(gameObject);
    }

}
