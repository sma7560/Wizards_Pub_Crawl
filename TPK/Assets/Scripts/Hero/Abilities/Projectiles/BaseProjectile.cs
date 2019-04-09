using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Base class for all projectiles.
/// </summary>
public class BaseProjectile : NetworkBehaviour
{
    public float range;
    public int damage;

    /// <summary>
    /// This function allows the caster to dynamically set the projectile specifications, mainly range and damage.
    /// </summary>
    public void SetProjectileParams(float r, int dmg)
    {
        range = r;
        damage = dmg;
    }

    // This is here for projectiles that actually collide with other objects.
    private void OnCollisionEnter(Collision collision)
    {
        if (!isServer) return;
        Behaviour(collision);
    }

    // This function is here for projectils that act as triggers and do not explicity collide. eg. Piercing Shot.
    private void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        Behaviour(other);
    }

    /// <summary>
    /// Base Behaviour function for all collision based projectiles.
    /// This is meant to be overriden by all scripts inheriting from this one.
    /// </summary>
    public virtual void Behaviour(Collision col) { }

    /// <summary>
    /// Base Behaviour function for all trigger based projectiles
    /// This is meant to be overriden by trigger based projectiles using scripts that inherit from this one.
    /// </summary>
    public virtual void Behaviour(Collider other) { }
}
