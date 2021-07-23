using UnityEngine;
using UnityEngine.InputSystem;

public class Pause_Menu : MonoBehaviour
{
    [SerializeField]
    GameObject Setting_canvas;

    // Start is called before the first frame update
    void Start()
    {
        InputManager.controls.Gameplay.ESCSettings.started += EnableCanvas;
    }
    void EnableCanvas(InputAction.CallbackContext context)
                => Setting_canvas.SetActive(!Setting_canvas.activeInHierarchy);

    private void OnDisable()
    {
        InputManager.controls.Gameplay.ESCSettings.started -= EnableCanvas;
    }

}
