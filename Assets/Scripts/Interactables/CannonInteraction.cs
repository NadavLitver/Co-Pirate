using UnityEngine;
using UnityEngine.Events;

public class CannonInteraction : BaseInteractable
{
    [SerializeField]
    private UnityEvent OnInteraction;


    public override bool InteractableCondition(PlayerController ctrl) => ctrl != null && ctrl.HoldingCannonBall;
    public override void OnInteract_Start(PlayerController ctrl)
    {
        if (ctrl.DoubleShootBuff)
            ctrl.DoubleShootBuff = false;
        else
            ctrl.UsedCannonball();

        OnInteraction?.Invoke();
        Debug.Log("Interacted With Cannon");

    }


}


