using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    public AudioSource audioSource1;
    [SerializeField]
    public AudioSource audioSource2;
    [SerializeField]
    public List<AudioSource> musicSources = new List<AudioSource>();
    [SerializeField]
    public List<AudioClip> paddleAudioClips = new List<AudioClip>();
    [SerializeField]
    public List<AudioClip> blockAudioClips = new List<AudioClip>();
    [SerializeField]
    public List<AudioClip> wallAudioClips = new List<AudioClip>();
    [SerializeField]
    public AudioClip powerupSlowTime;
    [SerializeField]
    public AudioClip powerupLargePaddle;
    [SerializeField]
    public AudioClip floorSFX;
    [SerializeField]
    public AudioClip gameOverSFX;
    [SerializeField]
    public AudioClip powerupSpawnSFX;
    [SerializeField]
    public AudioClip oneUpSFX;
}
