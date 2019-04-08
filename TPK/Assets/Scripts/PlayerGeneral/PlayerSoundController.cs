using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Plays all sound effects related to player attacks.
/// </summary>
public class PlayerSoundController : NetworkBehaviour
{
    private AudioSource source;
    private AudioClip aoe;
    private AudioClip[] projectileSounds;
    private AbilityCaster caster;

    void Start()
    {
        source = GetComponent<AudioSource>();
        caster = GetComponent<AbilityCaster>();

        // Loads in corresponding sound effect for each skill
        projectileSounds = new AudioClip[caster.projectiles.Length];
        for (int i = 0; i < caster.projectiles.Length; i++)
        {
            projectileSounds[i] = Resources.Load("SoundEffects/" + caster.projectiles[i].name) as AudioClip;
        }
        aoe = Resources.Load("SoundEffects/aoe") as AudioClip;
    }
    
    /// <summary>
    /// Play sound for AOE skill.
    /// </summary>
    [ClientRpc]
    public void RpcPlayAOESound()
    {
        source.PlayOneShot(aoe);
    }

    /// <summary>
    /// Play sound for corresponding projectile skill.
    /// </summary>
    [ClientRpc]
    public void RpcPlaySoundEffect(string projectileName)
    {
        source.PlayOneShot(GetSoundEffectWithName(projectileName));
    }

    /// <summary>
    /// Return projectile sound effect corresponding to projectile name.
    /// </summary>
    private AudioClip GetSoundEffectWithName(string projectileName)
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
