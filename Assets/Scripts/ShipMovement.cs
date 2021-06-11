using Sirenix.OdinInspector;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    Vector3 center;

    [SerializeField, DisableInPlayMode]
    float cycleDuration = 60;

    [SerializeField]
    float speed = 1;
    [SerializeField]
    Range speedRangeX;
    [SerializeField]
    Range speedRangeZ;

    Vector3 lastPos;
    Vector3 shipSpeed;
    private void Start()
    {
        center = transform.position;
        lastPos = center - transform.forward;
        //circumference = 2 * Mathf.PI * Mathf.Sqrt((Mathf.Pow(radius.x, 2) + Mathf.Pow(radius.y, 2)) / 2);
    }
    private void Update()
    {

        float cosX = Mathf.Sin((Time.time * (Mathf.PI * 2)) / cycleDuration);
        float sinZ = Mathf.Cos((Time.time * (Mathf.PI * 2)) / cycleDuration);

        float speedX = Mathf.Lerp(speedRangeX.minScalar, speedRangeX.maxScalar, (sinZ + 1) / 2) * speed;
        float speedZ = Mathf.Lerp(speedRangeZ.minScalar, speedRangeZ.maxScalar, (cosX + 1) / 2) * speed;

        shipSpeed = new Vector3(cosX * speedX, 0, sinZ * speedZ);
        transform.position += shipSpeed * Time.deltaTime;
        Vector3 lookDir = GetLookDir();
        
        Vector3 eulerRotation = Quaternion.LookRotation(lookDir).eulerAngles;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, eulerRotation.y, transform.rotation.eulerAngles.z);
        lastPos = transform.position;
    }

    private Vector3 GetLookDir()
    {
        return transform.position - new Vector3(lastPos.x, transform.position.y, lastPos.z);
    }

    [System.Serializable, InlineProperty]
    class Range
    {
        public float minScalar = 1;
        public float maxScalar = 1;
    }
}
