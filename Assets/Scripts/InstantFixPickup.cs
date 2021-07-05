using System.Collections;

public class InstantFixPickup : Pickup
{
    protected override void PickedUp(PlayerController playerRef)
    {
        playerRef.InstantFixBuff = true;

        Destroy(gameObject);

        base.PickedUp(playerRef);
    }



}
