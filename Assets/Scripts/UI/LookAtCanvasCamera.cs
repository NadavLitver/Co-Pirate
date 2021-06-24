using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCanvasCamera : MonoBehaviour
{
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
    }
    private void FixedUpdate()
    {
        var camera = _canvas.worldCamera;
        if(camera != null)
        {
            transform.LookAt(_canvas.worldCamera.transform.position, Vector3.up);
        }
    }
}
