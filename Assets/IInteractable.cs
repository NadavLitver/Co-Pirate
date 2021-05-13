using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void OnInteract();
    void OnBecomingTarget();
    void OnUnbecomingTarget();

    Sprite Icon { get; }
}
