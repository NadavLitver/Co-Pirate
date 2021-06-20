using Photon.Pun;
using UnityEngine;

[SelectionBase]
public class ShipMovement : MonoBehaviour
{
private enum axis { X, Z }
    [SerializeField]
    float diameter;
    [SerializeField]
    float speedRatio;
    [SerializeField]
    axis fasterAxis;

    [SerializeField]
    int lapDuration;
    [SerializeField]
    int maxDistance;

    const int centerX = 0;
    int CenterZ => (maxDistance/2) * (fasterAxis == axis.Z ? -1 : 1);

    private Vector3 lastPos;

    float xAxisDiameter => fasterAxis == axis.X ? diameter * speedRatio : diameter / speedRatio;
    float zAxisDiameter => fasterAxis == axis.Z ? diameter * speedRatio : diameter / speedRatio;

    float time => startTime + Time.time;
    float startTime;
    private void Awake()
    {
        startTime = (float)PhotonNetwork.Time;
    }
    void Update()
    {
        UpdatePosition();

        UpdateRotation();
    }

    private void UpdatePosition()
    {
        Vector2 position = PositionAtTime(time);
        transform.position = new Vector3(position.x, transform.position.y, position.y);
    }

    public Vector2 PositionAtTime(float time)
    {
        float lapProgress = (time / lapDuration) * 2 * Mathf.PI;
        float X = centerX + (xAxisDiameter * Mathf.Cos(lapProgress));
        X *= diameter / xAxisDiameter;
        float Z = CenterZ + (zAxisDiameter * Mathf.Sin(lapProgress));
        Z *= diameter / zAxisDiameter;
        return new Vector2(X, Z);
    }
    private void UpdateRotation()
    {
        Vector3 lookDir = GetLookDir();

        Vector3 eulerRotation = Quaternion.LookRotation(lookDir).eulerAngles;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, eulerRotation.y, transform.rotation.eulerAngles.z);
        lastPos = transform.position;
        Vector2 position = PositionAtTime(Time.time);
        transform.position = new Vector3(position.x, transform.position.y, position.y);
    }

    private Vector3 GetLookDir() => transform.position - new Vector3(lastPos.x, transform.position.y, lastPos.z);
}
