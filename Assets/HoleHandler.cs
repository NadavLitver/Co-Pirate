using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class HoleHandler : MonoBehaviourPun
{
    public GameObject[] holes;
    public int id;
    public ShipManager myShip;
    

    public void localCallNewHoleRPC()
    {
        photonView.RPC("NewHoleRPC", RpcTarget.All);

    }
    [PunRPC]
    void NewHoleRPC()
    {
        int curIndex = Randomizer.RandomNum(holes.Length);
        holes[curIndex].transform.position = new Vector3(holes[curIndex].transform.position.x + Randomizer.NormalizedFloat(), holes[curIndex].transform.position.y, holes[curIndex].transform.position.z + Randomizer.NormalizedFloat());
        holes[curIndex].SetActive(true);
        myShip.CurHoleAmountActive++;

    }
}
