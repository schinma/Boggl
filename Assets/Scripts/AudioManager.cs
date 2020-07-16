using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip wordFound;
    public AudioClip endRound;
    public AudioClip endGame;
    public AudioClip shuffleDices;
    public AudioClip timer;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    public void PlayWordFound()
    {
        audioSource.PlayOneShot(wordFound);
    }

    public void PlayEndRound()
    {
        audioSource.PlayOneShot(endRound);
    }

    public void PlayEndGame()
    {
        audioSource.PlayOneShot(endGame);
    }

    public void PlayShuffle()
    {
        audioSource.PlayOneShot(shuffleDices);
    }

    public void PlayTimer()
    {
        audioSource.PlayOneShot(timer);
    }
}
