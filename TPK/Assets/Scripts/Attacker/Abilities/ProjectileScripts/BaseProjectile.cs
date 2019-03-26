using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{

    public float range;
    public int damage;
    public DamageType damageType;

    public void SetProjectileParams(float r, int dmg, DamageType dtype)
    {
        range = r;
        damage = dmg;
        damageType = dtype;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Behaviour(collision);
    }
    private void OnTriggerEnter(Collider other)
    {
        Behaviour(other);
    }
    public virtual void Behaviour(Collision col)
    {
        //Debug.Log("Base Behaviour");

    }
    public virtual void Behaviour(Collider other) {
            //Meant to be overwitten with skills that have colliders as triggers.
    }
}
