using CustomAttributes;
using DG.Tweening;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class IconHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private float _offsetY;
    [SerializeField]
    private float _fadeoutSpeed;
    [SerializeField]
    private float _fadeinSpeed;
    [SerializeField, LocalComponent(true)]
    private Image _iconImage;

    private Camera _mainCam;
    private void Start()
    {
        _mainCam = GameManager.Instance.localPlayerObject.GetComponent<PlayerController>().personalCamera;

        GetComponentInParent<Canvas>().worldCamera = _mainCam;

        if (_player != null)
            SetIconPosition();
    }
    private void LateUpdate()
    {
        SetIconPosition();
    }

    private void SetIconPosition()
    {
        transform.position = _player.transform.position + Vector3.up * _offsetY;
        transform.LookAt(_mainCam.transform.position, Vector3.up);
    }
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
        if (_iconImage.color.a != 1)
            _iconImage.DOFade(1, (1 - _iconImage.color.a) / _fadeinSpeed).SetDelay(delay).OnComplete(callback);
        else
            callback?.Invoke();
    }

    private void FadeOut(float delay = 0, TweenCallback callback = null)
    {
        if (_iconImage.color.a != 0)
            _iconImage.DOFade(0, _iconImage.color.a / _fadeoutSpeed).SetDelay(delay).OnComplete(callback);
        else
            callback?.Invoke();
    }
}
