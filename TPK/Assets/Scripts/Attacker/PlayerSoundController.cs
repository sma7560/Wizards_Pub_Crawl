using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSoundController : NetworkBehaviour {

    private AudioSource source;
    private AudioSource footAudio1;
    private AudioSource footAudio2;
    private AudioSource footAudio3;

    private AudioClip deathBall;
    private AudioClip aoe;

    public enum Skills { DeathBall };

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();

        //load deathball sound effect from resource folder
        deathBall = Resources.Load("SoundEffects/DeathBall") as AudioClip;
        aoe = Resources.Load("SoundEffects/aoe") as AudioClip;

    }

    //plays different skill effects
    [ClientRpc]
    public void RpcplayDeathBallSoundEffect()
    {
        source.PlayOneShot(deathBall);
    }

    [ClientRpc]
    public void RpcplayAoeSound()
    {
        source.PlayOneShot(aoe);
    }
}
