using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class Settings_UI : MonoBehaviour
{
    public AudioMixer masterMixer;
    private float _soundLevel;

   

    public void SetSound(float soundLevel)
    {
        masterMixer.SetFloat("Volume", soundLevel);
        _soundLevel = soundLevel;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("Volume", _soundLevel);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
