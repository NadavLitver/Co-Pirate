using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Sounds : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private AudioClip[] sounds;
    [SerializeField]
    private AudioClip[] sounds_Interact;
    private AudioSource AudioSource;
    [SerializeField]
    private GameObject Interactions_Canvas;


    // Start is called before the first frame update
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        InputManager.controls.Gameplay.Speak.performed += Player_Speak;
        InputManager.controls.Gameplay.Q_interactions.started += InteractionsCanvas;
        InputManager.controls.Gameplay.Q_interactions.canceled += InteractionsCanvasCancle;
    }

    private void InteractionsCanvas(InputAction.CallbackContext obj)
    {
            if (!Interactions_Canvas.activeInHierarchy)
            {
                Interactions_Canvas.SetActive(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        
    }
    private void InteractionsCanvasCancle(InputAction.CallbackContext obj)
    {
        if (Interactions_Canvas.activeInHierarchy)
        {
            Interactions_Canvas.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }


    void Player_Speak(InputAction.CallbackContext context)
    {
        if (photonView.IsMine) 
        {
        photonView.RPC("PlayPlayerSounds", RpcTarget.All);
        }
    }

    [PunRPC]
    void PlayPlayerSounds()
    {
        AudioSource.PlayOneShot(sounds[Randomizer.RandomNum(sounds.Length)]);

    }

 

    public void Player_Sound_Interact(int index)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("PlaySounds", RpcTarget.All,index);
            AudioSource.volume = 0.5f + (index/10);
            Interactions_Canvas.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    [PunRPC]
    void PlaySounds(int index)
    {
        AudioSource.PlayOneShot(sounds_Interact[index]);
    }


}
