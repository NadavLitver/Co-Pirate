using DG.Tweening;
using UnityEngine;

public class Pushback : MonoBehaviour
{
    [SerializeField] private float amount;
    [SerializeField] private float duration;
    [SerializeField] private AnimationCurve curve;

    public float Duration => duration;
    Tween tween;
    public void Play()
    {
        if (tween == null && tween.IsActive())
        {
            if (tween.IsComplete())
                tween.Restart();
            else
                tween.PlayForward();
        }
        else
            tween = transform.DOLocalMove(transform.localPosition - transform.root.InverseTransformDirection(transform.forward * amount), duration).SetEase(curve).SetAutoKill(false);
    }
}
