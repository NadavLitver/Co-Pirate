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
    private AudioClip[] sounds_Interact;
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

    public void Player_Sound_Interact(int index)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("PlaySounds", RpcTarget.All,index);
        }
    }

    [PunRPC]
    void PlaySounds(int index)
    {
        AudioSource.PlayOneShot(sounds_Interact[index]);

    }


}
