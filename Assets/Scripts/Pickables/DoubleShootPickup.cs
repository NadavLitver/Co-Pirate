using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleShootPickup : Pickup
{
    protected override void PickedUp(PlayerController playerRef)
    {
        playerRef.DoubleShootBuff = true;

        base.PickedUp(playerRef);
    }
}
