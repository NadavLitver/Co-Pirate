using System.Collections.Generic;
using UnityEngine;

public static class InteractableHandler
{
    public static List<BaseInteractable> interactables = new List<BaseInteractable>();
    public static void Subscribe(BaseInteractable interactable) => interactables.Add(interactable);
    public static InteractableHit GetClosestInteractable(Vector3 pos)
    {
        BaseInteractable closestInteractable = null;
        float closestInterDist = float.MaxValue;
        foreach (var inter in interactables)
        {
            float _distance = Vector3.Distance(inter.transform.position, pos);
            if (_distance < closestInterDist)
            {
                closestInteractable = inter;
                closestInterDist = _distance;
            }
        }

        return new InteractableHit(closestInterDist, closestInteractable);
    }

}
public struct InteractableHit
{
    public float distance;
    public BaseInteractable interactable;

    public InteractableHit(float distance, BaseInteractable interactable)
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
