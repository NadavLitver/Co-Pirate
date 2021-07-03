using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class HoleController : MonoBehaviour
{
    [SerializeField]
    private float _randomPosRange;

    [SerializeField, FoldoutGroup("Events")]
    private UnityEvent OnInit;
    [SerializeField, FoldoutGroup("Events")]
    private UnityEvent OnFix;

    public HolesManager manager { get; set; }
    private Vector3 _startPos;
    


    private void OnEnable()
    {
        _startPos = transform.position;
    }
    public void Init()
    {
        gameObject.SetActive(true);
        transform.position = _startPos + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * _randomPosRange * Random.Range(0f, 1f);

        OnInit?.Invoke();
    }
    public void Fix()
    {
        manager.FixedHole(this);
    }
    public void FixRPC()
    {
        OnFix?.Invoke();
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _randomPosRange);
    }
#endif
}
