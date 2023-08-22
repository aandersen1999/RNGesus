using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] private AudioClip[] musicTracks;
    [SerializeField] private AudioSource audioSource;
    private bool musicPaused;

    public AudioClip playerMoveSound;
    public AudioClip playerDeathSound;
    public AudioClip playerWinSound;

    protected new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        
    }

    private void Start()
    {
        LoadNewSong(0);
    }

    private void Update()
    {
        
    }

    public void PlayMusic(AudioClip song)
    {
        if (audioSource.clip != song && !musicPaused)
        {
            audioSource.clip = song;
            audioSource.Stop();
            audioSource.Play();
        }
    }

    public void PauseMusic()
    {
        musicPaused = true;
        audioSource.Pause();
    }

    public void UnpauseMusic()
    {
        musicPaused = false;
        audioSource.UnPause();
    }

    public void PlaySound(AudioClip sound, float volume)
    {
        audioSource.PlayOneShot(sound, volume);
    }

    public void LoadNewSong(int id)
    {
        /*switch (level)
        {
            case "TitleScreen":
            case "Grass1":
            case "Grass 2":
            case "Grass 3":
            case "Grass 4":
                PlayMusic(musicTracks[0]);
                break;
            case "Desert 1":
            case "Desert 2":
            case "Desert 3":
            case "Desert 4":
                PlayMusic(musicTracks[1]);
                break;
            case "Snow 1":
            case "Snow 2":
            case "Snow 3":
            case "Snow 4":
                PlayMusic(musicTracks[2]);
                break;
            case "Lava 1":
            case "Lava 2":
            case "Lava 3":
            case "Lava 4":
                PlayMusic(musicTracks[3]);
                break;
        }
        */
        if(id < 7)
            PlayMusic(musicTracks[0]);
        else if(id < 12)
            PlayMusic(musicTracks[1]);
        else if(id < 16)
            PlayMusic(musicTracks[2]);
        else
        {
            PlayMusic(musicTracks[3]);
        }
    }
}
