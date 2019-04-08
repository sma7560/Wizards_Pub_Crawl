using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSoundController : NetworkBehaviour {

    private AudioSource source;

    [SerializeField]
    private AudioClip[] projectileSounds;
    private AudioClip aoe;
    private AbilityCaster ability;

    public enum Skills { DeathBall };

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        ability = GetComponent<AbilityCaster>();
        for(int i=0; i< ability.projectiles.Length; i++)
        {
            Debug.Log("Loading :" + ("SoundEffects/" + ability.projectiles[i].name));
            projectileSounds[i] = Resources.Load("SoundEffects/"+ability.projectiles[i].name) as AudioClip;
        }

        aoe = Resources.Load("SoundEffects/aoe") as AudioClip;

    }

    //plays different skill effects
    [ClientRpc]
    private void RpcplayProjectileSoundEffect(AudioClip sound)
    {
        source.PlayOneShot(sound);
    }

    public void playSoundeffectFor(string projectileName)
    {
        getSoundEffectWithName(projectileName);
    }

    private void getSoundEffectWithName(string projectileName)
    {
        for(int i =0; i< projectileSounds.Length; i++)
        {
            Debug.Log(projectileSounds[i].name);
            if(string.Compare(projectileSounds[i].name, projectileName)==0)
            {
                Debug.Log("returning" + projectileSounds[i].name);
            }
        }
    }

    [ClientRpc]
    public void RpcplayAoeSound()
    {
        source.PlayOneShot(aoe);
    }
}
