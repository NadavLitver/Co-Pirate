using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Sprite _icon;

    public Sprite Icon => _icon;

    void OnEnable()
    {
        InteractablesObserver.Subscribe(this);
    }
    void OnDisable()
    {
        InteractablesObserver.Unsubscribe(this);
    }
    public abstract void OnInteract();
    public abstract void OnBecomingTarget();
    public abstract void OnUnbecomingTarget();
    public virtual bool InteractableCondition() => true;
}
