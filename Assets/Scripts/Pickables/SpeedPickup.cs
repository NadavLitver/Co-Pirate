using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPickup : Pickup
{
    public float SpeedScaler = 2;

    protected override void PickedUp(PlayerController playerRef)
    {
        playerRef.SpeedBuff = true;
        playerRef.speedScalar = SpeedScaler;

        playerRef.StartCoroutine(DisableBuffRoutine(playerRef));

        base.PickedUp(playerRef);
    }
    private IEnumerator DisableBuffRoutine(PlayerController playerRef)
    {
        yield return new WaitForSeconds(15f);
        playerRef.SpeedBuff = false;
        playerRef.speedScalar = 1;
    }
}
