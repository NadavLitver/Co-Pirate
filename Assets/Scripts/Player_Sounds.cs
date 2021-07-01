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
    private AudioSource AudioSource;


    // Start is called before the first frame update
    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        InputManager.controls.Gameplay.Speak.performed += Player_Speak;
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

}
