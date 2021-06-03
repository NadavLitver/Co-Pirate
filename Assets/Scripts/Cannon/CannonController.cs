using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Photon.Pun;

public class CannonController : MonoBehaviour
{
    #region Serielized
    [SerializeField]
    private GameObject _barrelEdge;
    [SerializeField, AssetSelector(Paths = "Assets/Resources")]
    private GameObject _cannonBall;


    #endregion
    public void SHOOT()
    {
        var forwardDirection = Vector3.ProjectOnPlane(_barrelEdge.transform.position - transform.position, Vector3.up);
        
        Debug.DrawLine(_barrelEdge.transform.position, _barrelEdge.transform.position + forwardDirection, Color.red, 5);
        
        var rotation =  Quaternion.LookRotation(forwardDirection);

        PhotonNetwork.Instantiate(_cannonBall.name, _barrelEdge.transform.position, rotation);
    }
}
