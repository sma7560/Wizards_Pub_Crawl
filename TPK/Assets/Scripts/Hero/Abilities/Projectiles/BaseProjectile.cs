using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Base class for all projectiles.
/// </summary>
public class BaseProjectile : NetworkBehaviour
{
    public float range;
    public int damage;

    public void SetProjectileParams(float r, int dmg)
    {
        range = r;
        damage = dmg;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isServer) return;
        Behaviour(collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        Behaviour(other);
    }

    public virtual void Behaviour(Collision col) { }
    public virtual void Behaviour(Collider other) { }
}
