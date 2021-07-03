using Photon.Pun;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
[SelectionBase]
public class HolesManager : MonoBehaviourPun
{
    public HoleController[] holes;
    public ShipManager myShip;

    private void Awake()
    {
        Array.ForEach(holes, (x) => x.manager = this);
    }
    [PunRPC]
    public void NewHoleRPC(int curIndex)
    {
          Debug.Log("enterRPC for new hole");

      
            holes[curIndex].Init();
            myShip.CurHoleAmountActive++;


    }

    public void FixedHole(HoleController hole)
    {
        int index = Array.FindIndex(holes, (x) => x == hole);

        if (index != -1)
            photonView.RPC("FixedHoleRPC", RpcTarget.All, index);
    }
    [PunRPC]
    private void FixedHoleRPC(int index)
    {
        var hole = holes[index];

        myShip.CurHoleAmountActive--;

        hole.gameObject.SetActive(false);

        hole.FixRPC();
    }
    private void OnTriggerEnter(Collider other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball == null)
            return;
        if (ball.Team != myShip.Team)
        {
            // other.enabled = false;
           
                Debug.Log("Team " + (myShip.Team ? 1 : 2) + " took damage!");
                ball.HIT();
                NewHole();
            
        
        }
    }
  
    [Button, HideInEditorMode]
    public void NewHole()
    {
        var holesLeft = Array.FindAll(holes, (x) => !x.gameObject.activeSelf);
        if (holesLeft.Length != 0)
        {
            int curIndex = Randomizer.RandomNum(holesLeft.Length);
            Debug.Log("HolesLength" + holesLeft.Length + "Current index" + curIndex);
            photonView.RPC("NewHoleRPC", RpcTarget.All,curIndex);
            Debug.Log("calling rpc");
        }
     

    }
}
