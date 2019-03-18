using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{

    public float range = 10f;
    public int damage = 5;
    public DamageType damageType = DamageType.magical;

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

    public virtual void Behaviour(Collision col)
    {
        //Debug.Log("Base Behaviour");

    }
}
