using Photon.Pun;
using UnityEngine;
[SelectionBase]
public class HoleHandler : MonoBehaviourPun
{
    public GameObject[] holes;
    public ShipManager myShip;


    public void NewHole()
    {
        photonView.RPC("NewHoleRPC", RpcTarget.All);
        Debug.Log("calling rpc");

    }
    [PunRPC]
    public void NewHoleRPC()
    {
        Debug.Log("enterRPC for new hole");

        int curIndex = Randomizer.RandomNum(holes.Length);
        //holes[curIndex].transform.position = new Vector3(holes[curIndex].transform.position.x + Randomizer.NormalizedFloat(), holes[curIndex].transform.position.y, holes[curIndex].transform.position.z + Randomizer.NormalizedFloat());
        holes[curIndex].SetActive(true);
        myShip.CurHoleAmountActive++;


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
