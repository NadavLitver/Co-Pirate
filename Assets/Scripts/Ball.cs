using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class Ball : MonoBehaviourPun
{
    [SerializeField]
    float Speed;
    [SerializeField, MaxValue(0)]
    private float _gravity;

    [SerializeField]
    private UnityEvent OnHit;

    private float _verticalSpeed = 0;

    [SerializeField]
    private float damage;

    private bool _team;
    public bool Team => _team;

    Vector3 momentum;
    public void Init(bool team, Vector3 momentum)
    {
        this.momentum = momentum;
        _team = team;

        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        var moveDir = (transform.forward * Speed + momentum + Vector3.up * _verticalSpeed) * Time.deltaTime;

        transform.Translate(moveDir, Space.World);

        _verticalSpeed += _gravity * Time.deltaTime;
    }

    public void HIT()
    {
        OnHit?.Invoke();
    }
    public void Destroy()
    {
        
       // PhotonNetwork.Destroy(gameObject);
    }
}
