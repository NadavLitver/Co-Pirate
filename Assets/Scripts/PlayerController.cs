using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class PlayerController : MonoBehaviourPunCallbacks
{
    public static PlayerController localPlayerCtrl;
    #region Serielized

    private ShipManager myShip;
    #region Settings
    [FoldoutGroup("Settings")]
    [SerializeField]
    private float speed;
    public float speedScalar;
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


    [FoldoutGroup("Refrences")]
    [SerializeField]
    TextMeshProUGUI nameText;

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
    [SerializeField]
    private IconHandler _iconHandler;

    [FoldoutGroup("Refrences")]
    [SerializeField]
    private Image _holeFixProgressBar;
    #endregion

    #region Events
    [SerializeField, TabGroup("Team", GroupID = "Events")]
    [FormerlySerializedAs("OnTeam1")]
    private UnityEvent OnTeamBlue;
    [SerializeField, TabGroup("Team", GroupID = "Events")]
    [FormerlySerializedAs("OnTeam2")]
    private UnityEvent OnTeamRed;
    [SerializeField, TabGroup("Cannon ball", GroupID = "Events")]
    private UnityEvent OnCannonballPickup;
    [SerializeField, TabGroup("Cannon ball", GroupID = "Events")]
    private UnityEvent OnCannonballUse;
    [SerializeField, TabGroup("Other", GroupID = "Events")]
    private UnityEvent<Camera> OnNewCamera;

    [SerializeField, TabGroup("Speed", GroupID = "Events/Buffs")]
    private UnityEvent OnSpeed_Enabled;
    [SerializeField, TabGroup("Speed", GroupID = "Events/Buffs")]
    private UnityEvent OnSpeed_Disabled;
    [SerializeField, TabGroup("Instant fix", GroupID = "Events/Buffs")]
    private UnityEvent OnInstantFix_Enabled;
    [SerializeField, TabGroup("Instant fix", GroupID = "Events/Buffs")]
    private UnityEvent OnInstantFix_Disabled;
    [SerializeField, TabGroup("Double shoot", GroupID = "Events/Buffs")]
    private UnityEvent OnDoubleShoot_Enabled;
    [SerializeField, TabGroup("Double shoot", GroupID = "Events/Buffs")]
    private UnityEvent OnDoubleShoot_Disabled;
    #endregion

    #endregion
    #region Buffs
    private bool _speedBuff = false;

    private bool _instantFixBuff = false;

    private bool _doubleShootBuff = false;

    public bool SpeedBuff
    {
        get => _speedBuff;
        set
        {
            if (_speedBuff == value)
                return;

            _speedBuff = value;

            if (_speedBuff)
                photonView.RPC("SpeedEnabledRPC", RpcTarget.All);
            else
                photonView.RPC("SpeedDisabledRPC", RpcTarget.All);

        }
    }
    [PunRPC]
    private void SpeedEnabledRPC() => OnSpeed_Enabled?.Invoke();
    [PunRPC]
    private void SpeedDisabledRPC() => OnSpeed_Disabled?.Invoke();

    public bool InstantFixBuff
    {
        get => _instantFixBuff;
        set
        {
            if (_instantFixBuff == value)
                return;

            _instantFixBuff = value;

            if (_instantFixBuff)
                photonView.RPC("InstantFixEnabledRPC", RpcTarget.All);
            else
                photonView.RPC("InstantFixDisabledRPC", RpcTarget.All);
        }
    }
    [PunRPC]
    private void InstantFixEnabledRPC() => OnInstantFix_Enabled?.Invoke();
    [PunRPC]
    private void InstantFixDisabledRPC() => OnInstantFix_Disabled?.Invoke();
    public bool DoubleShootBuff
    {
        get => _doubleShootBuff;
        set
        {
            if (_doubleShootBuff == value)
                return;

            _doubleShootBuff = value;

            if (_doubleShootBuff)
                photonView.RPC("DoubleShootEnabledRPC", RpcTarget.All);
            else
                photonView.RPC("DoubleShootDisabledRPC", RpcTarget.All);
        }
    }
    [PunRPC]
    private void DoubleShootEnabledRPC() => OnDoubleShoot_Enabled?.Invoke();
    [PunRPC]
    private void DoubleShootDisabledRPC() => OnDoubleShoot_Disabled?.Invoke();
    #endregion
    #region State
    [ReadOnly]
    private Camera personalCamera;
    public Camera PersonalCamera
    {
        get => personalCamera;
        set
        {
            if (personalCamera == value)
                return;

            personalCamera = value;

            OnNewCamera?.Invoke(personalCamera);
        }
    }
    //
    //private CameraWork cameraWork;

    InteractableHit curInteractableHit;

    CharacterController characterController;

    [ReadOnly]
    public Team team;
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
            GameManager.instance.OnGameStart += CheckForInteractables;
        }


    }

    private void Start()
    {
        var playerNum = photonView.Owner.GetPlayerNum();
        var playerData = PlayerInformation.players[playerNum];
        playerData.playerinstance = gameObject;
        nameText.text = playerData.player.NickName;
        team = playerData.team;
        transform.SetParent(playerData.spawnPoint.transform.GetComponentInParent<ShipManager>().transform);
        myShip = GetComponentInParent<ShipManager>();
        //if (team == Team.Blue)
        //{
        //    myShip.SetNameOnRedEndingCanvas(PhotonNetwork.NickName);
        //}
        //else
        //{
        //    myShip.SetNameOnBlueEndingCanvas(PhotonNetwork.NickName);

        //}
        Debug.Log("Name: " + photonView.Owner.NickName + ", Team: " + team.ToString() + ", Player num: " + playerNum + ", Islocal: " + photonView.Owner.IsLocal + ", is master: " + photonView.Owner.IsMasterClient);

        (team == Team.Blue ? OnTeamBlue : OnTeamRed)?.Invoke();

        PersonalCamera = localPlayerCtrl.team == Team.Blue ? GameManager.instance.blueCamera : GameManager.instance.redCamera;
        PersonalCamera.gameObject.SetActive(true);

        if (photonView.IsMine)
        {
            InputManager.controls.Gameplay.Enable();
            InputManager.controls.Gameplay.Move.performed += OnMove;
            InputManager.controls.Gameplay.Move.canceled += (x) => dir = Vector3.zero;
            InputManager.controls.Gameplay.Interact.performed += OnInteractPreformed;
            InputManager.controls.Gameplay.Interact.canceled += OnInteractCanceled;
            characterController = GetComponent<CharacterController>();




            var cameraCtrl = PersonalCamera.GetComponent<CameraController>();
            cameraCtrl.myfollow = transform;
        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            targetVelocity = Vector2.zero;
            if (dir != Vector2.zero)
                targetVelocity = dir * speed * speedScalar * Time.deltaTime;

            Vector2 acceleration = targetVelocity - currentVelocity;

            currentVelocity += Vector2.ClampMagnitude(acceleration, maxAcceleration * Time.deltaTime);


            if (currentVelocity.magnitude >= 0.001f)
                CheckForInteractables();

            Vector3 forwardDirection = (transform.position - new Vector3(PersonalCamera.transform.position.x, transform.position.y, PersonalCamera.transform.position.z)).normalized;
            Vector3 rightDir = Vector3.Cross(Vector3.up, forwardDirection).normalized;
            Vector3 move = forwardDirection * currentVelocity.y + rightDir * currentVelocity.x;

            if (move != Vector3.zero)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(move), maxRotationSpeed * Time.deltaTime);

            Physics.SyncTransforms();
            characterController.Move(move - _gravity * Vector3.up);

        }


    }

    private void CheckForInteractables(IInteractable interactable) => CheckForInteractables();
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

            if (curInteractableHit.interactable != null)
                curInteractableHit.interactable.InteractFinished += CheckForInteractables;
        }
    }
    void OnInteractCanceled(InputAction.CallbackContext context)
    {
        if (curInteractableHit.interactable != null && curInteractableHit.distance <= _interactionRange)
        {
            curInteractableHit.interactable.OnInteract_End(this);
            CheckForInteractables();

            if (curInteractableHit.interactable != null)
                curInteractableHit.interactable.InteractFinished -= CheckForInteractables;
        }
    }
    public void ChangeCurInteratable(InteractableHit newInteractableHit)
    {
        var curInteractable = curInteractableHit.interactable;

        var newInteractable = newInteractableHit.interactable;

        if (curInteractable != newInteractable)
        {
            if (_holeFixProgressBar != null)
            {
                if (curInteractable != null && curInteractable is HoleInteractable _hole)
                    FinishedFixing(_hole);

                if (newInteractable != null && newInteractable is HoleInteractable _newHole)
                    StartFixing(_newHole);
            }

            if (curInteractable != null)
            {
                curInteractable.OnUnbecomingTarget(this);
                if (curInteractable.IsInteracting)
                    curInteractable.OnInteract_End(this);
            }

            curInteractableHit = newInteractableHit;
            curInteractable = curInteractableHit.interactable;

            if (curInteractable != null)
                curInteractable.OnBecomingTarget(this);

            Sprite icon = (curInteractable == null ? null : curInteractable.Icon);



            _iconHandler.SetIcon(icon);


            //Anonymous methods
            void FinishedFixing(IInteractable interactable)
            {
                var hole = (HoleInteractable)interactable;

                hole.OnFixProgress -= UpdateFixProgressBar;
                hole.InteractFinished -= FinishedFixing;
                _holeFixProgressBar.fillAmount = 0;
                _holeFixProgressBar.gameObject.SetActive(false);
            }

            void StartFixing(HoleInteractable _newHole)
            {
                _holeFixProgressBar.gameObject.SetActive(true);
                _newHole.OnFixProgress += UpdateFixProgressBar;
                _newHole.InteractFinished += FinishedFixing;
            }
            void UpdateFixProgressBar(float progress)
            {
                if (_holeFixProgressBar != null && isActiveAndEnabled)
                    _holeFixProgressBar.fillAmount = progress;
            }
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
            OnCannonballPickup?.Invoke();
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
        {
            _holdingCannonBall = false;
            OnCannonballUse?.Invoke();
        }
    }
    /* [PunRPC]
     void SetNameRPC(int playerNum)
     {
         if (photonView.Owner.GetPlayerNum() == playerNum)
             nameHandler.nameText.text = nameHandler.playerName;

     }
     public void CallSetNameRPC()
     {
         photonView.RPC("SetNameRPC", RpcTarget.All, photonView.Owner.GetPlayerNum());
     }*/
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _interactionRange);
    }
}
