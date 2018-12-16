using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

    public static AudioController instance;

    //Background Music elements
    public AudioSource BGMusic;
    public AudioClip[] BGSongs;
    private float songLength;
    private AudioClip previousClip;
    private float currentSongTime;

	private void Start () {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
        ChangeBGMusic();

    }

	private void Update () {
        currentSongTime += Time.deltaTime;
        if(currentSongTime >= songLength) {
            currentSongTime = 0;
            ChangeBGMusic();
        }
	}

    private void ChangeBGMusic() {
        BGMusic.clip = BGSongs[Random.Range(0, BGSongs.Length)];
        //making sure one song doesn't loop again after playing right away
        if (previousClip != null && previousClip == BGMusic.clip) {
            while(previousClip == BGMusic.clip) {
                BGMusic.clip = BGSongs[Random.Range(0, BGSongs.Length)];
            }
        }
        previousClip = BGMusic.clip;
        BGMusic.Play();
        songLength = BGMusic.clip.length;

    }
}
