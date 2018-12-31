using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//changes background music and plays various interaction sounds

public enum SoundClip {
   rewardCoin, rewardWeapon, enemyDeath, gateOpen, victory
};
public class AudioController : MonoBehaviour {
   
    public static AudioController instance;

    //Background Music elements
    public AudioSource BGMusic;
    public AudioSource soundSource;
    public AudioSource enemySoundSource;
    public AudioClip[] BGSongs;
    private float songLength;
    private AudioClip previousClip;
    private float currentSongTime;
    public AudioClip rewardCoin;
    public AudioClip rewardWeapon;
    public AudioClip enemyDeath;
    public AudioClip gateOpen;
    public AudioClip victory;
    private void Start () {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
        ChangeBGMusic();
        soundSource = GetComponent<AudioSource>();
        enemySoundSource = transform.GetChild(1).GetComponent<AudioSource>();
        
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

    public void PlaySound(SoundClip soundClip) {
        if (!soundSource.isPlaying) {
            switch(soundClip) {
                case SoundClip.rewardCoin:
                    soundSource.clip = rewardCoin;
                    soundSource.Play();
                    break;
                case SoundClip.rewardWeapon:
                    soundSource.clip = rewardWeapon;
                    soundSource.Play();
                    break;
                case SoundClip.enemyDeath:
                    enemySoundSource.clip = enemyDeath;
                    enemySoundSource.Play();
                    break;
                case SoundClip.gateOpen:
                    soundSource.clip = gateOpen;
                    soundSource.Play();
                    break;
                case SoundClip.victory:
                    enemySoundSource.clip = victory;
                    enemySoundSource.Play();
                    break;
                default:
                    break;
            }
        }
            
    }
  
}

