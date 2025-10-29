using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public List<AudioSource> audioSources = new List<AudioSource>();
    public AudioSource musicSource; 

    public List<AudioClip> paddleAudioClips = new List<AudioClip>();
    public List<AudioClip> blockAudioClips = new List<AudioClip>();
    public List<AudioClip> wallAudioClips = new List<AudioClip>();
    public List<AudioClip> powerupClips = new List<AudioClip>();

    public AudioClip floorSFX;
    public AudioClip gameOverSFX;
    public AudioClip nextLevelSFX;
    public AudioClip powerupSpawn;

    [SerializeField]
    private bool sfx = true, music = true;

    [SerializeField]
    private GameObject SFXIcon, MusicIcon;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Volume"))
            PlayerPrefs.SetInt("Volume", 0);

        if (!PlayerPrefs.HasKey("Music"))
            PlayerPrefs.SetInt("Music", 0);

        var BVol = PlayerPrefs.GetInt("Volume");
        if (BVol == 0)
            sfx = true;
        else
            sfx = false;

        BVol = PlayerPrefs.GetInt("Music");
        if (BVol == 0)
            music = true;
        else
            music = false;

        if (SFXIcon != null)
            SFXIcon.SetActive(!sfx);

        if (MusicIcon != null)
            MusicIcon.SetActive(!music);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            ToggleSFX();
        }
        
        if (Input.GetKeyUp(KeyCode.N))
        {
            ToggleMusic();
        }

        if (sfx)
        {
            PlayerPrefs.SetInt("Volume", 0);

            foreach (AudioSource AS in audioSources)
                AS.volume = 1;
        }

        else
        {
            PlayerPrefs.SetInt("Volume", 1);

            foreach (AudioSource AS in audioSources)
                AS.volume = 0;
        }

        if (music)
        {
            PlayerPrefs.SetInt("Music", 0);

            musicSource.volume = 1;
        }

        else
        {
            PlayerPrefs.SetInt("Music", 1);

            musicSource.volume = 0;
        }
    }

    public void ToggleSFX()
    {
        sfx = !sfx;
        if (SFXIcon != null)
            SFXIcon.SetActive(!sfx);
    }

    public void ToggleMusic()
    {
        music = !music;
        if (MusicIcon != null)
            MusicIcon.SetActive(!music);
    }

    public void PlayBall(int Pitch, List<AudioClip> Sounds, int index)
    {
        if (sfx)
        {
            audioSources[0].pitch = Pitch;
            audioSources[0].PlayOneShot(Sounds[index]);
        }
    }

    public void PlaySound(AudioClip Sound)
    {
        if (sfx)
        {
            audioSources[1].PlayOneShot(Sound);
        }
    }

    public void PlayVol(AudioClip Sound, float Vol)
    {
        if (sfx)
        {
            audioSources[2].PlayOneShot(Sound, Vol);
        }
    }

    public void PlayVol(List<AudioClip> Sounds, int index, float Vol)
    {
        if (sfx)
        {
            audioSources[2].PlayOneShot(Sounds[index], Vol);
            Debug.Log("index");
        }
    }
}
