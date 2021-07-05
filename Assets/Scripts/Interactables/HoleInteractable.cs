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

    #endregion
    public event Action<float> OnFixProgress;
    private float _fixStartTime = Mathf.Infinity;
    private bool _interacting;

    public bool Interacting
    {
        get => _interacting;
        set
        {
            if (_interacting == value)
                return;

            _interacting = value;

            if (_interacting)
                _fixStartTime = Time.time;
            else
                _fixStartTime = Mathf.Infinity;
        }
    }

    public override event Action<IInteractable> InteractFinished;

    private void Awake()
    {
        if (_holeCtrl == null)
            _holeCtrl = GetComponent<HoleController>();
    }
    private void Update()
    {
        if (Interacting)
        {
            float progress = Mathf.Max(0, (Time.time - _fixStartTime) / _fixTime);
            OnFixProgress?.Invoke(progress);

            if (progress >= 1)
                Fixed();
        }
    }
    public override bool InteractableCondition(PlayerController ctrl) => ctrl != null && !ctrl.HoldingCannonBall;

    public override void OnInteract_End(PlayerController ctrl)
    {
        base.OnInteract_End(ctrl);
        if (!Interacting)
            return;


        Interacting = false;
    }

    private void Fixed()
    {
        _holeCtrl.Fix();
        Interacting = false;
        InteractFinished?.Invoke(this);
    }

    public override void OnInteract_Start(PlayerController ctrl)
    {
        if (ctrl.InstantFixBuff)
        {
            Fixed();
            ctrl.InstantFixBuff = false;
            return;
        }
        Interacting = true;
    }
}
