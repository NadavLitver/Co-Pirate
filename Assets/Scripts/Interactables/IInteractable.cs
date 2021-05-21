using UnityEngine;

public interface IInteractable
{
    void OnInteract();
    void OnBecomingTarget();
    void OnUnbecomingTarget();
    Sprite Icon { get; }
    bool InteractableCondition();
}
