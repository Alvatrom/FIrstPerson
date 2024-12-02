using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Music Sounds")]
    [SerializeField] private Sound[] musicSounds;
    [Header("SFX Sounds")]
    [SerializeField] private Sound[] sfxSounds;


    [SerializeField] private AudioSource musicSource, audioSourceSfx;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += FiltroEscena; //cuando cargue una escena, dependiendo de que escena sea reproduce una cancion o otra 
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void ReproducirSonido(AudioClip clip)
    {
        //Reproducir una vez el sonido
        audioSourceSfx.PlayOneShot(clip);
    }

    private void Start()
    {
        //PlayMusic("PlayMode");
    }

    private void FiltroEscena(Scene scene, LoadSceneMode mode)
    {
        //Reproducir la música correspondiente a la escena cargada
        if (scene.name == "Titulo")
        {
            PlayMusic("MenuPrincipal");
        }
        else if (scene.name == "Game")
        {
            PlayMusic("PlayMode");
        }
        else if (scene.name == "Final")
        {
            PlayMusic("MenuFinal");
        }
    }

    public void PlayMusic(string name)//busca por nombre en el array creado los siguientes sonidos
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Musica no encontrada");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }
    public void PlaySFX(string name)//busca por nombre en el array creado los siguientes sonidos
    {
        //preguntar esto
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Musica no encontrada");
        }
        else
        {
            audioSourceSfx.PlayOneShot(s.clip);
        }
    }
    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleSFX()
    {
        audioSourceSfx.mute = !audioSourceSfx.mute;
    }
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SFXVolume(float volume)
    {
        audioSourceSfx.volume = volume;
    }

}
