using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimConrtoller : MonoBehaviour {
    public Animator anim;
    public float inputH;
    public float inputV;
    public Vector3 forward;
    public float headingAngle;

    // Parameters for animator to set
    // FBMove - This is for determining forward backwards movement
    // LRMove - For determining left right movement
    //

    // Use this for initialization
    void Start () {
        // Set Up Animator
        anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
        forward = this.transform.forward;
        forward.y = 0;
        headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;
        if (headingAngle > 180f) headingAngle -= 360f; // Keeps it between -180 and 180
        Debug.Log("Horizontal: " + Input.GetAxisRaw("Horizontal"));
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");
        if (headingAngle < 45.0f && headingAngle > -45.0f)
        {
            // For Left Right
            if (inputH > 0)
            {
                // Set LR to 1;
                anim.SetFloat("LRMove", 1.0f);
            }
            else if (inputH < 0)
            {
                // Set LR to -1
                anim.SetFloat("LRMove", -1.0f);
            }
            else {
                anim.SetFloat("LRMove", 0f);
            }

            // For Forward Back
            if (inputV > 0)
            {
                // Set LR to 1;
                anim.SetFloat("FBMove", 1.0f);
            }
            else if (inputV < 0)
            {
                // Set LR to -1
                anim.SetFloat("FBMove", -1.0f);
            }
            else {
                anim.SetFloat("FBMove", 0.0f);
            }
        }
        else if (headingAngle < 135.0f && headingAngle > 45.0f)
        {
            // For Left Right
            if (inputV > 0)
            {
                // Set LR to 1;
                anim.SetFloat("LRMove", 1.0f);
            }
            else if (inputV < 0)
            {
                // Set LR to -1
                anim.SetFloat("LRMove", -1.0f);
            }
            else {
                anim.SetFloat("LRMove", 0.0f);
            }

            // For Forward Back
            if (inputH > 0)
            {
                // Set LR to 1;
                anim.SetFloat("FBMove", 1.0f);
            }
            else if (inputH < 0)
            {
                // Set LR to -1
                anim.SetFloat("FBMove", -1.0f);
            }
            else {
                anim.SetFloat("FBMove", 0.0f);
            }
        }
        else if (headingAngle > -135.0f && headingAngle < -45.0f)
        {
            // For Left Right
            if (inputV < 0)
            {
                // Set LR to 1;
                anim.SetFloat("LRMove", 1.0f);
            }
            else if (inputV > 0)
            {
                // Set LR to -1
                anim.SetFloat("LRMove", -1.0f);
            }
            else {
                anim.SetFloat("LRMove", 0.0f);
            }

            // For Forward Back
            if (inputH < 0)
            {
                // Set LR to 1;
                anim.SetFloat("FBMove", 1.0f);
            }
            else if (inputH > 0)
            {
                // Set LR to -1
                anim.SetFloat("FBMove", -1.0f);
            }
            else {
                anim.SetFloat("FBMove", 0.0f);
            }
        }
        else if (headingAngle < -135.0f || headingAngle > 135.0f)
        {
            // For Left Right
            if (inputH < 0)
            {
                // Set LR to 1;
                anim.SetFloat("LRMove", 1.0f);
            }
            else if (inputH > 0)
            {
                // Set LR to -1
                anim.SetFloat("LRMove", -1.0f);
            }
            else {
                anim.SetFloat("LRMove", 0.0f);
            }

            // For Forward Back
            if (inputV < 0)
            {
                // Set LR to 1;
                anim.SetFloat("FBMove", 1.0f);
            }
            else if (inputV > 0)
            {
                // Set LR to -1
                anim.SetFloat("FBMove", -1.0f);
            }
            else {
                anim.SetFloat("FBMove", 0.0f);
            }
        }
    }
}
