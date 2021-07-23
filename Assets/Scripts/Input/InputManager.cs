using System;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    public static Controls controls = new Controls();

    private static List<ShowCursorToken> _showCursorTokens = new List<ShowCursorToken>();

    static InputManager()
    {
        HideCursor();
    }
    public static ShowCursorToken ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        return CreateNewToken();
    }

    private static ShowCursorToken CreateNewToken()
    {
        ShowCursorToken token = new ShowCursorToken();
        token.OnRelease += TokenReleased;
        _showCursorTokens.Add(token);
        return token;

        void TokenReleased(ShowCursorToken token)
        {
            _showCursorTokens.Remove(token);
            if (_showCursorTokens.Count == 0)
                HideCursor();
        }
    }

    private static void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
public class ShowCursorToken
{
    public bool _released = false;
    public event Action<ShowCursorToken> OnRelease;
    public void Release()
    {
        if (_released)
            return;

        _released = true;

        OnRelease?.Invoke(this);
    }
}