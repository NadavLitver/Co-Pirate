using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTest : InteractableHandler
{
  

    public override void OnInteract()
    {
      
    }

    private void OnTriggerEnter(Collider col)
    {
        col.GetComponent<PlayerController>().ChangeCurInteratable(this);
    }
    private void OnTriggerExit(Collider col)
    {
        col.GetComponent<PlayerController>().SetcurInteractableNull();
    }
  
       
}
