using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShipManager : MonoBehaviourPun
{
    #region Serielized
    [SerializeField, ReadOnly] float curDamageLevel;
    float CurDamageLevel
    {
        get => curDamageLevel;


        set
        {
            value = Mathf.Clamp(value, 0, maxDamageLevel);

            if (curDamageLevel == value)
                return;

            curDamageLevel = value;

            UpdateShip();
        }

    }
    [SerializeField] float maxDamageLevel = 100;
    [SerializeField, ValueDropdown("TeamName")]
    private bool _team;
    public bool Team => _team;
    private ValueDropdownList<bool> TeamName => new ValueDropdownList<bool>() { new ValueDropdownItem<bool>("Team 1", true), new ValueDropdownItem<bool>("Team 2", false) };
    [Tooltip("The distance the ship sinks")]
    [SerializeField] float sinkDepth;

    public int CurHoleAmountActive = 0;
    [SerializeField] private float DPSPerHole = 1;

    [SerializeField] float maxRotation = -4;

    [SerializeField] float maxDegrees = 1;
    public Transform center;
    [SerializeField]
    private float _syncInterval = 5;
    #endregion
   
    float startHeight;
    Quaternion startRotation;

    #region Events
    [SerializeField, FoldoutGroup("Events", Order = 99)]
    private UnityEvent OnTakeDamage;
    [SerializeField, FoldoutGroup("Events", Order = 99)]
    private UnityEvent OnLose;
    [SerializeField, FoldoutGroup("Events", Order = 99)]
    private UnityEvent OnWin;

    #endregion

    private float _lastSyncTime = 0;


    private void Start()
    {
        startHeight = transform.position.y;
        startRotation = transform.rotation;
    }
    private void Update()
    {

        CurDamageLevel += CurHoleAmountActive * DPSPerHole * Time.deltaTime;

         if(PhotonNetwork.IsMasterClient && Time.time > _lastSyncTime + _syncInterval)
        {
            photonView.RPC("SyncDamageRPC", RpcTarget.Others, CurDamageLevel);
            _lastSyncTime = Time.time;
        }

    }

    [PunRPC]
    private void SyncDamageRPC(float damage) => CurDamageLevel = damage;
    void UpdateShip()
    {
        ChangeShipHeight();
        ChangeShipZRotation();

        if (CurDamageLevel == maxDamageLevel)
            Lose();
    }
    //public void TakeDamage(float damage)
    //{
    //    photonView.RPC("TakeDamageRPC", RpcTarget.All);

    //}//NOT IN USE
    //[PunRPC]
    //public void TakeDamageRPC(float damage, int _shipID)//take damage in game from here instead of through the slider
    //{// NOT IN USE
    //    CurDamageLevel += damage;

    //    OnTakeDamage?.Invoke();


    //}
    void ChangeShipHeight()
    {

        transform.position = new Vector3(transform.position.x, Mathf.Lerp(startHeight, startHeight - sinkDepth, curDamageLevel / maxDamageLevel), transform.position.z);
    }
    void ChangeShipZRotation()
    {
        transform.rotation = Quaternion.Euler(transform.TransformVector(Mathf.DeltaAngle(transform.rotation.eulerAngles.x, Mathf.Lerp(0, maxRotation, curDamageLevel / maxDamageLevel)), 0, 0)) * transform.rotation;
    }
    private void Lose()
    {
        if (PhotonNetwork.IsMasterClient)
            photonView.RPC("LoseRPC", RpcTarget.All);
    }
    [PunRPC]
    private void LoseRPC()
    {
        OnLose?.Invoke();
    }
    public void Win()
    {
        OnWin?.Invoke();
    }
}
