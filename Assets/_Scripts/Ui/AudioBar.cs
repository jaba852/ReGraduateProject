using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioBar : MonoBehaviour
{   public AudioMixer MusicMixer;
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    public void Awake()
    {
      //masterSlider.value = PlayerPrefs.GetFloat("Master",0.75f);
     
        
    }
    public void SetMasterVolume(float volume)
    {  

       MusicMixer.SetFloat("Master",Mathf.Log10(masterSlider.value)*20);
       
    }

    public void SetBGMVolume(float volume)
    {
      MusicMixer.SetFloat("BGM",Mathf.Log10(bgmSlider.value)*20);
    }

    public void SetSFXVolume(float volume)
    {
       MusicMixer.SetFloat("SFX",Mathf.Log10(sfxSlider.value)*20);
    }


    
}
