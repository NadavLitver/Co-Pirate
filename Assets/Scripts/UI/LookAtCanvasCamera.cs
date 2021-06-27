using UnityEngine;

public class LookAtCanvasCamera : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;

    private void Awake()
    {
        if (_canvas == null)
            _canvas = GetComponentInParent<Canvas>();
    }
    private void FixedUpdate()
    {
        var camera = _canvas.worldCamera;
        if (camera != null)
        {
            transform.LookAt(_canvas.worldCamera.transform.position, Vector3.up);
        }
    }
}
