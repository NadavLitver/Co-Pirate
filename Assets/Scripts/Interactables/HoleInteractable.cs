using CustomAttributes;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class HoleInteractable : BaseInteractable
{
    [SerializeField, LocalComponent]
    private HoleController _holeCtrl;
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

    private float _fixStartTime = Mathf.Infinity;
    private bool _interacting;
    private ShipManager myShip;
    private void Awake()
    {
        myShip = GetComponentInParent<ShipManager>();
        if (_holeCtrl == null)
            _holeCtrl = GetComponent<HoleController>();
    }
    private void Update()
    {
        if (_interacting && Time.time - _fixStartTime >= _fixTime)
            Fixed();
    }
    public override bool InteractableCondition(PlayerController ctrl) => ctrl != null && !ctrl.HoldingCannonBall;

    public override void OnInteract_End(PlayerController ctrl)
    {
        if (!_interacting)
            return;

        base.OnInteract_End(ctrl);

        _interacting = false;
    }

    private void Fixed()
    {
        OnFixed?.Invoke();
        _holeCtrl.Fix();
        myShip.CurHoleAmountActive--;
    }

    public override void OnInteract_Start(PlayerController ctrl)
    {
        _interacting = true;
        _fixStartTime = Time.time;
    }
}
