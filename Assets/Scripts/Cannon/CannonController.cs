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
        PhotonNetwork.Instantiate(_cannonBall.name, _barrelEdge.transform.position, Quaternion.LookRotation(_barrelEdge.transform.position - transform.position, transform.up));
    }
}
