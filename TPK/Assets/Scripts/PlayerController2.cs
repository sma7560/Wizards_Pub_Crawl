using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour {
	public float speed;
	public Rigidbody myPlayer;
	private Vector3 moveInput;
	private Vector3 moveVelocity;
	private Camera mainCamera;
	//public GunController theGun; 

	// Use this for initialization
	void Start () {
		myPlayer =  GetComponent<Rigidbody>();
		mainCamera = FindObjectOfType<Camera>();
		
	}
	/*public void Drawline(){
		Vector3.Start;
		Vector3.End;

	}*/
	
	// Update is called once per frame
	void Update () {
		moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
		moveVelocity = moveInput * speed;
		Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
		Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
		float rayLength;

		if (groundPlane.Raycast(cameraRay, out rayLength)){
			Vector3 pointToLook = cameraRay.GetPoint(rayLength);
			//Debug.Drawline(cameraRay.origin, pointToLook, Color.blue);
			
			transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
		}
		/*if(Input.GetMouseButtonDown(0))
			theGun.isFiring = true;

		if(Input.GetMouseButtonUp(0))
			theGun.isFiring = false;*/
	}

	void FixedUpdate(){
		myPlayer.velocity = moveVelocity;
	}
}
