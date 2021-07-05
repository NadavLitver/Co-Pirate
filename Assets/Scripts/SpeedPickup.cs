using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPickup : Pickup
{
    public float SpeedScaler = 2;

    protected override IEnumerator PickedUp(PlayerController playerReference)
    {
        Debug.Log(playerReference.name);
        playerReference.speedScalar = SpeedScaler;
        PickupHandler.isSpeedPickedUp = true;
        Destroy(gameObject);
        yield return new WaitForSeconds(3f);
        PickupHandler.isSpeedPickedUp = false;
        playerReference.speedScalar = 1;

    }
}
