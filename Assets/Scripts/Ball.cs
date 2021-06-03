using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    float Speed;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * Speed);
    }
}
