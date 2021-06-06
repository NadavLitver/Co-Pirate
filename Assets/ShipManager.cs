using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class ShipManager : MonoBehaviourPun
{
    #region Serielized
    [SerializeField, ValueDropdown("TeamName")]
    private bool _team;
    public bool Team => _team;
    private ValueDropdownList<bool> TeamName => new ValueDropdownList<bool>() { new ValueDropdownItem<bool>("Team 1", true), new ValueDropdownItem<bool>("Team 2", false) };
    [Tooltip("The distance the ship sinks")]
    [SerializeField] float sinkDistance;

    public int CurHoleAmountActive = 0;

    [SerializeField] float maxRotation = -4;

    private float damageOverTime;

    [SerializeField] const float maxDamagedLevel = 100;
    float curDamagedLevel;
    [SerializeField] float maxDegrees = 1;
    float DamageDelta;
    #endregion

    #region Events
    [SerializeField, FoldoutGroup("Events", Order = 99)]
    private UnityEvent OnTakeDamage;
    #endregion
    private float RotationDelta;

    float CurDamagedLevel
    {
        get => curDamagedLevel;


        set
        {
            if (curDamagedLevel == value)
                return;

            curDamagedLevel = value;
            DamageDelta = curDamagedLevel * sinkDistance * 0.01f;
            RotationDelta = curDamagedLevel * maxRotation * 0.01f;


        }

    }


    private void Update()
    {

        /* if (Input.GetKeyDown(KeyCode.Space))
         {
             TakeDamage(90); // just a test dont judge me 
         }*/
        UpdateShip();

    }

    void UpdateShip()
    {
        damageOverTime = CurHoleAmountActive * Time.deltaTime;
        CurDamagedLevel += damageOverTime;
        ChangeShipHeight();
        ChangeShipZRotation();
    }
    public void TakeDamage(float damage)
    {
        photonView.RPC("TakeDamageRPC", RpcTarget.All);

    }//NOT IN USE
    [PunRPC]
    public void TakeDamageRPC(float damage, int _shipID)//take damage in game from here instead of through the slider
    {// NOT IN USE
        CurDamagedLevel += damage;

        OnTakeDamage?.Invoke();

        if (CurDamagedLevel == maxDamagedLevel)
            Lose();
    }
    void ChangeShipHeight()
    {

        transform.position = new Vector3(transform.position.x, Mathf.MoveTowards(transform.position.y, DamageDelta, Time.deltaTime), transform.position.z);
    }
    void ChangeShipZRotation()
    {

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(RotationDelta, 0, 0), maxDegrees * Time.deltaTime);

    }
    private void Lose()
    {

    }

}
