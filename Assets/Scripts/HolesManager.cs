using Photon.Pun;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
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
    public void FixedHole(HoleController hole)
    {
        Debug.Log("Fixed hole");

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

        Debug.Log("Ball from team " + ball.Team + " entered collision with ship: " + myShip.gameObject.name);

        if (ball.Team != myShip.Team)
        {
            // other.enabled = false;

            Debug.Log("Team " + myShip.Team + " took damage!");
            ball.HIT();
            if (PhotonNetwork.IsMasterClient)
                NewHole();
        }
    }

    [Button, HideInEditorMode]
    public void NewHole()
    {

        List<(HoleController hole, int index)> _indexedHoles = new List<(HoleController hole, int index)>(holes.Length);

        for (int i = 0; i < holes.Length; i++)
            _indexedHoles[i] = (holes[i], i);

        var holesLeft = _indexedHoles.FindAll((x) => !x.hole.gameObject.activeSelf);
        if (holesLeft.Count == 0)
            return;

        int curIndex = Randomizer.RandomNum(holesLeft.Count);

        Debug.Log("HolesLength " + holesLeft.Count + ", Current index" + curIndex);

        photonView.RPC("NewHoleRPC", RpcTarget.All, holesLeft[curIndex].index);

        Debug.Log("calling rpc");
    }
    [PunRPC]
    public void NewHoleRPC(int index)
    {
        Debug.Log("enterRPC for new hole");
        if (!holes[index].gameObject.activeSelf)
        {
            holes[index].Init();
            myShip.CurHoleAmountActive++;
        }
    }
}
