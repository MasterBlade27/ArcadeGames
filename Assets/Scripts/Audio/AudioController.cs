using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    /* 0 = Levels + Power Ups
     * 1 = Player + Death
     * 2 = Enemy #1 + Block
     * 3 = Enemy #2 + Wall
     * 4 = Enemy #3
     * 5 = Enemy #4
     * 6 = Enemy #5 + Mushrooms
    */
    public List<AudioSource> SFS = new List<AudioSource>();
    public AudioSource MS;

    [Header("Breakout")]
    public List<AudioClip> paddleSounds = new List<AudioClip>();
    public List<AudioClip> blockSounds = new List<AudioClip>();
    public List<AudioClip> wallSounds = new List<AudioClip>();

    [Header("Centipede")]
    public List<AudioClip> playerSounds = new List<AudioClip>();
    public List<AudioClip> centipedeSounds = new List<AudioClip>();
    public List<AudioClip> spiderSounds = new List<AudioClip>();
    public List<AudioClip> scorpionSounds = new List<AudioClip>();
    public List<AudioClip> tickSounds = new List<AudioClip>();
    public AudioClip centipedeHit;
    public AudioClip spiderHit;
    public AudioClip scorpionHit;
    public AudioClip tickHit;
    public AudioClip mushroom;
    public List<AudioClip> scoreMarch = new List<AudioClip>();

    [Header("Power Ups")]
    public AudioClip powerupSpawn;
    public List<AudioClip> powerupSounds = new List<AudioClip>();

    [Header("Levels")]
    public AudioClip death;
    public AudioClip gameOver;
    public AudioClip nextLevel;

    [Header("Mute Button")]
    [SerializeField]
    private bool sfx = true;
    [SerializeField]
    private bool music = true;

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
            ToggleSFX();
        
        if (Input.GetKeyUp(KeyCode.N))
            ToggleMusic();

        if (sfx)
        {
            PlayerPrefs.SetInt("Volume", 0);

            foreach (AudioSource AS in SFS)
                AS.volume = 1;
        }

        else
        {
            PlayerPrefs.SetInt("Volume", 1);

            foreach (AudioSource AS in SFS)
                AS.volume = 0;
        }

        if (music)
        {
            PlayerPrefs.SetInt("Music", 0);

            MS.volume = 1;
        }

        else
        {
            PlayerPrefs.SetInt("Music", 1);

            MS.volume = 0;
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

    public void PlayNormal(AudioClip Sound, int ASN)
    {
        if (sfx)
        {
            SFS[ASN].PlayOneShot(Sound);
        }
    }

    public void PlayRandom(List<AudioClip> Sounds, int index, int ASN)
    {
        if (sfx)
        {
            SFS[ASN].PlayOneShot(Sounds[index]);
        }
    }

    public void PlayVol(float Vol, AudioClip Sound, int ASN)
    {
        if (sfx)
        {
            SFS[ASN].PlayOneShot(Sound, Vol);
        }
    }

    public void PlayVol(float Vol, List<AudioClip> Sounds, int index, int ASN)
    {
        if (sfx)
        {
            SFS[ASN].PlayOneShot(Sounds[index], Vol);
        }
    }

    public void PlayPitch(int Pitch, AudioClip Sound, int ASN)
    {
        if (sfx)
        {
            SFS[ASN].pitch = Pitch;
            SFS[ASN].PlayOneShot(Sound);
        }
    }

    public void PlayPitch(int Pitch, List<AudioClip> Sounds, int index, int ASN)
    {
        if (sfx)
        {
            SFS[ASN].pitch = Pitch;
            SFS[ASN].PlayOneShot(Sounds[index]);
        }
    }

    public void StopVol()
    {
        foreach (AudioSource Sound in SFS)
            Sound.Stop();
    }
}
