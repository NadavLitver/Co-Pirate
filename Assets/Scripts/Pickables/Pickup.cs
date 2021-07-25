using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Pickup : BaseInteractable
{
    public override bool IsInteracting => false;
    [SerializeField]
    protected UnityEvent OnPickupLocal;
    [SerializeField]
    protected UnityEvent OnPickUpRemote;
    public override void OnInteract_Start(PlayerController ctrl) => PickedUp(ctrl);
    protected virtual void PickedUp(PlayerController playerRef)
    {
        photonView.RPC("PickedUpRPC", Photon.Pun.RpcTarget.All);
    }
    [PunRPC]
    protected virtual void PickedUpRPC()
    {
        OnPickUpRemote?.Invoke();
        Destroy(gameObject);

    }

}
