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

    private void Awake()
    {
        _ship = GetComponentInParent<ShipManager>();
        _shipMover = _ship.GetComponent<ShipMovement>();
    }
    public void SHOOT()
    {
        var forwardDirection = Vector3.ProjectOnPlane(_barrelEdge.transform.position - transform.position, Vector3.up);

        Debug.DrawLine(_barrelEdge.transform.position, _barrelEdge.transform.position + forwardDirection, Color.red, 5);

        var rotation = Quaternion.LookRotation(forwardDirection);

        var ballObj = PhotonNetwork.Instantiate(_cannonBall.name, _barrelEdge.transform.position, rotation);

        var cannonBall = ballObj.GetComponent<Ball>();

        var shipSpeed = _shipMover.Speed;
        cannonBall.Init(_ship.Team, new Vector3(shipSpeed.x, 0, shipSpeed.y));


        photonView.RPC("ShootRPC", RpcTarget.All);
    }

    [PunRPC]
    private void ShootRPC()
    {
        OnShoot?.Invoke();
    }
}
