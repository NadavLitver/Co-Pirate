using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Ball : MonoBehaviourPun
{
    [SerializeField]
    float Speed;
    [SerializeField, MaxValue(0)]
    private float _gravity;

    private float _verticalSpeed;

    [SerializeField]
    private float damage;

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        var moveDir = (transform.forward * Speed + Vector3.up * _verticalSpeed) * Time.deltaTime;

        transform.Translate(moveDir, Space.World);

        _verticalSpeed += _gravity * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        // other.GetComponentInParent<ShipManager>().;
        other.GetComponent<HoleHandler>().localCallNewHoleRPC();
    }    
        
    
}
