using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CannonInteraction : BaseInteractable
{
    [SerializeField]
    private UnityEvent OnInteraction;

    [SerializeField]
   
    public override bool InteractableCondition(PlayerController ctrl) => ctrl != null && ctrl.HoldingCannonBall;
    public override void OnInteract_Start(PlayerController ctrl)
    {

        if (InteractableCondition(ctrl))
        {
            OnInteraction?.Invoke();
            Debug.Log("Interacted With Cannon");
            ctrl.UsedCannonball();

        }

    }
        
       
}
   

