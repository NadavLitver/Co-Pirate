using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class HoleHandler : MonoBehaviourPun
{
    public GameObject[] holes;
    public int id;
    public ShipManager myShip;
    

    public void localCallNewHoleRPC(int ID)
    {
        photonView.RPC("NewHoleRPC", RpcTarget.All,ID);
        Debug.Log("calling rpc");

    }
    [PunRPC]
    public void NewHoleRPC(int ID)
    {
        Debug.Log("enterRPC for new hole");
        if(ID == this.id)
        {
            Debug.Log("ID == this.id");

            int curIndex = Randomizer.RandomNum(holes.Length);
            holes[curIndex].transform.position = new Vector3(holes[curIndex].transform.position.x + Randomizer.NormalizedFloat(), holes[curIndex].transform.position.y, holes[curIndex].transform.position.z + Randomizer.NormalizedFloat());
            holes[curIndex].SetActive(true);
            myShip.CurHoleAmountActive++;
        }
       

    }
}
