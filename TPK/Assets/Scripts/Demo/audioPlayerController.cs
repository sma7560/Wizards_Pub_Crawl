using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioPlayerController : MonoBehaviour {

    public AudioClip[] music;
    public AudioClip[] fx;
    public AudioSource soundPlayer;

    private bool isPlayingMusic;
    private int muiscIndex;
    private int fxIndex;
    // Use this for initialization
    void Start () {
        muiscIndex = 0;
        fxIndex = 0;
        isPlayingMusic = true;
        soundPlayer = gameObject.GetComponent<AudioSource>();
        soundPlayer.clip = music[muiscIndex];
        soundPlayer.Play();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ToggleMusic() {
        isPlayingMusic = !isPlayingMusic;
        if (isPlayingMusic)
        {
            if (muiscIndex >= music.Length || muiscIndex < 0) {
                muiscIndex = 0;
                return;
            }
          
            soundPlayer.clip = music[muiscIndex];
            soundPlayer.Play();
        }
        else {
            if (fxIndex >= fx.Length || fxIndex < 0)
            {
                fxIndex = 0;
                return;
            }
            soundPlayer.clip = fx[fxIndex];
            soundPlayer.Play();
        }
    }

    public void nextClip() {
        if (isPlayingMusic)
        {
            muiscIndex++;
            if (muiscIndex >= music.Length) {
                muiscIndex--;
                return;
            }
            
            soundPlayer.clip = music[muiscIndex];
            soundPlayer.Play();
        }
        else {
            fxIndex++;
            if (fxIndex >= fx.Length) {
                fxIndex--;
                return;
            }
            
            soundPlayer.clip = fx[fxIndex];
            soundPlayer.Play();
        }
    }
    public void previousClip()
    {
        if (isPlayingMusic)
        {
            muiscIndex--;
            if (muiscIndex < 0) {
                muiscIndex++;
                return;
            }
            
            soundPlayer.clip = music[muiscIndex];
            soundPlayer.Play();
        }
        else
        {
            fxIndex--;
            if (fxIndex < 0) {
                fxIndex++;
                return;
            }
            soundPlayer.clip = fx[fxIndex];
            soundPlayer.Play();
        }
    }
}
