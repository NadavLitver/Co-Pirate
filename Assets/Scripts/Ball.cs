using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    float Speed;

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * Speed * Time.deltaTime);
    }
}
