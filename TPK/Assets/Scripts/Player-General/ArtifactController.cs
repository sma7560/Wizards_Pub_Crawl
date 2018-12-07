using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Carryable Artifact script
public class ArtifactController : MonoBehaviour {

    private bool isCarried;     //is artifact carried by someone?
    private GameObject playerThatOwns; //set to player that is carrying this

    //Common, Rare, Epic, Legendary
    public string rarity 
    {
        get
        {
            return rarity;
        }
        set
        {
            rarity = value;
        }
    }

    //how many points artifact is worth
    public string points 
    {
        get
        {
            return points;
        }
        set
        {
            points = value;
        }
    }

    private Transform artifactTransform;
    private Rigidbody artifactBody;

    // Use this for initialization
    void Start()
    {
        artifactTransform = GetComponent<Transform>();
        artifactBody = GetComponent<Rigidbody>();
    }


    void Update()
    {
        //if being carried, update location of artifact to where the carrier is
        if (isCarried)
        {
            artifactTransform.position = playerThatOwns.transform.position;
        }
    }

    //set player that "picked" up artifact
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Hero")
        {
            //make object small enough to hide inside character model
            artifactTransform.localScale = new Vector3( 0.1f, 0.1f, 0.1f);
            playerThatOwns = collision.gameObject;
            isCarried = true;
        }
    }

    //artifact has been "dropped"
    public void droppedArtifact()
    {
        playerThatOwns = null;
        isCarried = false;
        //scale size back up
        artifactTransform.localScale = new Vector3(1f, 1f, 1f);
    }
}
