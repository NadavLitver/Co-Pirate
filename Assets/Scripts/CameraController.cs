using Sirenix.OdinInspector;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] ShipManager enemyShip;
    [SerializeField] Vector3 offsetFromPlayer;
    [SerializeField] float playerPosWeight = 1;
    [SerializeField] float enemyShipPosWeight = 1;
    [SerializeField, ReadOnly, HideInEditorMode] internal Transform myfollow;
    private void LateUpdate()
    {
        SetZaxis();
    }
    void SetZaxis()
    {
        //this.transform.position = new Vector3(transform.position.x, transform.position.y, myfollow.position.z);
        transform.position = myfollow.position + (myfollow.position - enemyShip.center.position).normalized * offsetFromPlayer.z + offsetFromPlayer.y * Vector3.up;
        transform.position += offsetFromPlayer.x * transform.right;

        var toPlayer = (myfollow.position - transform.position).normalized;
        var toEnemyShip = (enemyShip.center.position - transform.position).normalized;

        var lookDirection = ((toPlayer * playerPosWeight + toEnemyShip * enemyShipPosWeight) / 2).normalized;
        Debug.DrawLine(transform.position, transform.position + lookDirection * 20);
        transform.rotation = Quaternion.LookRotation(lookDirection);

    }
}
