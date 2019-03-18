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
    [SyncVar] public int damage = 10;
    [SyncVar] public HeroType attackType;
    [SyncVar] public DamageType damageType = DamageType.none;
    public GameObject projectilePrefab;

    // Use this for initialization
    void Start()
    {
        angleRange = 45f;
    }

    // Update is called once per frame
    void Update()
    {

        //if (!isLocalPlayer) return;
        //// The code here is temporary.
        //if (Input.GetKeyDown(KeyCode.F)) {
        //    if (attackType == HeroType.melee)
        //    {
        //        Collider[] aroundMe = Physics.OverlapSphere(this.transform.position, range);
        //        CmdDoMelee(aroundMe);
        //    }
        //    else {
        //        CmdDoMagic();
        //    }
        //}
        //if (Input.GetMouseButtonDown(1)) {
        //    switch (attackType) {
        //        case HeroType.magic:
        //        case HeroType.range:
        //            attackType = HeroType.melee;
        //            break;
        //        case HeroType.melee:
        //            attackType = HeroType.magic;
        //            break;

        //    }
        //}
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
        damageType = DamageType.physical;
        //switch (attackType)
        //{
        //    case HeroType.magic:
        //    case HeroType.range:
        //        magRange = 10f;
        //        damageType = (attackType == HeroType.magic) ? DamageType.magical : DamageType.physical;
        //        break;
        //    case HeroType.melee:
        //        range = 2f;
        //        damageType = DamageType.physical;
        //        break;
        //    default:
        //        range = 2;
        //        magRange = 10;
        //        damageType = DamageType.none;
        //        break;
        //}
    }

    public void PerformAttack()
    {
        if (!isLocalPlayer) return;
        //switch (attackType)
        //{
        //    case HeroType.magic:
        //    case HeroType.range:
        //        CmdDoMagic();
        //        break;
        //    case HeroType.melee:
        //        CmdDoMelee();
        //        break;
        //}
        CmdDoMagic();
    }

    // This function is a command to spawn the basic attack project tile on the server.
    [Command]
    private void CmdDoMagic()
    {
        GameObject bolt = Instantiate(projectilePrefab);
        bolt.transform.position = transform.position + transform.forward * 2f + transform.up * 1.5f;
        bolt.transform.rotation = transform.rotation;
        bolt.GetComponent<Rigidbody>().velocity = bolt.transform.forward * 10f;
        bolt.GetComponent<Projectile>().SetProjectileParams(magRange, damage, damageType); //Give the projectile the parameters;
        NetworkServer.Spawn(bolt);
        Destroy(bolt, magRange);

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
