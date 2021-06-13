using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [SerializeField, DisableInPlayMode]
    float cycleDuration = 60;

    [SerializeField]
    float speed = 1;
    [SerializeField]
    Range speedRangeX;
    [SerializeField]
    Range speedRangeZ;

    Vector3 lastPos;
    [HideInInspector]
    public Vector3 shipSpeed;

    [SerializeField, ReadOnly]
    private List<Transform> subscibers = new List<Transform>();
    private void Start()
    {
        lastPos = transform.position - transform.forward;
        //circumference = 2 * Mathf.PI * Mathf.Sqrt((Mathf.Pow(radius.x, 2) + Mathf.Pow(radius.y, 2)) / 2);
    }
    private void Update()
    {
        UpdatePosition();

        UpdateRotation();
    }

    private void UpdatePosition()
    {
        float cosX = Mathf.Sin((Time.time * (Mathf.PI * 2)) / cycleDuration);
        float sinZ = Mathf.Cos((Time.time * (Mathf.PI * 2)) / cycleDuration);

        float speedX = Mathf.Lerp(speedRangeX.minScalar, speedRangeX.maxScalar, (sinZ + 1) / 2) * speed;
        float speedZ = Mathf.Lerp(speedRangeZ.minScalar, speedRangeZ.maxScalar, (cosX + 1) / 2) * speed;

        shipSpeed = new Vector3(cosX * speedX, 0, sinZ * speedZ);
        Move(shipSpeed * Time.deltaTime);
    }

    private void Move(Vector3 movement)
    {
        transform.position += movement;
        subscibers.ForEach((x) => { if (x != null) x.position += movement; });
    }

    private void UpdateRotation()
    {
        Vector3 lookDir = GetLookDir();

        Vector3 eulerRotation = Quaternion.LookRotation(lookDir).eulerAngles;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, eulerRotation.y, transform.rotation.eulerAngles.z);
        lastPos = transform.position;
    }

    private Vector3 GetLookDir() => transform.position - new Vector3(lastPos.x, transform.position.y, lastPos.z);

    [System.Serializable, InlineProperty]
    class Range
    {
        public float minScalar = 1;
        public float maxScalar = 1;
    }
    public void Subscribe(Transform transform) => subscibers.Add(transform);
    public void Unsubscribe(Transform transform) => subscibers.Remove(transform);
}
