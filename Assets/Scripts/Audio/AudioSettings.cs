using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;



public class AudioSettings : MonoBehaviour
{

    [SerializeField] AudioMixer masterMixer;

    public void SetMasterVolume(float volume){
        masterMixer.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume){
        masterMixer.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume){
        masterMixer.SetFloat("SFXVolume", volume);
    }

    public void SetAmbientVolume(float volume)
    {
        masterMixer.SetFloat("AmbientVolume", volume);
    }
}
