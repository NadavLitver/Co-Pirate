using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//
public class PlayerController : MonoBehaviour
{
    // Controls controls;
    Vector2 dir;
    Vector2 velocity;
    [SerializeField]
    float speed;


    
    InteractableHandler curInteractableHandlers;

    

    CharacterController characterController;

    private void Start()
    {
        InputManager.controls.Gameplay.Enable();
        InputManager.controls.Gameplay.Move.performed += OnMove;
        InputManager.controls.Gameplay.Interact.performed += OnInteractPreformed;
        characterController = GetComponent<CharacterController>();
    }
    private void FixedUpdate()
    {
        velocity = dir * speed * Time.deltaTime;
        characterController.Move(new Vector3(velocity.x,0,velocity.y));
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();
    }
    void OnInteractPreformed(InputAction.CallbackContext context)
    {
        if(curInteractableHandlers != null)
        {
            curInteractableHandlers.OnInteract();
        }
    }
    public void ChangeCurInteratable(InteractableHandler newInteractable)
    {
        curInteractableHandlers = newInteractable;
    }
    public void SetcurInteractableNull()
    {
        curInteractableHandlers = null;
    }
}
