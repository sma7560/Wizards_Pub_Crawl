using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// HeroController: script for controlling attacker/hero character.
public class HeroController : NetworkBehaviour
{

    public GameObject heroCam;
    private GameObject cam;
    private float moveSpeed;
    private Rigidbody heroRigidbody;

    // Use this for initialization
    void Start()
    {
        if (!hasAuthority)
        {
            return;
        }

        moveSpeed = 5.0f;
        heroRigidbody = GetComponent<Rigidbody>();

        StartCamera();
    }

    // Update is called once per frame
    void Update()
    {
        // This function runs on all heroes

        if (!hasAuthority)
        {
            return;
        }

        CharacterMovement();
    }

    // Hero character movement
    private void CharacterMovement()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            heroRigidbody.velocity = new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed, 0, Input.GetAxisRaw("Vertical") * moveSpeed);
        }
        else
        {
            heroRigidbody.velocity = new Vector3(0, 0, 0);
        }
    }

    private void StartCamera()
    {
        Debug.Log("StartCamera called.");
        GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
        cam = Instantiate(heroCam);
        cam.GetComponent<HeroCameraController>().setTarget(this.transform);
    }
}
