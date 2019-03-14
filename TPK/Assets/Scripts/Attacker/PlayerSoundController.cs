using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour {

    private AudioSource source;
    private AudioClip deathBall;

    public enum Skills { DeathBall };

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        //load deathball sound effect from resource folder
        deathBall = Resources.Load("SoundEffects/DeathBall") as AudioClip;
	}
	
    //plays different skill effects
	public void playDeathBallSoundEffect()
    {
         source.PlayOneShot(deathBall);
    }
}
