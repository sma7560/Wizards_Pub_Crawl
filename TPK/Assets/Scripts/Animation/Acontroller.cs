using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acontroller : MonoBehaviour {
	public Animator anim;
    public Rigidbody rbody;
    /*public float speed = 0.0001F;
	public float rotationSpeed = 1.0F;*/
    private float inputH;
    private float inputV;
    

    // Use this for initialization
    void Start () {
		anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

        /*float translation = Input.GetAxis("Vertical") * speed;
        
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
		translation += Time.deltaTime;
		rotation += Time.deltaTime;
		transform.Translate(0, 0, translation);
		transform.Rotate(0, rotation, 0);*/
        bool walkF = Input.GetKey("w");
        anim.SetBool("isWalking", walkF);
        bool hitPressed = Input.GetKey("space");
        anim.SetBool("isHit", hitPressed);
        if (Input.GetKeyDown("j"))
        {
            anim.Play("atk2", -1, 0f);
        }
        
        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");
        anim.SetFloat("inputH", inputH);
        anim.SetFloat("inputV", inputV);
        float moveX = inputH * 20f * Time.deltaTime;
        float moveZ = inputV * 50f * Time.deltaTime;
        transform.Translate(0, 0, 0.01f * moveZ);
        //rbody.velocity = new Vector3(moveX, 0f, moveZ);

    }
} 
