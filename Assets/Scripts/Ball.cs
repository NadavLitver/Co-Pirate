using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class Ball : MonoBehaviourPun
{
    [SerializeField]
    private float TTL;
    [SerializeField]
    float Speed;
    [SerializeField, MaxValue(0)]
    private float _gravity;

    [SerializeField]
    private UnityEvent OnHit;

    private float _verticalSpeed = 0;

    [SerializeField]
    private float damage;

    private Team _team;
    public Team Team => _team;

    Vector3 momentum;
    public void Init(Team team, Vector3 momentum)
    {
        this.momentum = momentum;
        _team = team;

        gameObject.SetActive(true);
        StartCoroutine(DestoryDelayRoutine());
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
        
       //
    }
    IEnumerator DestoryDelayRoutine()
    {
        yield return new WaitForSeconds(TTL);
        PhotonNetwork.Destroy(gameObject);

    }
}
