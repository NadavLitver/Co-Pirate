using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantFixPickup : Pickup
{
    protected override IEnumerator PickedUp(PlayerController playerRef)
    {
        PickupHandler.isInstantPickedUp = true;
        Destroy(gameObject);
        yield return null;

    }

  
  
}
