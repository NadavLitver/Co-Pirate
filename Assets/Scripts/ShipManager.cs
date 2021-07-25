using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

    public enum Team { Red, Blue}
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
    //public Text[] redTeamEndingText;
    //public Text[] blueTeamEndingText;
    [SerializeField] float maxDamageLevel = 100;
    [SerializeField] private Team _team;
    public Team Team => _team;
    [Tooltip("The distance the ship sinks")]
    [SerializeField] float sinkDepth;
    [SerializeField] private float DPSPerHole = 1;
    [SerializeField] float maxRotation = -4;
    [SerializeField] private float _syncInterval = 5;

    public int CurHoleAmountActive = 0;
    public Transform center;
    #endregion
   
    float startHeight;

    #region Events
    [SerializeField, FoldoutGroup("Events", Order = 99)]
    private UnityEvent OnTakeDamage;
    [SerializeField, FoldoutGroup("Events", Order = 99)]
    private UnityEvent OnLose;
    [SerializeField, FoldoutGroup("Events", Order = 99)]
    private UnityEvent OnWin;

    #endregion



    private void Start()
    {
        startHeight = transform.position.y;
    }
    private void FixedUpdate()
    {
#if UNITY_EDITOR
        if (PhotonNetwork.IsMasterClient && Input.GetKeyDown(KeyCode.P))
            CurDamageLevel = maxDamageLevel;
        else
#endif
        CurDamageLevel += CurHoleAmountActive * DPSPerHole * Time.deltaTime;
    }
    void UpdateShip()
    {
        ChangeShipHeight();
        ChangeShipZRotation();

        if (CurDamageLevel == maxDamageLevel)
            Lose();
    }
    //public void SetNameOnRedEndingCanvas(string name)
    //{
    //    for (int i = 0; i < redTeamEndingText.Length; i++)
    //    {
    //        redTeamEndingText[i].text += " " + name;
    //    }
    //}
    //public void SetNameOnBlueEndingCanvas(string name)
    //{
    //    for (int i = 0; i < blueTeamEndingText.Length; i++)
    //    {
    //        blueTeamEndingText[i].text += " " + name;
    //    }
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
