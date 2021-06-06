using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class HoleInteractable : BaseInteractable
{
    [SerializeField]
    private float _fixTime;

    #region Events
    [SerializeField, FoldoutGroup("Events", order: 99)]
    private UnityEvent OnStartInteraction;
    [SerializeField, FoldoutGroup("Events", order: 99)]
    private UnityEvent OnEndInteraction;
    [SerializeField, FoldoutGroup("Events", order: 99)]
    private UnityEvent OnFixed;
    #endregion

    private float _fixProgress;

    private float _fixStartTime = Mathf.Infinity;
    private bool _interacting;
    private ShipManager myShip;
    private void Awake()
    {
        myShip = GetComponentInParent<ShipManager>();
    }
    public override bool InteractableCondition(PlayerController ctrl) => ctrl != null && !ctrl.HoldingCannonBall;

    public override void OnInteract_End(PlayerController ctrl)
    {
        if (!_interacting)
            return;

        base.OnInteract_End(ctrl);
        _fixProgress -= Time.time - _fixStartTime;
        if (_fixProgress <= 0)
            Fixed();

        _interacting = false;
    }

    private void Fixed()
    {
        OnFixed?.Invoke();
        gameObject.SetActive(false);
        myShip.CurHoleAmountActive--;
    }

    public override void OnInteract_Start(PlayerController ctrl)
    {
        _interacting = true;
        _fixStartTime = Time.time;
    }
}
