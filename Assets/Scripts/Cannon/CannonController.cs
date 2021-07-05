using Photon.Pun;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]

public class CannonController : MonoBehaviourPun
{
    #region Serielized
    [SerializeField]
    private GameObject _barrelEdge;
    [SerializeField, AssetSelector(Paths = "Assets/Resources")]
    private GameObject _cannonBall;
    [SerializeField]
    private UnityEvent OnShoot;

    #endregion

    private ShipManager _ship;
    private ShipMovement _shipMover;


    float ShootCD = 0.5f;
    float shootCDCurrent;

    private void Awake()
    {
        _ship = GetComponentInParent<ShipManager>();
        _shipMover = _ship.GetComponent<ShipMovement>();
    }
    public void SHOOT()
    {
        if (CanShoot())
        {
            var forwardDirection = Vector3.ProjectOnPlane(_barrelEdge.transform.position - transform.position, Vector3.up);

            Debug.DrawLine(_barrelEdge.transform.position, _barrelEdge.transform.position + forwardDirection, Color.red, 5);

            var rotation = Quaternion.LookRotation(forwardDirection);
            var position = _barrelEdge.transform.position;

            Debug.Log("Shot");
            photonView.RPC("ShootRPC", RpcTarget.All, new object[] { _barrelEdge.transform.position, rotation });
        }
     
    }

    [PunRPC]
    private void ShootRPC(Vector3 barrelPosition, Quaternion ballRotation)
    {
        var ballObj = Instantiate(_cannonBall, barrelPosition, ballRotation);

        var cannonBall = ballObj.GetComponent<Ball>();

        var shipSpeed = _shipMover.Speed;
        cannonBall.Init(_ship.Team, new Vector3(shipSpeed.x, 0, shipSpeed.y));

        OnShoot?.Invoke();
    }
    private void Update()
    {
        shootCDCurrent += Time.deltaTime;
    }
    bool CanShoot()
    {
      if(shootCDCurrent >= ShootCD)
      {
            shootCDCurrent = 0;
            return true;
      }

        return false;
    }
}
