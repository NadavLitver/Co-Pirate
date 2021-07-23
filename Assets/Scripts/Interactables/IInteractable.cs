using System;
using UnityEngine;

public interface IInteractable
{
    void OnInteract_Start(PlayerController ctrl);
    void OnInteract_End(PlayerController ctrl);
    void OnBecomingTarget(PlayerController ctrl);
    void OnUnbecomingTarget(PlayerController ctrl);
    Sprite Icon { get; }
    bool InteractableCondition(PlayerController ctrl);
    event Action<IInteractable> InteractFinished;
    bool IsInteracting { get; }
}
