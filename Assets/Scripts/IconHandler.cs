using CustomAttributes;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class IconHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private float _offsetY;
    [SerializeField, LocalComponent(true)]
    private Image _iconImage;

    private Camera _mainCam;
    private void Awake()
    {
        _mainCam = Camera.main;
    }
    private void Start()
    {
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
                _iconImage.enabled = false;
            else
            {
                _iconImage.enabled = true;
                _iconImage.sprite = icon;
            }

        }
    }
}
