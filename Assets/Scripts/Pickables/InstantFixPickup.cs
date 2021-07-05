using System.Collections;

public class InstantFixPickup : Pickup
{
    protected override void PickedUp(PlayerController playerRef)
    {
        playerRef.InstantFixBuff = true;

        base.PickedUp(playerRef);
    }



}
