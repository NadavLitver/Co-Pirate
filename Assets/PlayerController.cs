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

    [SerializeField]
    private float _interactionRange;



    InteractableHit curInteractableHit;



    CharacterController characterController;

    private void Start()
    {
        InputManager.controls.Gameplay.Enable();
        InputManager.controls.Gameplay.Move.performed += OnMove;
        InputManager.controls.Gameplay.Move.canceled += (x) => dir = Vector3.zero;
        InputManager.controls.Gameplay.Interact.performed += OnInteractPreformed;
        characterController = GetComponent<CharacterController>();
    }
    private void FixedUpdate()
    {


        if (dir != Vector2.zero)
        {
            velocity = dir * speed * Time.deltaTime;
            characterController.Move(new Vector3(velocity.x, 0, velocity.y));
            CheckForInteractables();
        }


    }

    private void CheckForInteractables()
    {
        InteractableHit hit = InteractableHandler.GetClosestInteractable(transform.position);
        if ((curInteractableHit.interactable == null || curInteractableHit.distance > hit.distance) && hit.distance <= _interactionRange && hit.interactable != null)
            ChangeCurInteratable(hit);
        else if (hit.distance > _interactionRange)
            ChangeCurInteratable(new InteractableHit(hit.distance, null));
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();
    }
    void OnInteractPreformed(InputAction.CallbackContext context)
    {
        if (curInteractableHit.interactable != null && curInteractableHit.distance <= _interactionRange)
        {
            curInteractableHit.interactable.OnInteract();
        }
    }
    public void ChangeCurInteratable(InteractableHit newInteractableHit)
    {
        if (curInteractableHit.interactable != newInteractableHit.interactable)
        {
            if (curInteractableHit.interactable != null)
                curInteractableHit.interactable.OnUnbecomingTarget();

            curInteractableHit = newInteractableHit;

            if (curInteractableHit.interactable != null)
                curInteractableHit.interactable.OnBecomingTarget();
        }
    }
    public void SetCurInteractableNull()
    {
        curInteractableHit.interactable = null;
    }
}
