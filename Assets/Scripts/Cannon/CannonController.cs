using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Photon.Pun;
using UnityEngine.Events;

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
    public void SHOOT()
    {
        photonView.RPC("ShootRPC", RpcTarget.All);
    }

    [PunRPC]
    private void ShootRPC()
    {
        var forwardDirection = Vector3.ProjectOnPlane(_barrelEdge.transform.position - transform.position, Vector3.up);

        Debug.DrawLine(_barrelEdge.transform.position, _barrelEdge.transform.position + forwardDirection, Color.red, 5);

        var rotation = Quaternion.LookRotation(forwardDirection);

        //PhotonNetwork.Instantiate(_cannonBall.name, _barrelEdge.transform.position, rotation);
        Instantiate(_cannonBall, _barrelEdge.transform.position, rotation);

        OnShoot?.Invoke();
    }
}
