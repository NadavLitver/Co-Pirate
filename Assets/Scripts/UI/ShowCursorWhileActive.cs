using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[HideMonoScript]
public class ShowCursorWhileActive : MonoBehaviour
{
    private ShowCursorToken _showCursorToken;
    private void OnEnable()
    {
        _showCursorToken = InputManager.ShowCursor();
    }
    private void OnDisable()
    {
        _showCursorToken?.Release();
    }
}
