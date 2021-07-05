using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Pickup : MonoBehaviour
{
    [SerializeField]
    protected UnityEvent OnPickUp;
    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerRef = other.GetComponent<PlayerController>();
        if (playerRef != null)
            PickedUp(playerRef);
    }

    protected virtual void PickedUp(PlayerController playerRef)
    {
        OnPickUp?.Invoke();
        Destroy(gameObject);
    }

}
