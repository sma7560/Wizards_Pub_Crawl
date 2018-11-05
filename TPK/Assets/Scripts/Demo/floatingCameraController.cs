using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatingCameraController : MonoBehaviour {

    public float flySpeed;
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private Vector3 x;
    private Vector3 z;
    private Vector3 direction;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Camera Rotation
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        //Camera move
        x = Input.GetAxis("Horizontal") * Time.deltaTime * transform.right * flySpeed;
        z = Input.GetAxis("Vertical") * Time.deltaTime * transform.forward * flySpeed;
        direction = x + z;
        transform.position += direction;
        

    }
}
