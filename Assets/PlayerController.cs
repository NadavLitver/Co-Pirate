using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun.Demo.PunBasics;
using System.Collections;



public class PlayerController : MonoBehaviourPunCallbacks
{
    // Controls controls;


    Vector2 dir;
    Vector2 currentVelocity = Vector2.zero;
    Vector2 targetVelocity = Vector2.zero;
    [SerializeField]
    float speed;

    [SerializeField]
    private float _interactionRange;
    [SerializeField]
    private float gravityScale;
    [SerializeField]
    private float maxAcceleration = 1;

    private GameObject personalCamera;

   
    private CameraWork cameraWork;

    InteractableHit curInteractableHit;


    CharacterController characterController;

    private void Start()
    {
        InputManager.controls.Gameplay.Enable();
        InputManager.controls.Gameplay.Move.performed += OnMove;
        InputManager.controls.Gameplay.Move.canceled += (x) => dir = Vector3.zero;
        InputManager.controls.Gameplay.Interact.performed += OnInteractPreformed;
        characterController = GetComponent<CharacterController>();
        personalCamera = Camera.main.gameObject;
        cameraWork = GetComponent<CameraWork>();
        if (photonView.IsMine)
        {
            // personalCamera.SetActive(true);
            cameraWork.OnStartFollowing();
          

        }
    }
    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            targetVelocity = Vector2.zero;
            if(dir != Vector2.zero)
            {
                targetVelocity = dir * speed * Time.deltaTime;
                CheckForInteractables();
            }

            Vector2 acceleration = targetVelocity - currentVelocity;

            currentVelocity += Vector2.ClampMagnitude(acceleration, maxAcceleration * Time.deltaTime);
            
            characterController.Move(new Vector3(currentVelocity.x, 0, currentVelocity.y));
        }

       
    }
    // gravity
    // deaccleration
    // lerp on movement
   
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
