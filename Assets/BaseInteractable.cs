using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Sprite _icon;
    protected virtual void Awake()
    {
        InteractableHandler.Subscribe(this);
    }
    public abstract void OnInteract();
    public abstract void OnBecomingTarget();
    public abstract void OnUnbecomingTarget();
}
