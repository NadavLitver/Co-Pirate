using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviourPun
{
    [Tooltip("The distance the ship sinks")]
    [SerializeField] float sinkDistance;

    public int CurHoleAmountActive;

    [SerializeField] float maxRotation = -4;

   private float damageOverTime;

    [SerializeField] const float maxDamagedLevel = 100;
    float curDamagedLevel;
    [SerializeField] float maxDegrees = 1;
    float DamageDelta;
    float RotationDelta;

     public int shipID;

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

       /* if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(90); // just a test dont judge me 
        }*/
        UpdateShip();

    }

    void UpdateShip()
    {
        damageOverTime = CurHoleAmountActive * Time.deltaTime;
        ChangeShipHeight();
        ChangeShipZRotation();
    }
    public void localCallTakeDamageRPC(float damage)
    {
        photonView.RPC("TakeDamageRPC", RpcTarget.All, damage,shipID);

    }
    [PunRPC]
    public  void TakeDamageRPC(float damage,int _shipID)//take damage in game from here instead of through the slider
    {
        if(_shipID == shipID)
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
