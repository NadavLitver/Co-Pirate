using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using CustomAttributes;
using System.Collections.Generic;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
{
    // Controls controls;

    [SerializeField]
    Material redMat;

    [SerializeField]
    Material blueMat;

    Vector2 dir;
    Vector2 currentVelocity = Vector2.zero;
    Vector2 targetVelocity = Vector2.zero;
    [SerializeField]
    float speed;

    [SerializeField]
    private float _interactionRange;
    [SerializeField]
    private float maxAcceleration = 1;
    [SerializeField]
    private float _gravity;

  
    public bool isTeam1;

    [SerializeField, LocalComponent(true, true)]
    private IconHandler _iconHandler;

    public Camera personalCamera;
    //
    //private CameraWork cameraWork;

    InteractableHit curInteractableHit;

    static int playerCount;

    CharacterController characterController;

    public Player player;

    private void Start()
    {

        player = PlayerNumbering.SortedPlayers[playerCount];
        playerCount++;
        isTeam1 = player.GetPlayerNumber() <= 2;

        GetComponent<MeshRenderer>().material = isTeam1 ? redMat : blueMat;

        Debug.Log(isTeam1);

        if (photonView.IsMine)
        {
            InputManager.controls.Gameplay.Enable();
            InputManager.controls.Gameplay.Move.performed += OnMove;
            InputManager.controls.Gameplay.Move.canceled += (x) => dir = Vector3.zero;
            InputManager.controls.Gameplay.Interact.performed += OnInteractPreformed;
            characterController = GetComponent<CharacterController>();


            
            personalCamera = isTeam1 ? GameManager.Instance.redCamera : GameManager.Instance.blueCamera;
            personalCamera.gameObject.SetActive(true);
            personalCamera.GetComponent<CameraController>().myfollow = transform;
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            targetVelocity = Vector2.zero;
            if(dir != Vector2.zero)
                targetVelocity = dir * speed * Time.deltaTime;

            Vector2 acceleration = targetVelocity - currentVelocity;

            currentVelocity += Vector2.ClampMagnitude(acceleration, maxAcceleration * Time.deltaTime);

            if (currentVelocity.magnitude >= 0.001f)
                CheckForInteractables();

            Vector3 forwardDirection = (transform.position - new Vector3(personalCamera.transform.position.x, transform.position.y, personalCamera.transform.position.z)).normalized;
            Vector3 rightDir = Vector3.Cross(Vector3.up, forwardDirection).normalized;
            Vector3 move = forwardDirection * currentVelocity.y + rightDir * currentVelocity.x;
            characterController.Move(move -_gravity * Vector3.up);
        }

       
    }
   
    private void CheckForInteractables()
    {
        InteractableHit hit = InteractablesObserver.GetClosestInteractable(transform.position);
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

            Sprite icon = (curInteractableHit.interactable == null ? null : curInteractableHit.interactable.Icon);

            _iconHandler.SetIcon(icon);
        }
    }
    public void SetCurInteractableNull()
    {
        curInteractableHit.interactable = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _interactionRange);
    }
}
