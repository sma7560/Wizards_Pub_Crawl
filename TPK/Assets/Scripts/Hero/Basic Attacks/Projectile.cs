using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Projectile : NetworkBehaviour
{
    // Assume these to be the default values for the projectile properties.
    public float range;
    public int damage;
    public DamageType damageType = DamageType.magical;
    public int playerID;
    private float hasTimeElapsed;
    private float s = 0.25f;

    // Update is called once per frame
    void Update()
    {

    }

    // This function is to overide the default values set for the projectile.
    public void SetProjectileParams(float r, int dmg, DamageType dtype, int pid)
    {
        range = r;
        damage = dmg;
        damageType = dtype;
        playerID = pid;
        hasTimeElapsed = Time.time + s;
    }

    private void OnCollisionEnter(Collision col)
    {
        // Do damage calc here.
        // Switch statment here 

        if (!isServer) return;

        switch (col.collider.tag)
        {
            case "Enemy":
                if (col.collider.GetComponent<EnemyModel>())
                {
                    // This will change.
                    col.collider.GetComponent<EnemyModel>().CmdTakeDamage(damage);
                }
                break;
            case "Player":
                if (col.collider.GetComponent<HeroModel>())
                {
                    if (Time.time < hasTimeElapsed && col.collider.GetComponent<HeroModel>().GetPlayerId() == playerID) return;
                    Debug.Log("Player ID: " + playerID + ", Hit Player ID: " + col.collider.GetComponent<HeroModel>().GetPlayerId());
                    col.collider.GetComponent<HeroModel>().CmdTakeDamage(damage, DamageType.none);
                }
                // This means you can shoot yourself... so far.
                break;
        }

        Destroy(gameObject);
    }
    private IEnumerator CollisionEnable(Collider bolt)
    {

        yield return new WaitForSeconds(0.25f);
        Physics.IgnoreCollision(GetComponent<Collider>(), bolt.GetComponent<Collider>(), false);

    }
}
