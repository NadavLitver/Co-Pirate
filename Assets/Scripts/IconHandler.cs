using CustomAttributes;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class IconHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private float _fadeoutSpeed;
    [SerializeField]
    private float _fadeinSpeed;
    [SerializeField, LocalComponent(true)]
    private Image _iconImage;

    private Tween _transitionTween;
    public void SetIcon(Sprite icon)
    {
        if (_iconImage != null)
        {
            if (icon == null)
                FadeOut();
            else
                FadeOut(0, () => SwapIcon(icon));

        }
        void SwapIcon(Sprite newIcon)
        {
            _iconImage.sprite = icon;
            FadeIn();
        }
    }

    private void FadeIn(float delay = 0, TweenCallback callback = null)
    {
        KillTween();

        if (_iconImage.color.a != 1)
            _transitionTween = _iconImage.DOFade(1, (1 - _iconImage.color.a) / _fadeinSpeed).SetDelay(delay).OnComplete(callback);
        else
            callback?.Invoke();
    }


    private void FadeOut(float delay = 0, TweenCallback callback = null)
    {
        KillTween();

        if (_iconImage.color.a != 0)
            _transitionTween = _iconImage.DOFade(0, _iconImage.color.a / _fadeoutSpeed).SetDelay(delay).OnComplete(callback);
        else
            callback?.Invoke();
    }
    private void KillTween()
    {
        if (_transitionTween != null)
        {
            _transitionTween.onComplete?.Invoke();
            _transitionTween.Kill();
        }
    }
}
