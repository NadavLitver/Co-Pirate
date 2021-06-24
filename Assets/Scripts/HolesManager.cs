using Photon.Pun;
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
    public void NewHole()
    {
        photonView.RPC("NewHoleRPC", RpcTarget.All);
        Debug.Log("calling rpc");

    }
    [PunRPC]
    public void NewHoleRPC()
    {
        Debug.Log("enterRPC for new hole");

        var holesLeft = Array.FindAll(holes, (x) => !x.gameObject.activeSelf);
        if (holesLeft.Length != 0)
        {
            int curIndex = Randomizer.RandomNum(holesLeft.Length);
            //holes[curIndex].transform.position = new Vector3(holes[curIndex].transform.position.x + Randomizer.NormalizedFloat(), holes[curIndex].transform.position.y, holes[curIndex].transform.position.z + Randomizer.NormalizedFloat());
            holesLeft[curIndex].Init();
            myShip.CurHoleAmountActive++;
        }


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
        hole.Fix();

        myShip.CurHoleAmountActive--;

        hole.gameObject.SetActive(false);

        hole.FixRPC();
    }
    private void OnTriggerEnter(Collider other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball.Team != myShip.Team)
        {
            NewHole();
            Debug.Log("Team " + (myShip.Team ? 1 : 2) + " took damage!");
            ball.HIT();
        }
    }
}
