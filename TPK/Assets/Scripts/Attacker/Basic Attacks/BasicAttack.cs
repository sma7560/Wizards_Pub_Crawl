using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class BasicAttack : NetworkBehaviour {

    // Should probably make these private and create getters and setters.
    public float range;
    public float magRange;
    public float angleRange;
    public int damage;
    public HeroType attackType;
    public DamageType damageType;
    public GameObject projectilePrefab;

    // Use this for initialization
    void Start() {
    }

    //public void SetDamage(int amount, DamageType dtype) {

    //}

    // Update is called once per frame
    void Update() {

        if (!isLocalPlayer) return;
        // The code here is temporary.
        if (Input.GetKeyDown(KeyCode.F)) {
            if (attackType == HeroType.melee)
            {
                Collider[] aroundMe = Physics.OverlapSphere(this.transform.position, range);
                DoMelee(aroundMe);
            }
            else {
                CmdDoMagic();
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            switch (attackType) {
                case HeroType.magic:
                case HeroType.range:
                    attackType = HeroType.melee;
                    break;
                case HeroType.melee:
                    attackType = HeroType.magic;
                    break;

            }
        }

    }
    [Command]
    private void CmdDoMagic() {
        GameObject bolt = Instantiate(projectilePrefab);
        bolt.transform.position = transform.position + transform.forward*1.5f;
        bolt.transform.rotation = transform.rotation;
        bolt.GetComponent<Rigidbody>().velocity = bolt.transform.forward * 10f;
        NetworkServer.Spawn(bolt);
        Destroy(bolt, magRange);
        
    }

    // This function is 
    private void DoMelee(Collider[] aroundme) {
        for (int i = 0; i < aroundme.Length; i++) {
            if (aroundme[i].tag == "Player" || aroundme[i].tag == "Enemy") {
                if (aroundme[i].transform.root == transform)
                {
                    Debug.Log("You fool, this is me");
                }
                else {
                    Vector3 target = aroundme[i].transform.position - transform.position;
                    float angle = Vector3.Angle(target, transform.forward);
                    if (angle < angleRange) {
                        Debug.Log("Object " + i + " found: " + aroundme[i].name + ", Distance: " + target.magnitude + ", Angle:" + angle);
                        switch (aroundme[i].tag)
                        {
                            case "Enemy":
                                // Deal damage to enemy/monster via their take damage script.
                                Debug.Log("Enemy spawn is taking damage.");

                                break;
                            case "Player":
                                // Deal damage to player via their damage taking script.
                                Debug.Log("Player is taking damage");
                                //aroundme[i].GetComponent<NetworkHeroManager>();
                                if (aroundme[i].GetComponent<NetworkHeroManager>()) {
                                    Debug.Log("Applying Damage...");
                                    aroundme[i].GetComponent<NetworkHeroManager>().CmdTakeDamage(10, DamageType.none);
                                    
                                }
                                break;
                        }
                    }
                    //aroundme[i].enabled = false;


                }
                
            }

        }
    }
}
