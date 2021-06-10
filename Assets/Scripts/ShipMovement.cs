using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    Vector3 startPos;

    [SerializeField]
    Vector2 radius;
    [SerializeField]
    float cycleDuration;

    Vector3 lastPos;
    private void Start()
    {
        startPos = transform.position;
        lastPos = startPos - transform.forward;
    }
    private void Update()
    {

        
       transform.position = new Vector3(startPos.x + Mathf.Cos((Time.time * (Mathf.PI * 2)) / cycleDuration) * radius.x, startPos.y, startPos.z + Mathf.Sin((Time.time * (Mathf.PI * 2)) / cycleDuration) * radius.y);
       Vector3 lookDir = transform.position - lastPos;
       transform.rotation = Quaternion.LookRotation(lookDir);
       lastPos = transform.position;
    }
}
