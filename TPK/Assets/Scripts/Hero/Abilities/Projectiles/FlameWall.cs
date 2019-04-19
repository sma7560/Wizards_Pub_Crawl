using UnityEngine;

/// <summary>
/// Behaviour for Flame Wall skill.
/// </summary>
public class FlameWall : BaseProjectile
{
    private readonly float dmgDelay = 0.1f;     // cooldown of damage done by flame wall
    private float nextActiveTime;               // the next time where flame wall will do damage

    void Awake()
    {
        nextActiveTime = 0;
    }

    void Update()
    {
        // Update next active time
        if (Time.time > nextActiveTime)
        {
            nextActiveTime = Time.time + dmgDelay;
        }
    }

    /// <summary>
    /// Damage over time if enemy/player stays within the flame wall.
    /// </summary>
    void OnTriggerStay(Collider col)
    {
        if (Time.time < nextActiveTime) return;

        switch (col.transform.tag)
        {
            case "Enemy":
                // Damage enemy
                if (col.transform.GetComponent<EnemyModel>() != null)
                {
                    col.transform.GetComponent<EnemyModel>().CmdTakeDamage(damage);
                }
                break;
            case "Player":
                // Damage player
                if (col.transform.GetComponent<HeroModel>() != null)
                {
                    col.transform.GetComponent<HeroModel>().CmdTakeDamage(damage);
                }
                break;
        }
    }

    /// <summary>
    /// Initial damage when enemy/player touches flame wall.
    /// </summary>
    public override void Behaviour(Collider col)
    {
        switch (col.transform.tag)
        {
            case "Enemy":
                // Damage enemy
                if (col.transform.GetComponent<EnemyModel>() != null)
                {
                    col.transform.GetComponent<EnemyModel>().CmdTakeDamage(damage);
                }
                break;
            case "Player":
                // Damage player
                if (col.transform.GetComponent<HeroModel>() != null)
                {
                    col.transform.GetComponent<HeroModel>().CmdTakeDamage(damage);
                }
                break;
        }
    }
}
