using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Casts skills based on their description.
/// Skill descriptions come from the designated Skill ScriptableObject asset.
/// </summary>
public class AbilityCaster : NetworkBehaviour
{
    public GameObject[] effects;        // list of all effects to be played on the server
    public GameObject[] projectiles;    // list of all projectiles on the server

    private Skill currentCastSkill;             // skill currently being cast
    private AnimController anim;                // Player animator
    private HeroModel stats;                    // Hero stat data
    private PlayerSoundController playerSounds; // plays skill sound effects

    void Start()
    {
        anim = GetComponent<AnimController>();
        stats = GetComponent<HeroModel>();
        playerSounds = GetComponent<PlayerSoundController>();
    }

    /// <summary>
    /// Casts the specified skill.
    /// Order of events:
    ///   1. Determine how to cast and what to affect.
    ///   2. Determine if there is movement included.
    ///   3. Play the visuals associated.
    /// </summary>
    /// <param name="skillToCast">Skill to cast.</param>
    public void CastSkill(Skill skillToCast)
    {
        if (!isLocalPlayer) return;

        StartCoroutine(AnimDelay(skillToCast));
    }

    /// <summary>
    /// Adds a delay for animation before actually casting the skill.
    /// </summary>
    /// <param name="skillToCast">Skill to cast.</param>
    private IEnumerator AnimDelay(Skill skillToCast)
    {
        // Play animation
        SkillType st = skillToCast.skillType;
        anim.PlayAnim(st);

        // Wait
        switch (st)
        {
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

    /// <summary>
    /// Carries out the effects associated with the casted skill.
    /// </summary>
    /// <param name="skillToCast">Skill to cast.</param>
    private void PlaySkillEffects(Skill skillToCast)
    {
        currentCastSkill = skillToCast;

        // Calculate damage
        float atk = stats.GetCurrentAttack();
        atk = atk / 50;
        float finalDmg = currentCastSkill.damageAmount * (1 + atk);
        int fd = (int)finalDmg;

        switch (currentCastSkill.castType)
        {
            case CastType.selfAoe:
                CmdCastSelfAOE(currentCastSkill.skillRange, fd);
                CmdPlayEffect(currentCastSkill.visualEffectIndex);
                break;
            case CastType.projectile:
                Vector3 fwd = transform.forward;
                Vector3 rh = transform.right;   // left is negative of this
                Vector3 pos = transform.position;
                Vector3 rot = transform.rotation.eulerAngles;
                // Max number of projectiles is 8
                Vector3[] directions = { fwd, rh + fwd, -rh + fwd, -fwd, -fwd - rh, -fwd + rh, rh, -rh };
                for (int i = 0; i <= currentCastSkill.numProjectiles - 1; i++)
                {
                    if (i > 6) break;
                    CmdCastProjectile(currentCastSkill.skillRange, fd, currentCastSkill.projectileSpeed, currentCastSkill.projectilePrefabIndex, fwd.x, fwd.y, fwd.z, pos.x, pos.y, pos.z, rot.x, rot.y, rot.z);
                }

                break;
            case CastType.raycast:
                CastRayCast();
                break;
            case CastType.self:
                CastSelf();
                break;
        }

        switch (currentCastSkill.moveType)
        {
            case MovementType.dash:
                MoveDash();
                break;
            case MovementType.teleport:
                MoveTeleport();
                break;
        }
    }

    /// <summary>
    /// Deals AOE damage.
    /// </summary>
    /// <param name="range">Range of AOE skill.</param>
    /// <param name="damage">Damge of skill.</param>
    [Command]
    private void CmdCastSelfAOE(float range, int damage)
    {
        playerSounds.RpcPlayAOESound();

        Collider[] aroundMe = Physics.OverlapSphere(transform.position, range);
        foreach (Collider hit in aroundMe)
        {
            if (hit.tag == "Enemy" || hit.tag == "Player")
            {
                if (hit.transform.root != transform)
                {
                    switch (hit.tag)
                    {
                        case "Enemy":
                            // Damage enemy
                            if (hit.GetComponent<EnemyModel>() != null)
                            {
                                hit.GetComponent<EnemyModel>().CmdTakeDamage(damage);
                            }
                            break;
                        case "Player":
                            // Damage player
                            if (hit.GetComponent<HeroModel>() != null)
                            {
                                hit.GetComponent<HeroModel>().CmdTakeDamage(damage);
                            }
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Casts the projectile.
    /// </summary>
    /// <param name="pindex">Index of the desired projectile in the list of projectiles.</param>
    [Command]
    private void CmdCastProjectile(float range, int damage, float speed, int pindex, float fx, float fy, float fz, float px, float py, float pz, float rx, float ry, float rz)
    {
        playerSounds.RpcPlaySoundEffect(projectiles[pindex].name);

        Vector3 originalPOS = new Vector3(px, py, pz);
        Vector3 originalFWD = new Vector3(fx, fy, fz);
        Quaternion originalROT = Quaternion.Euler(rx, ry, rz);

        // Set bolt position, speed, and parameters
        // This should be done locally so the direction is synced on client side to feel better
        Vector3 projPos = transform.position + originalFWD * 2f + transform.up * 1.5f;

		GameObject bolt = Instantiate(projectiles[pindex], projPos, originalROT);
        
        
        bolt.GetComponent<Rigidbody>().velocity = bolt.transform.forward * speed;
        bolt.GetComponent<BaseProjectile>().SetProjectileParams(range, damage);

        NetworkServer.Spawn(bolt);
        Destroy(bolt, range);
    }

    /// <summary>
    /// Plays the particle effect.
    /// </summary>
    /// <param name="index">Index of effect in effect list.</param>
    [Command]
    private void CmdPlayEffect(int index)
    {
        GameObject effect = Instantiate(effects[index], transform);
        NetworkServer.Spawn(effect);
    }

    /// ------------------------------
    /// TODO
    /// ------------------------------
    private void CastRayCast() { }
    private void MoveTeleport() { }
    private void MoveDash() { }
    private void CastSelf() { }
}
