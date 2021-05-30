using System.Collections.Generic;
using UnityEngine;

public static class InteractablesObserver
{
    public static List<IInteractable> interactables = new List<IInteractable>();
    public static void Subscribe(this IInteractable interactable)
    {
        if(!interactables.Contains(interactable))
            interactables.Add(interactable);
    }
    public static void Unsubscribe(this IInteractable interactable)
    {
        interactables.Remove(interactable);
    }
    public static InteractableHit GetClosestInteractable(Vector3 pos)
    {
        IInteractable closestInteractable = null;
        float closestInterDist = float.MaxValue;
        foreach (var inter in interactables)
        {
            if (inter is Component _interactableComponent && inter.InteractableCondition())
            {
                float _distance = Vector3.Distance(_interactableComponent.transform.position, pos);

                if (_distance < closestInterDist)
                {
                    closestInteractable = inter;
                    closestInterDist = _distance;
                }
            }
        }

        return new InteractableHit(closestInterDist, closestInteractable);
    }

}
public struct InteractableHit
{
    public float distance;
    public IInteractable interactable;

    public InteractableHit(float distance, IInteractable interactable)
    {
        this.distance = distance;
        this.interactable = interactable;
    }

    public override bool Equals(object obj)
    {
        if (obj is InteractableHit _otherHit)
            return distance == _otherHit.distance && interactable == _otherHit.interactable;
        else
            return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
