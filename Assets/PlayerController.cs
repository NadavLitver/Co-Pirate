using CustomAttributes;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPunCallbacks
{
    public static PlayerController localPlayerCtrl;
    #region Serielized

    #region Settings
    [FoldoutGroup("Settings")]
    [SerializeField]
    private float speed;

    [FoldoutGroup("Settings")]
    [SerializeField]
    private float _interactionRange;

    [FoldoutGroup("Settings")]
    [SerializeField]
    private float maxAcceleration = 1;

    [FoldoutGroup("Settings")]
    [SerializeField, SuffixLabel("ApS"), Tooltip("Maximum rotation in angles per second")]
    private float maxRotationSpeed = 360;

    [FoldoutGroup("Settings")]
    [SerializeField]
    private float _gravity;
    #endregion

    #region Refrences
    [SerializeField, FoldoutGroup("Refrences")]
    private GameObject cannonBall;

    [FoldoutGroup("Refrences")]
    [SerializeField]
    Material redMat;

    [FoldoutGroup("Refrences")]
    [SerializeField]
    Material blueMat;

    [FoldoutGroup("Refrences")]
    [SerializeField]
    Material Hat;

    [FoldoutGroup("Refrences")]
    [SerializeField]
    Material Body;

    [FoldoutGroup("Refrences")]
    [SerializeField, LocalComponent(true, true)]
    private IconHandler _iconHandler;
    #endregion

    #endregion

    #region State
    [HideInInspector]
    public Camera personalCamera;
    //
    //private CameraWork cameraWork;

    InteractableHit curInteractableHit;

    CharacterController characterController;

    [ReadOnly]
    public bool isTeam1;
    Vector2 dir;
    Vector2 currentVelocity = Vector2.zero;
    Vector2 targetVelocity = Vector2.zero;

    private bool _holdingCannonBall = false;

    public bool HoldingCannonBall => _holdingCannonBall;
    #endregion
    private void Awake()
    {
        if (photonView.IsMine)
        {
            localPlayerCtrl = this;
        }
    }

    private void Start()
    {
        var playerNum = photonView.Owner.GetPlayerNum();
        PlayerInformation.players[playerNum].playerinstance = gameObject;
        isTeam1 = playerNum <= 2;

        Debug.Log("Name: " + photonView.Owner.NickName + ", Player num: " + playerNum + ", Islocal: " + photonView.Owner.IsLocal + ", is master: " + photonView.Owner.IsMasterClient);

        //GetComponent<MeshRenderer>().material = isTeam1 ? redMat : blueMat;
        if (isTeam1) 
        {
            Hat.SetColor("_MainColor", Color.red);
            Body.SetColor("_MainColor", Color.red);
        }
        else 
        {
            Hat.SetColor("_MainColor", Color.blue);
            Body.SetColor("_MainColor", Color.blue);
        }

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
            if (dir != Vector2.zero)
                targetVelocity = dir * speed * Time.deltaTime;

            Vector2 acceleration = targetVelocity - currentVelocity;

            currentVelocity += Vector2.ClampMagnitude(acceleration, maxAcceleration * Time.deltaTime);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, Vector3.SignedAngle(transform.forward, new Vector3(dir.x, 0, dir.y), Vector3.up), 0) * transform.rotation, maxRotationSpeed * Time.deltaTime);

            if (currentVelocity.magnitude >= 0.001f)
                CheckForInteractables();

            Vector3 forwardDirection = (transform.position - new Vector3(personalCamera.transform.position.x, transform.position.y, personalCamera.transform.position.z)).normalized;
            Vector3 rightDir = Vector3.Cross(Vector3.up, forwardDirection).normalized;
            Vector3 move = forwardDirection * currentVelocity.y + rightDir * currentVelocity.x;
            characterController.Move(move - _gravity * Vector3.up);
        }


    }

    private void CheckForInteractables()
    {
        InteractableHit hit = InteractablesObserver.GetClosestInteractable(this);
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
            curInteractableHit.interactable.OnInteract_Start(this);
            CheckForInteractables();
        }
    }
    public void ChangeCurInteratable(InteractableHit newInteractableHit)
    {
        if (curInteractableHit.interactable != newInteractableHit.interactable)
        {
            if (curInteractableHit.interactable != null)
                curInteractableHit.interactable.OnUnbecomingTarget(this);

            curInteractableHit = newInteractableHit;

            if (curInteractableHit.interactable != null)
                curInteractableHit.interactable.OnBecomingTarget(this);

            Sprite icon = (curInteractableHit.interactable == null ? null : curInteractableHit.interactable.Icon);

            _iconHandler.SetIcon(icon);
        }
    }
    public void SetCurInteractableNull()
    {
        curInteractableHit.interactable = null;
    }
    public void PickedUpCannonball()
    {
        photonView.RPC("PickedUpCannonballRPC", RpcTarget.All, photonView.Owner.GetPlayerNum());
        _holdingCannonBall = true;
    }
    [PunRPC]
    private void PickedUpCannonballRPC(int playerNum)
    {
        if (photonView.Owner.GetPlayerNum() == playerNum)
            GetComponent<PlayerController>().cannonBall.SetActive(true);
    }
    public void UsedCannonball()
    {
        photonView.RPC("UsedCannonballRPC", RpcTarget.All, photonView.Owner.GetPlayerNum());
        _holdingCannonBall = false;
    }
    [PunRPC]
    private void UsedCannonballRPC(int playerNum)
    {
        if (photonView.Owner.GetPlayerNum() == playerNum)
            GetComponent<PlayerController>().cannonBall.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _interactionRange);
    }
}
