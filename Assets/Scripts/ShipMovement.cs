using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class ShipMovement : MonoBehaviourPun
{
    #region Serielized
    [SerializeField]
    bool fasterFirst = false;
    [SerializeField]
    float diameter;
    [SerializeField]
    int lapDuration;
    [SerializeField]
    int maxDistance;
    [SerializeField, Range(-0.5f, 0.5f)]
    float lapStartOffset = 0f;
    [SerializeField, Range(0, 1)]
    private float _curveWeight;
    [SerializeField]
    private AnimationCurve _curveFirst;
    [SerializeField]
    private AnimationCurve _curveSecond;
    [SerializeField]
    private float _startDelay = 10;

    #region Events
    [SerializeField]
    private UnityEvent OnStart;
    #endregion
    #endregion
    int CenterZ => (maxDistance / 2) * (fasterFirst ? 1 : -1);
    int CenterX => (maxDistance / 2) * (fasterFirst ? -1 : 1);

    float localStartTime;
    float LocalTime => Time.time - localStartTime;
    Vector3 nextPos;
    AnimationCurve curve;
    bool _enabled = false;
    public bool Enabled
    {
        get => _enabled;
        set
        {
            if (_enabled == value)
                return;

            _enabled = value;

            SetStartPoint();
        }
    }
    private void Awake()
    {
        SetStartPoint();

        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(StartAfterDelayRoutine(_startDelay));
    }

    void FixedUpdate()
    {
        if (Enabled)
        {
            UpdatePosition();

            UpdateNextPos();

            UpdateRotation();
        }
    }
    private void SetStartPoint()
    {
        localStartTime = Time.time;

        CalculateAnimationCurve();

        transform.position = ToVector3(PositionAtTime(LocalTime), transform.position.y);

        UpdateNextPos();

        OnStart?.Invoke();
    }

    private void UpdateNextPos() => nextPos = ToVector3(PositionAtTime(LocalTime + Time.fixedDeltaTime), transform.position.y);

    private void UpdatePosition() => transform.position = nextPos;

    public Vector2 PositionAtTime(float time)
    {
        float lapProgress = Mathf.Repeat((time / lapDuration) + lapStartOffset, 1);
        lapProgress = curve.Evaluate(lapProgress);
        lapProgress *= 2 * Mathf.PI;

        float X = (diameter + CenterX) * Mathf.Cos(lapProgress);

        float Z = (diameter + CenterZ) * Mathf.Sin(lapProgress);
        return new Vector2(X, Z);
    }
    private void UpdateRotation()
    {
        Vector3 lookDir = nextPos - transform.position;

        Vector3 eulerRotation = Quaternion.LookRotation(lookDir).eulerAngles;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, eulerRotation.y, transform.rotation.eulerAngles.z);
    }
    private Vector3 ToVector3(Vector2 direction, float yValue) => new Vector3(direction.x, yValue, direction.y);
    private Vector2 ToVector2(Vector3 direction) => new Vector2(direction.x, direction.z);
    public Vector2 Speed => (ToVector2(nextPos) - ToVector2(transform.position)) / Time.fixedDeltaTime;


    private void CalculateAnimationCurve()
    {
        curve = fasterFirst ? _curveFirst : _curveSecond;
        var keyframes = new Keyframe[curve.length];
        for (int i = 0; i < curve.length; i++)
        {
            float time = curve.keys[i].time;
            float value = curve.keys[i].value;
            float inTangent = Mathf.Lerp(1, curve.keys[i].inTangent, _curveWeight);
            float outTangent = Mathf.Lerp(1, curve.keys[i].outTangent, _curveWeight);

            keyframes[i] = new Keyframe(time, value, inTangent, outTangent);
        }
        curve = new AnimationCurve(keyframes);
    }
    private IEnumerator StartAfterDelayRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        photonView.RPC("StartAfterDelayRPC", RpcTarget.All);
    }
    [PunRPC]
    private void StartAfterDelayRPC()
    {
        Enabled = true;
    }
}
