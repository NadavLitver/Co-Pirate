using Photon.Pun;
using UnityEngine;

public class LookAtCanvasCamera : MonoBehaviour
{
    [SerializeField]
    private bool _assignCameraToMain = false;
    [SerializeField]
    private Canvas _canvas;

    private void Awake()
    {
       
        
        if (_canvas == null)
            _canvas = GetComponent<Canvas>();

        if (_assignCameraToMain)
            _canvas.worldCamera = Camera.main;
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
