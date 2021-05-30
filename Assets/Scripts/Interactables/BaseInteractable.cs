using UnityEngine;

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Sprite _icon;

    public Sprite Icon => _icon;

    void OnEnable()
    {
        this.Subscribe();
    }
    void OnDisable()
    {
        this.Unsubscribe();
    }
    public abstract void OnInteract_Start(PlayerController ctrl);
    public virtual void OnInteract_End(PlayerController ctrl) { }
    public abstract void OnBecomingTarget(PlayerController ctrl);
    public abstract void OnUnbecomingTarget(PlayerController ctrl);
    public virtual bool InteractableCondition() => true;

}
