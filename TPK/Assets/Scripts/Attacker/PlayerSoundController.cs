using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSoundController : NetworkBehaviour {

    private AudioSource source1;
    private AudioSource source2;
    private AudioSource footAudio1;
    private AudioSource footAudio2;
    private AudioSource footAudio3;

    private AudioClip deathBall;
    private AudioClip aoe;
    private AudioClip f1;
    private AudioClip f2;
    private AudioClip f3;


    public enum Skills { DeathBall };

	// Use this for initialization
	void Start () {
        source1 = GetComponent<AudioSource>();
        source2 = GetComponent<AudioSource>();
        footAudio1 = GetComponent<AudioSource>();
        footAudio2 = GetComponent<AudioSource>();
        footAudio3 = GetComponent<AudioSource>();

        //load deathball sound effect from resource folder
        deathBall = Resources.Load("SoundEffects/DeathBall") as AudioClip;
        aoe = Resources.Load("SoundEffects/aoe") as AudioClip;
        f1 = Resources.Load("SoundEffects/footStep1") as AudioClip;
        f2 = Resources.Load("SoundEffects/footStep2") as AudioClip;
        f3 = Resources.Load("SoundEffects/footStep3") as AudioClip;

    }

    //plays different skill effects
    [ClientRpc]
    public void RpcplayDeathBallSoundEffect()
    {
         source1.PlayOneShot(deathBall);
    }

    [ClientRpc]
    public void RpcplayAoeSound()
    {
        source2.PlayOneShot(aoe);
    }
    [ClientRpc]
    public void RpcplayFootStep()
    {
        int rand = Random.Range(1, 3);
        switch (rand)
        {
            case 1:
                footAudio1.pitch = (Random.Range(0.6f, .9f));
                footAudio1.PlayOneShot(f1);
                break;
            case 2:
                footAudio2.pitch = (Random.Range(0.6f, .9f));
                footAudio2.PlayOneShot(f2);
                break;
            case 3:
                footAudio3.pitch = (Random.Range(0.6f, .9f));
                footAudio3.PlayOneShot(f3);
                break;


        }

    }

}
