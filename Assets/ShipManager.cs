using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    [Tooltip("The distance the ship sinks")]
    [SerializeField] float sinkDistance;


    [SerializeField] float maxRotation = -4;


    [SerializeField] const float maxDamagedLevel = 100;
    [Range(0.0f, maxDamagedLevel)]
    [SerializeField] float curDamagedLevel;
    [SerializeField] float maxDegrees = 1;
    float DamageDelta;
    float RotationDelta;
  


    private void Update()
    {

        DamageDelta = curDamagedLevel * sinkDistance * 0.01f;
        RotationDelta = curDamagedLevel * maxRotation * 0.01f;
        UpdateShip();

    }

    void UpdateShip()
    {
     
        ChangeShipHeight();
        ChangeShipZRotation();
    }
    void TakeDamage(float damage)//take damage in game from here instead of through the slider
    {
        curDamagedLevel += damage;
        if(curDamagedLevel == maxDamagedLevel)
        {
            Lose();
        }
    }
    void ChangeShipHeight()
    {
      
       transform.position = new Vector3(transform.position.x, Mathf.MoveTowards(transform.position.y, DamageDelta, Time.deltaTime), transform.position.z);
    }
    void ChangeShipZRotation()
    {
      
     // transform.rotation = Quaternion.Lerp(transform.rotation)
     transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(RotationDelta,0, 0), maxDegrees * Time.deltaTime);

    }
    private void Lose()
    {
        
    }
}
