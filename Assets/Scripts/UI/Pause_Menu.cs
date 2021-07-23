using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
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
    {
        
            if (!Setting_canvas.activeInHierarchy)
            {
                Setting_canvas.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
           else
            {
                Setting_canvas.SetActive(false);
                //Cursor.visible = false;
                //Cursor.lockState = CursorLockMode.Locked;
            }

    }

    private void OnDisable()
    {
        InputManager.controls.Gameplay.ESCSettings.started -= EnableCanvas;
    }

}
