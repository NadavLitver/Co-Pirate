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
    float curDamagedLevel;
    [SerializeField] float maxDegrees = 1;
    float DamageDelta;
    float RotationDelta;


    float CurDamagedLevel {
        get => curDamagedLevel;
            
            
          set{
                 if (curDamagedLevel == value)
                     return;

                curDamagedLevel = value;
                DamageDelta = curDamagedLevel * sinkDistance * 0.01f;
                RotationDelta = curDamagedLevel * maxRotation * 0.01f;


          }
    
    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(90); // just a test dont judge me 
        }
        UpdateShip();

    }

    void UpdateShip()
    {
     
        ChangeShipHeight();
        ChangeShipZRotation();
    }
     public  void TakeDamage(float damage)//take damage in game from here instead of through the slider
    {
      
        CurDamagedLevel += damage;
        if(CurDamagedLevel == maxDamagedLevel)
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
      
     transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.Euler(RotationDelta,0, 0), maxDegrees * Time.deltaTime);

    }
    private void Lose()
    {
        
    }
}
