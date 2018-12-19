using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Carryable Artifact script
public class ArtifactController : MonoBehaviour {

    private bool isCarried;     //is artifact carried by someone?
    private GameObject playerThatOwns; //set to player that is carrying this
	private int ownerID;
	private Vector3 ownerSpawn;

	Vector3 smallscale = new Vector3( 0.8f, 0.8f, 0.8f);	//smaller size when carried
	Vector3 normalscale;									//normal size on ground

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
    //private Rigidbody artifactBody;

    // Use this for initialization
    void Start()
    {
        artifactTransform = GetComponent<Transform>();
		normalscale = artifactTransform.localScale;
        //artifactBody = GetComponent<Rigidbody>();
    }


    void Update()
    {
        //if being carried, update location of artifact to where the carrier is
        if (isCarried)
        {
			artifactTransform.position = new Vector3(playerThatOwns.transform.position.x, playerThatOwns.transform.position.y + 4, playerThatOwns.transform.position.z);
        }
    }

    //set player that "picked" up artifact
	private void OnTriggerEnter(Collider col)
    {
		switch (col.gameObject.tag)
        {
		case ("Player"):
			if (!isCarried) {
				//make object float above character model's head
				artifactTransform.localScale = smallscale;
				playerThatOwns = col.gameObject;
				ownerID = playerThatOwns.GetComponent<HeroController> ().getPlayerId ();
				ownerSpawn = GameObject.FindGameObjectWithTag ("Match Manager").GetComponent<HeroManager> ().GetSpawnLocationOfPlayer (ownerID);
				isCarried = true;
			}
			break;
		case ("Spawn"):			//Player entercores point
			if (isCarried) {
				//need to check that the spawn is the right one for the player carrying the artifact
				if (Vector3.Distance (artifactTransform.position, ownerSpawn) <= 5) {
					playerThatOwns.GetComponent<HeroController>().
				}
			}
			break;
        }
    }

    //artifact has been "dropped"
    public void droppedArtifact()
    {
        playerThatOwns = null;
        isCarried = false;
        //scale size back up
		artifactTransform.localScale = normalscale;
		artifactTransform.position = new Vector3(playerThatOwns.transform.position.x, playerThatOwns.transform.position.y - 4, playerThatOwns.transform.position.z);
    }
}
