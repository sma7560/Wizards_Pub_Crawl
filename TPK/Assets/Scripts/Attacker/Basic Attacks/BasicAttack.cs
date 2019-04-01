using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class BasicAttack : NetworkBehaviour
{
    // Should probably make these private and create getters and setters.
    [SyncVar] public float range = 0;
    [SyncVar] public float magRange = 0;
    [SyncVar] public float angleRange;
    [SyncVar] public int damage;
    [SyncVar] public HeroType attackType;
    [SyncVar] public DamageType damageType = DamageType.none;
    private float cooldown = 0.5f;
    private float nextActiveTime = 0;
    public GameObject projectilePrefab;

    // Use this for initialization
    void Start()
    {
        angleRange = 45f;
    }

    // Update is called once per frame
    void Update()
    {
    }

    // This function shouldnt be called unless by a local player but just incase, if it is called double check for local-ness...
    [Command]
    public void CmdSetAttackParameters()
    {
        //if (!isServer) return; Since is command, do not need this
        HeroModel heroModel = GetComponent<HeroModel>();
        attackType = heroModel.GetHeroType();
        attackType = HeroType.range;
        magRange = 5f;
        damageType = DamageType.none;
        damage = heroModel.GetCurrentAttack();
    }

    public void PerformAttack()
    {
        if (!isLocalPlayer) return;
        if (Time.time > nextActiveTime) {
            Debug.Log("I Number " + GetComponent<HeroModel>().GetPlayerId() + " am attacking");
            int id = GetComponent<HeroModel>().GetPlayerId();
            CmdDoMagic(id);
            nextActiveTime = Time.time + cooldown;
        }
    }


    // This function is a command to spawn the basic attack projectile on the server.
    [Command]
    private void CmdDoMagic(int id)
    {
        HeroModel heroModel = GetComponent<HeroModel>();
        Debug.Log("Player ID: " + heroModel.GetPlayerId());
        GameObject bolt = Instantiate(projectilePrefab);
        bolt.transform.position = transform.position + transform.forward * 2f + transform.up * 1.5f;
        bolt.transform.rotation = transform.rotation;
        // The speed of the basic Attack projectile.
        bolt.GetComponent<Rigidbody>().velocity = bolt.transform.forward * 28f;
        Debug.Log("Attack Value: " + heroModel.GetCurrentAttack() + ", Attacker ID:"+ id);
        bolt.GetComponent<Projectile>().SetProjectileParams(3, heroModel.GetCurrentAttack(), damageType, id); //Give the projectile the parameters;
        Debug.Log("Damage: "+ bolt.GetComponent<Projectile>().damage + ", ID: " + bolt.GetComponent<Projectile>().playerID);
        NetworkServer.Spawn(bolt);
        Destroy(bolt, 3);

    }

    // This function is a script to perform a melee check when doing a melee attack.
    [Command]
    private void CmdDoMelee()
    {
        Collider[] aroundme = Physics.OverlapSphere(this.transform.position, range);

        for (int i = 0; i < aroundme.Length; i++)
        {
            if (aroundme[i].tag == "Player" || aroundme[i].tag == "Enemy")
            {
                if (aroundme[i].transform.root == transform)
                {
                    Debug.Log("You fool, this is me");
                }
                else
                {
                    Vector3 target = aroundme[i].transform.position - transform.position;
                    float angle = Vector3.Angle(target, transform.forward);
                    if (angle < angleRange)
                    {
                        Debug.Log("Object " + i + " found: " + aroundme[i].name + ", Distance: " + target.magnitude + ", Angle:" + angle);
                        switch (aroundme[i].tag)
                        {
                            case "Enemy":
                                // Deal damage to enemy/monster via their take damage script.
                                if (aroundme[i].GetComponent<EnemyStats>())
                                {
                                    // This will change.
                                    aroundme[i].GetComponent<EnemyStats>().CmdTakeDamage(damage);
                                }

                                break;
                            case "Player":
                                // Deal damage to player via their damage taking script.
                                Debug.Log("Player is taking damage");
                                //aroundme[i].GetComponent<NetworkHeroManager>();
                                if (aroundme[i].GetComponent<HeroModel>())
                                {
                                    Debug.Log("Applying Damage...");
                                    aroundme[i].GetComponent<HeroModel>().CmdTakeDamage(damage, damageType);

                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}
