using CustomAttributes;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerLobbyController : MonoBehaviourPunCallbacks
{
    
   
        public static PlayerLobbyController localPlayerCtrl;
        #region Serielized

        #region Settings
        [FoldoutGroup("Settings")]
        [SerializeField]
        private float speed;

        private string playerName;
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
     

    [FoldoutGroup("Refrences")]
        [SerializeField, LocalComponent(true, true)]
        private IconHandler _iconHandler;
        #endregion

        #region Events
        [SerializeField, FoldoutGroup("Events", 99, Expanded = false)]
        public UnityEvent OnTeam1;
        [SerializeField, FoldoutGroup("Events", 99, Expanded = false)]
        public UnityEvent OnTeam2;
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
        PlayerData playerData;
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
            playerData = PlayerInformation.players[playerNum];
            playerData.playerinstance = gameObject;
            isTeam1 = photonView.Owner.GetPlayerTeam();
            playerName = PhotonNetwork.NickName;

            //transform.SetParent(playerData.spawnPoint.transform.GetComponentInParent<ShipManager>().transform);

        Debug.Log("Name: " + photonView.Owner.NickName + ", Player num: " + playerNum + ", Islocal: " + photonView.Owner.IsLocal + ", is master: " + photonView.Owner.IsMasterClient);

            (isTeam1 ? OnTeam1 : OnTeam2)?.Invoke();

            Debug.Log(isTeam1);

            if (photonView.IsMine)
            {
                InputManager.controls.Gameplay.Enable();
                InputManager.controls.Gameplay.Move.performed += OnMove;
                InputManager.controls.Gameplay.Move.canceled += (x) => dir = Vector3.zero;
                //InputManager.controls.Gameplay.Interact.performed += OnInteractPreformed;
                characterController = GetComponent<CharacterController>();



               personalCamera = Camera.main;

                personalCamera.gameObject.SetActive(true);
                var cameraCtrl = personalCamera.GetComponent<CameraController>();
               // LiveLobbyGameManager.Instance.readyButton.onClick.AddListener(SetLocalReady);

            // cameraCtrl.myfollow = transform;
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


               

                Vector3 forwardDirection = (transform.position - new Vector3(personalCamera.transform.position.x, transform.position.y, personalCamera.transform.position.z)).normalized;
                Vector3 rightDir = Vector3.Cross(Vector3.up, forwardDirection).normalized;
                Vector3 move = forwardDirection * currentVelocity.y + rightDir * currentVelocity.x;

                if (move != Vector3.zero)
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(move), maxRotationSpeed * Time.deltaTime);

                Physics.SyncTransforms();
                characterController.Move(move - _gravity * Vector3.up);

            }


        }
        public void SetPlayerData()
        {
        playerData.isTeam1 = isTeam1;

           
         }
    [PunRPC]
        public void SetPlayerDataRpc()
        {
        //playerData.isTeam1 = isTeam1;

        }
    [PunRPC]
    void SetNameRPC()
    {
        NameTextHandler nameHandler = this.GetComponentInChildren<NameTextHandler>();
        nameHandler.nameText.text = nameHandler.name;

    }
    public void CallSetNameRPC()
    {
        photonView.RPC("SetNameRPC", RpcTarget.All);
    }
    private void OnMove(InputAction.CallbackContext context)
    {
            dir = context.ReadValue<Vector2>();
    }
     
      
    
       

      
    
}
