using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Pickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerRef = other.GetComponent<PlayerController>();
        if (playerRef != null)
        {
            playerRef.StartCoroutine(PickedUp(playerRef));

        }
    }

    protected abstract IEnumerator PickedUp(PlayerController playerRef);

}
