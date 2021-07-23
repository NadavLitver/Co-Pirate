using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CannonInteraction : BaseInteractable
{
    [SerializeField]
    private UnityEvent OnInteraction;

    private Pushback _pushBack;
    private bool _onCooldown = false;
    public override bool InteractableCondition(PlayerController ctrl) => ctrl != null && ctrl.HoldingCannonBall && !_onCooldown;
    private void Awake()
    {
        TryGetComponent(out _pushBack);
    }
    public override void OnInteract_Start(PlayerController ctrl)
    {
        if (ctrl.DoubleShootBuff)
            ctrl.DoubleShootBuff = false;
        else
            ctrl.UsedCannonball();

        if (_pushBack != null)
            StartCoroutine(CooldownRoutine());

        OnInteraction?.Invoke();
        Debug.Log("Interacted With Cannon");
    }

    private IEnumerator CooldownRoutine()
    {
        _onCooldown = true;
        yield return new WaitForSeconds(_pushBack.Duration);
        _onCooldown = false;
    }
}


