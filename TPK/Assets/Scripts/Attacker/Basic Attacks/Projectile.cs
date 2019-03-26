using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Assume these to be the default values for the projectile properties.
    public float range = 10f;
    public int damage = 1;
    public DamageType damageType = DamageType.magical;

    // Update is called once per frame
    void Update()
    {

    }

    // This function is to overide the default values set for the projectile.
    public void SetProjectileParams(float r, int dmg, DamageType dtype)
    {
        range = r;
        damage = dmg;
        damageType = dtype;
    }

    private void OnCollisionEnter(Collision col)
    {
        // Do damage calc here.
        // Switch statment here 
        switch (col.collider.tag)
        {
            case "Enemy":
                if (col.collider.GetComponent<EnemyStats>())
                {
                    // This will change.
                    col.collider.GetComponent<EnemyStats>().CmdTakeDamage(damage);
                }
                break;
            case "Player":
                if (col.collider.GetComponent<HeroModel>())
                {
                    col.collider.GetComponent<HeroModel>().CmdTakeDamage(damage, DamageType.none);
                }
                // This means you can shoot yourself... so far.
                break;
        }

        Destroy(gameObject);
    }
}
