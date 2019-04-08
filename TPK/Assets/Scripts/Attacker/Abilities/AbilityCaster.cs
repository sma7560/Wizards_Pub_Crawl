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
    private HeroModel stats;
    private PlayerSoundController playerSounds;
    // Probably will need some network synching thing here to command server to do stuff. Maybe Call it server interface.

    // On Start of this script get the animation
    void Start()
    {
        //if (!isLocalPlayer) return;
        // Getting these components to work
        anim = GetComponent<TestAnimConrtoller>();
        stats = GetComponent<HeroModel>();
        playerSounds = GetComponent<PlayerSoundController>();
    }
    // To do the inital cast of the skill.
    // Order Of events: 1. Determine how to cast and what to affect. 2.Determine if there is movement included. 3. play the visuals associated.
    // This will probably be a command.
    public void CastSkill(Skill skillToCast)
    {
        Debug.Log("I am localplayer: " + isLocalPlayer);
        if (!isLocalPlayer) return;
        StartCoroutine(AnimDelay(skillToCast));
     
    }
    private void PlaySkillEffects(Skill skillToCast) {
        currentCastSkill = skillToCast;
        float atk = (float)stats.GetCurrentAttack();
        atk = atk / 50;
        Debug.Log("Calculated percentage modification: " + atk);
        float finalDmg = currentCastSkill.damageAmount * (1 + atk);
        Debug.Log("Final Damage Calculated:" + (int)finalDmg);
        int fd = (int)finalDmg;
        switch (currentCastSkill.castType)
        {
            case CastType.selfAoe:
                Debug.Log("Calling Casting on Server");
                CmdCastSelfAOE(currentCastSkill.skillRange, fd, currentCastSkill.damageType);
                CmdPlayEffect(currentCastSkill.visualEffectIndex);
                break;
            case CastType.projectile:
                Vector3 fwd = transform.forward;
                Vector3 rh = transform.right; // Left is negative this.

                // Max number of projectiles is 8
                Vector3[] directions = { fwd, rh + fwd, -rh + fwd, -fwd, -fwd - rh, -fwd + rh, rh, -rh };
                for (int i = 0; i <= currentCastSkill.numProjectiles-1; i++) {
                    if (i > 6) break;
                    CmdCastProjectile(currentCastSkill.skillRange, fd, currentCastSkill.damageType, currentCastSkill.projectileSpeed, currentCastSkill.projectilePrefabIndex, directions[i].x, directions[i].y, directions[i].z);
                }
                //CmdCastProjectile(currentCastSkill.skillRange, currentCastSkill.damageAmount, currentCastSkill.damageType, currentCastSkill.projectileSpeed, currentCastSkill.projectilePrefabIndex, fwd.x, fwd.y, fwd.z);

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
    }
    private IEnumerator AnimDelay(Skill skillToCast) {
        // Play Animation
        SkillType st = skillToCast.skillType;
        anim.PlayAnim(st);

        // Wait
        switch (st) {
            case SkillType.buff:
                yield return new WaitForSeconds(0.30f);
                break;
            case SkillType.magicHeavy:
                yield return new WaitForSeconds(1.10f);
                break;
            case SkillType.magicLight:
                yield return new WaitForSeconds(0.40f);
                break;
        }

        // Play skill effect
        PlaySkillEffects(skillToCast);
    }


    [Command]
    private void CmdCastSelfAOE(float range, int damage, DamageType dtype)
    {
        playerSounds.RpcplayAoeSound();
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
                            hit.GetComponent<HeroModel>().CmdTakeDamage(damage, dtype);
                            break;
                    }
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
    private void CmdCastProjectile(float range, int damage, DamageType dtype, float speed, int pindex, float x, float y, float z)
    {
        Debug.Log("Damage: " + damage);
        playerSounds.RpcplaySoundeffectFor(projectiles[pindex].name); //play corresponding sound effect
        Vector3 fwd = new Vector3(x, y, z);
        GameObject bolt = Instantiate(projectiles[pindex]);
        // This should be done locally so the direction is synched on client side to feel better.
        bolt.transform.position = transform.position + fwd * 2f + transform.up * 1.5f;
        bolt.transform.rotation = transform.rotation;
        bolt.GetComponent<Rigidbody>().velocity = bolt.transform.forward * speed;
        bolt.GetComponent<BaseProjectile>().SetProjectileParams(range, damage, dtype); //Give the projectile the parameters;
        NetworkServer.Spawn(bolt);
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
        //Destroy(effect, 2);
    }

    private IEnumerator SpawnAbilityTimer(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
    // 0 for Attack, 1 for Defence, 2 for Movement
    private IEnumerator BuffAbilityTimer(int statToChange, int amount, int duration)
    {

        switch (statToChange) {
            case 0:
                stats.SetCurrentAttack(stats.GetCurrentAttack() + amount);
                break;
            case 1:
                stats.SetCurrentDefense(stats.GetCurrentDefense() + amount);
                break;
            case 2:
                stats.SetCurrentMoveSpeed(stats.GetCurrentMoveSpeed() + amount);
                break;
        }
        yield return new WaitForSeconds(duration);
        switch (statToChange)
        {
            case 0:
                stats.SetCurrentAttack(stats.GetCurrentAttack() - amount);
                break;
            case 1:
                stats.SetCurrentDefense(stats.GetCurrentDefense() - amount);
                break;
            case 2:
                stats.SetCurrentMoveSpeed(stats.GetCurrentMoveSpeed() - amount);
                break;
        }
    }
}
