using System;
using UnityEngine;
using Photon.Pun;

public abstract class BaseInteractable : MonoBehaviourPun, IInteractable
{
    [SerializeField]
    private Sprite _icon;

    public virtual event Action<IInteractable> InteractFinished;

    public Sprite Icon => _icon;
    public abstract bool IsInteracting { get; }

    void OnEnable()
    {
        this.Subscribe();
    }
    void OnDisable()
    {
        this.Unsubscribe();
    }
    public abstract void OnInteract_Start(PlayerController ctrl);
    public virtual void OnInteract_End(PlayerController ctrl) { InteractFinished?.Invoke(this); }
    public virtual void OnBecomingTarget(PlayerController ctrl) { }
    public virtual void OnUnbecomingTarget(PlayerController ctrl) { }
    public virtual bool InteractableCondition(PlayerController ctrl) => true;

}
