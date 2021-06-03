using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CannonInteraction : BaseInteractable
{
    [SerializeField]
    private UnityEvent OnInteraction;
    public override bool InteractableCondition(PlayerController ctrl) => ctrl != null && ctrl.HoldingCannonBall;
    public override void OnInteract_Start(PlayerController ctrl)
    {
        OnInteraction?.Invoke();
        ctrl.UsedCannonball();
    }
}
