using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acontroller : MonoBehaviour {
	static Animator anim;
	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		float translation = Input.GetAxis("Vertical") * speed;
		float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
		translation += Time.deltaTime;
		rotation += Time.deltaTime;
		transform.Translate(0, 0, translation);
		transform.Rotate(0, rotation, 0);
		if(Input.GetButtonDown("Jump")){
			anim.SetTrigger("punch");
		}

		if(translation != 0){
			anim.SetBool("isRunning", true);
		}
		else{
			anim.SetBool("isRunning", false);
		}
		
	}
} 
