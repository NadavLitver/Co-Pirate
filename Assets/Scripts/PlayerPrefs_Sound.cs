using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PlayerPrefs_Sound : MonoBehaviour
{
    public AudioMixer masterMixer;
    public Slider vulome_slider; 
    // Start is called before the first frame update
    private void Start()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            masterMixer.SetFloat("Volume", PlayerPrefs.GetFloat("Volume"));
            vulome_slider.value = PlayerPrefs.GetFloat("Volume");
        }
    }

  
}
