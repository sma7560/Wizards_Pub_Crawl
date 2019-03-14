using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//This script is for casting skills based on their description from the designated Skill Scriptable object Asset.
public class AbilityCaster : NetworkBehaviour
{
    private Skill currentCastSkill;
    public GameObject[] effects; // Will use this to store a list of effects to be played on the server
    public GameObject[] projectiles; // Will Use this to store a list of projectiles on the server
    private TestAnimConrtoller anim;
    private PlayerSoundController playerSounds;

    // Probably will need some network synching thing here to command server to do stuff. Maybe Call it server interface.

    // On Start of this script get the animation
    void Start()
    {
        //if (!isLocalPlayer) return;
        // Getting these components to work
        anim = GetComponent<TestAnimConrtoller>();
        playerSounds = GetComponent<PlayerSoundController>();
    }
    // To do the inital cast of the skill.
    // Order Of events: 1. Determine how to cast and what to affect. 2.Determine if there is movement included. 3. play the visuals associated.
    // This will probably be a command.
    public void CastSkill(Skill skillToCast)
    {
        Debug.Log("I am localplayer: " + isLocalPlayer);
        if (!isLocalPlayer) return;
        //Debug.Log(skillToCast.castType);
        currentCastSkill = skillToCast;
        Debug.Log(currentCastSkill.castType);
        switch (currentCastSkill.castType)
        {
            case CastType.selfAoe:
                Debug.Log("Calling Casting on Server");
                CmdCastSelfAOE(currentCastSkill.skillRange, currentCastSkill.damageAmount, currentCastSkill.damageType);
                CmdPlayEffect(currentCastSkill.visualEffectIndex);
                break;
            case CastType.projectile:
                CmdCastProjectile(currentCastSkill.skillRange, currentCastSkill.damageAmount, currentCastSkill.damageType, currentCastSkill.projectileSpeed, currentCastSkill.projectilePrefabIndex);
                break;
            case CastType.raycast: // TODO
                CastRayCast();
                break;
            case CastType.self: // TODO
                CastSelf();
                break;
        }
        switch (currentCastSkill.moveType)
        {
            case MovementType.dash:
                MoveTeleport();
                break;
            case MovementType.teleport:
                MoveDash();
                break;
        }
        anim.PlayAnim(currentCastSkill.skillType); // Via network animator animations are already synched on network.
        //CmdPlayEffect(currentCastSkill.visualEffectIndex);

    }
    [Command]
    private void CmdCastSelfAOE(float range, int damage, DamageType dtype)
    {
        Debug.Log("I got here");
        Collider[] aroundMe = Physics.OverlapSphere(transform.position, range);
        foreach (Collider hit in aroundMe)
        {
            if (hit.tag == "Enemy" || hit.tag == "Player")
            {
                if (hit.transform.root != transform)
                {
                    // Doing stuff in here.
                    switch (hit.tag)
                    {
                        case "Enemy":
                            // Get enemy component for dealing damage to it.
                            if (hit.GetComponent<EnemyStats>())
                            {
                                // This will change.
                                hit.GetComponent<EnemyStats>().CmdTakeDamage(damage);
                            }
                            break;
                        case "Player":
                            // Get other players component for taking damage.
                            hit.GetComponent<NetworkHeroManager>().CmdTakeDamage(damage, dtype);
                            break;
                    }

                    //Rigidbody r = hit.GetComponent<Rigidbody>();
                    // Can probably add a pulling effect on this too but not now.
                    //if (r != null && currentCastSkill.ccType == CcType.push) {
                    //    r.AddExplosionForce((float)currentCastSkill.damageAmount, t.position, currentCastSkill.skillRange);
                    //}
                }
            }
        }
    }

    private void CastSelf()
    {
        // Casting self.
        Debug.Log("Casting Self Skill");
    }
    [Command]
    private void CmdCastProjectile(float range, int damage, DamageType dtype, float speed, int pindex)
    {

        GameObject bolt = Instantiate(projectiles[pindex]);
        bolt.transform.position = transform.position + transform.forward * 1f + transform.up * 1f;
        bolt.transform.rotation = transform.rotation;
        bolt.GetComponent<Rigidbody>().velocity = bolt.transform.forward * speed;
        bolt.GetComponent<BaseProjectile>().SetProjectileParams(range, damage, dtype); //Give the projectile the parameters;
        NetworkServer.Spawn(bolt);
        playerSounds.playDeathBallSoundEffect();
        Destroy(bolt, range);

    }

    private void CastRayCast()
    {

    }
    private void MoveTeleport()
    {

    }

    //
    private void MoveDash()
    {
    }

    // To go over and play the particle effect.
    [Command]
    private void CmdPlayEffect(int index)
    {
        if (effects == null) Debug.Log("Effects not instantiated. on server.");
        GameObject effect = Instantiate(effects[index], transform);
        NetworkServer.Spawn(effect);
        Destroy(effect, 2);
    }

    private void PlayAnimation()
    {
    }
}
