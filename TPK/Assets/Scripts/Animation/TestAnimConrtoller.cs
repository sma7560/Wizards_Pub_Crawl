﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestAnimConrtoller : NetworkBehaviour {
    public Animator anim;
    public float inputH;
    public float inputV;
    public Vector3 forward;
    public float headingAngle;
    public int basicNum;
    public float timeToReset;
    public float timeElapsed;
    public float timeElapsedBetween;
    public float timeBetween;
    public float attaking;

    public bool isWalking;

    // Parameters for animator to set
    // FBMove - This is for determining forward backwards movement
    // LRMove - For determining left right movement
    //

    // Use this for initialization
    void Start () {
        if (!isLocalPlayer) return;
        // Set Up Animator
        anim = GetComponent<Animator>();
        basicNum = 0;
        timeToReset = 1.5f;
        timeElapsed = 0;
        timeElapsedBetween = 0;
        timeBetween = 1.0f;
        attaking = 0;
        isWalking = false;


    }
	
	// Update is called once per frame
	void Update () {
         if (!isLocalPlayer) return;
        forward = this.transform.forward;
        forward.y = 0;
        headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;
        if (headingAngle > 180f) headingAngle -= 360f; // Keeps it between -180 and 180
        //Debug.Log("Horizontal: " + Input.GetAxisRaw("Horizontal"));
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");
        if (inputH != 0 || inputV != 0)
        {
            isWalking = true;
            anim.SetBool("isWalking", isWalking);
            SetMovementAnim();
        }
        else {
            isWalking = false;
            anim.SetBool("isWalking", isWalking);
        }
        // See if attacks Are being performed.
        SetBasicAttack();

    }
    private void SetBasicAttack() {
        // On mouse click.
        if (Input.GetMouseButtonDown(0) && timeElapsed < timeToReset && basicNum != 2)
        {
            Debug.Log("Clicked: " + basicNum);
            switch (basicNum)
            {
                case 0:
                    // Reset the count on first go of basic attacks;
                    timeElapsed = 0;
                    basicNum++;
                    anim.SetInteger("BasicAtkNum", basicNum);
                    timeElapsedBetween = 0;
                    break;
                case 1:
                    basicNum = 2;
                    anim.SetTrigger("Basic2");
                    break;
            }
        }
        else if (Input.GetMouseButtonDown(1)) {
            anim.SetTrigger("MBasic");
        }
        else
        {
            timeElapsed += Time.deltaTime;
            if (timeElapsed > timeToReset)
            {
                basicNum = 0;
                anim.SetInteger("BasicAtkNum", basicNum);
                timeElapsed = 0;
            }
        }

    }
    // This function is for setting up the movement for the legs.
    private void SetMovementAnim() {

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
            else
            {
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
            else
            {
                anim.SetFloat("FBMove", 0.0f);
            }
        }
        else if (headingAngle < 135.0f && headingAngle > 45.0f)
        {
            // For Left Right
            if (inputV > 0)
            {
                // Set LR to 1;
                anim.SetFloat("LRMove", -1.0f);
            }
            else if (inputV < 0)
            {
                // Set LR to -1
                anim.SetFloat("LRMove", 1.0f);
            }
            else
            {
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
            else
            {
                anim.SetFloat("FBMove", 0.0f);
            }
        }
        else if (headingAngle > -135.0f && headingAngle < -45.0f)
        {
            // For Left Right
            if (inputV < 0)
            {
                // Set LR to 1;
                anim.SetFloat("LRMove", -1.0f);
            }
            else if (inputV > 0)
            {
                // Set LR to -1
                anim.SetFloat("LRMove", 1.0f);
            }
            else
            {
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
            else
            {
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
            else
            {
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
            else
            {
                anim.SetFloat("FBMove", 0.0f);
            }
        }
    }
}