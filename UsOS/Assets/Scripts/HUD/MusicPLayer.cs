using System.Runtime.CompilerServices;
using UnityEngine;

public class Music : MonoBehaviour
{
    AudioClip[] songs;
    private AudioSource audioSource;
    public bool isShuffle;
    public uint currentSongIndex;

    public void Start()
    {
        this.songs = Resources.LoadAll<AudioClip>("Music");
        this.audioSource = Camera.main.GetComponent<AudioSource>();
        this.audioSource.clip = this.songs[this.currentSongIndex];
        PauseOrPlay();
    }

    public void PlayNext()
    {
        if (this.isShuffle)
        {
            this.currentSongIndex = (uint)Random.Range(0, this.songs.Length);
        }
        else
        {
            this.currentSongIndex += 1;
        }

        this.audioSource.clip = this.songs[this.currentSongIndex];
        this.audioSource.Play();
    }

    public void PauseOrPlay()
    {
        if (this.audioSource.isPlaying)
        {
            this.audioSource.Pause();
        }
        else
        {
            this.audioSource.Play();
        }
    }

    public void SwitchShuffle()
    {
        this.isShuffle = !this.isShuffle;
    }
}