using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{

    public AudioClip[] trackList;

    [SerializeField]private AudioSource musicPlayer;
    [SerializeField] AudioSource ambientPlayer;
    [SerializeField] AudioSource sfxPlayer;


    private int currentTrack;


    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        ChangeSong(trackList[0]);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

    }

    public void StopAll()
    {
        if (musicPlayer)
            musicPlayer.Stop();

        if (ambientPlayer)
            ambientPlayer.Stop();
    }

    public void ChangeSong(AudioClip newSong)
    {
        
        musicPlayer.clip = newSong;
        musicPlayer.Play();
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        sfxPlayer.PlayOneShot(clip); 
    }


}
