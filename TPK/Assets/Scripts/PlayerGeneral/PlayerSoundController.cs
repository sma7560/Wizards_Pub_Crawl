using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSoundController : NetworkBehaviour {

    private AudioSource source;
    private AudioClip deathBall;
    private AudioClip aoe;
    public AudioClip[] projectileSounds;
    private AbilityCaster ability;

	void Start () {
        source = GetComponent<AudioSource>();
        ability = GetComponent<AbilityCaster>();
        // loads in corresponding sound effect for each skill
        projectileSounds = new AudioClip[ability.projectiles.Length];
        for (int i = 0; i < ability.projectiles.Length; i++)
        {
            projectileSounds[i] = Resources.Load("SoundEffects/" + ability.projectiles[i].name) as AudioClip;
        }
        aoe = Resources.Load("SoundEffects/aoe") as AudioClip;
    }


    /// <summary>
    /// Play sound for AOE skill
    /// </summary>
    [ClientRpc]
    public void RpcplayAoeSound()
    {
        source.PlayOneShot(aoe);
    }

    /// <summary>
    /// Play sound for corresponding projectile skill
    /// </summary>
    [ClientRpc]
    public void RpcplaySoundeffectFor(string projectileName)
    {
        source.PlayOneShot(getSoundEffectWithName(projectileName));
    }

    /// <summary>
    /// Returnprojectile sound effect corresponding to string name input
    /// </summary>
    private AudioClip getSoundEffectWithName(string projectileName)
    {
        for (int i = 0; i < projectileSounds.Length; i++)
        {
            if (string.Compare(projectileSounds[i].name, projectileName) == 0)
            {
                return projectileSounds[i];
            }
        }
        return null;
    }
}
