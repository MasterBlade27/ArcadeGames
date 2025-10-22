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
    public AudioClip oneUpSFX;
    public AudioClip powerupSpawn;

    public bool volume = true;

    [SerializeField]
    private GameObject MuteIcon;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Volume"))
            PlayerPrefs.SetInt("Volume", 0);

        var BVol = PlayerPrefs.GetInt("Volume");
        if (BVol == 0)
            volume = true;
        else
            volume = false;

        if (MuteIcon != null)
            MuteIcon.SetActive(!volume);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            ToggleMute();
        }

        if (volume)
        {
            PlayerPrefs.SetInt("Volume", 0);

            foreach (AudioSource AS in audioSources)
                AS.volume = 1;

            musicSource.volume = 1;
        }

        else
        {
            PlayerPrefs.SetInt("Volume", 1);

            foreach (AudioSource AS in audioSources)
                AS.volume = 0;

            musicSource.volume = 0;
        }
    }

    public void ToggleMute()
    {
        volume = !volume;
        if (MuteIcon != null)
            MuteIcon.SetActive(!volume);
    }

    public void PlayBall(int Pitch, List<AudioClip> Sounds, int index)
    {
        if (volume)
        {
            audioSources[0].pitch = Pitch;
            audioSources[0].PlayOneShot(Sounds[index]);
        }
    }

    public void PlaySound(AudioClip Sound)
    {
        if (volume)
        {
            audioSources[1].PlayOneShot(Sound);
        }
    }

    public void PlayVol(AudioClip Sound, float Vol)
    {
        if (volume)
        {
            audioSources[2].PlayOneShot(Sound, Vol);
        }
    }

    public void PlayVol(List<AudioClip> Sounds, int index, float Vol)
    {
        if (volume)
        {
            audioSources[2].PlayOneShot(Sounds[index], Vol);
        }
    }
}
