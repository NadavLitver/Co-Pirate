using CustomAttributes;
using Sirenix.OdinInspector;
using System;
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
    public event Action<float> OnFixProgress;
    private float _fixStartTime = Mathf.Infinity;
    private bool _interacting;
    private ShipManager myShip;

    public override event Action InteractFinished;

    private void Awake()
    {
        myShip = GetComponentInParent<ShipManager>();
        if (_holeCtrl == null)
            _holeCtrl = GetComponent<HoleController>();
    }
    private void Update()
    {
        if (_interacting)
        {
            float progress = (Time.time - _fixStartTime) / _fixTime;
            OnFixProgress?.Invoke(progress);

            if (progress >= 1)
                Fixed();
        }
    }
    public override bool InteractableCondition(PlayerController ctrl) => ctrl != null && !ctrl.HoldingCannonBall;

    public override void OnInteract_End(PlayerController ctrl)
    {
        base.OnInteract_End(ctrl);
        if (!_interacting)
            return;


        _interacting = false;
    }

    private void Fixed()
    {
        _holeCtrl.Fix();
        myShip.CurHoleAmountActive--;
        InteractFinished?.Invoke();
        OnFixed?.Invoke();

        Destroy(gameObject);
    }

    public override void OnInteract_Start(PlayerController ctrl)
    {
        _interacting = true;
        _fixStartTime = Time.time;
    }
}
