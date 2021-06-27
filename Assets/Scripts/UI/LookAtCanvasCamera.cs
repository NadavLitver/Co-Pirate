using UnityEngine;

public class LookAtCanvasCamera : MonoBehaviour
{
    [SerializeField]
    private Canvas _canvas;

    private void Awake()
    {
        if (_canvas == null)
            _canvas = GetComponent<Canvas>();
    }
    private void LateUpdate()
    {
        var camera = _canvas.worldCamera;
        if (camera != null)
        {
            transform.LookAt(camera.transform.position, Vector3.up);
        }
    }
}
