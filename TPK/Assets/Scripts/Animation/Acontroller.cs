using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acontroller : MonoBehaviour {
	public Animator anim;
    public Rigidbody rbody;
    private float inputH;
    private float inputV;
    private bool run;
    //Use this for skill
    public GameObject RangedSpellPrefab;
    public GameObject selectedUnit;

    // Use this for initialization
    void Start () {
		anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
        run = false;
    }
	
	// Update is called once per frame
	void Update () {

        /*This is for top down angle with 360 degree rotation
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
		translation += Time.deltaTime;
		rotation += Time.deltaTime;
		transform.Translate(0, 0, translation);
		transform.Rotate(0, rotation, 0);*/
        //bool walkF = Input.GetKey("w");
        //anim.SetBool("isWalking", walkF);
        //bool hitPressed = Input.GetKey("space");
        //anim.SetBool("isHit", hitPressed);

        // Matk for magic atk, P for physics atk
        if (Input.GetKeyDown("j"))
        {
            anim.Play("Matk1", -1, 0f);
            RangeAttack();
        }

        if (Input.GetKeyDown("k"))
        {
            anim.Play("Matk2", -1, 0f);
        }
        //Swing
        if (Input.GetKey("space"))
        {
            anim.Play("Patk1", -1, 0f);
        }
        //Kick
        if (Input.GetKey(KeyCode.Alpha1))
        {
            anim.Play("Patk2", -1, 0f);
        }

        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");
        anim.SetFloat("inputH", inputH);
        anim.SetFloat("inputV", inputV);
        anim.SetBool("run", run);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            run = true;
        }
        else
        {
            run = false;
        }
        float moveX = inputH * 20f * Time.deltaTime;
        float moveZ = inputV * 50f * Time.deltaTime;

        if(moveZ <= 0f)
        {
            moveX = 0f;
        }
        else if(run)
        {
            moveX *= 3f;
            moveZ *= 3f;
        }
        transform.Translate(0, 0, 0.01f * moveZ);
        //altertate translation
        //rbody.velocity = new Vector3(moveX, 0f, 1f * moveZ);

    }
    /*void BasicAttack()
    {
        EnemyStats.Receivedamage(?);
    }*/
    void RangeAttack()
    {
        //projectile spawing at centre of player
        Vector3 SpawnSpell = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        GameObject clone;
        clone = Instantiate(RangedSpellPrefab, SpawnSpell, Quaternion.identity);
        clone.transform.GetComponent<RangedSpell>().Target = selectedUnit;
    }
} 
