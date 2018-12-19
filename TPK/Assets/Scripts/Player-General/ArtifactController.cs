using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Carryable Artifact script
public class ArtifactController : MonoBehaviour {

    private bool isCarried;     //is artifact carried by someone?
    private GameObject playerThatOwns; //set to player that is carrying this
	private int ownerID;
	private Vector3 ownerSpawn;

	Vector3 smallscale = new Vector3( 1f, 1f, 1f);	//smaller size when carried
	Vector3 normalscale = new Vector3(2f, 2f, 2f);	//normal size on ground

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

    // Use this for initialization
    void Start()
    {
		transform.localScale = normalscale;
        //artifactBody = GetComponent<Rigidbody>();
    }


    void Update()
    {
        //if being carried, update location of artifact to where the carrier is
        if (isCarried)
        {
			transform.position = new Vector3(playerThatOwns.transform.position.x, playerThatOwns.transform.position.y + 3, playerThatOwns.transform.position.z);
			if ( playerThatOwns.GetComponent<HeroController> ().GetKnockedOutStatus() ) {
				//player is knocked out, so he drops the artifact
				droppedArtifact();
			}
        }

		//rotation animation
		transform.RotateAround(transform.position, transform.up, Time.deltaTime * 90f);
    }

    //set player that "picked" up artifact
	private void OnTriggerEnter(Collider col)
    {
		switch (col.gameObject.transform.tag)
        {
		case ("Player"):
			//check to make sure the player isn't knocked out
			if ( !isCarried && !( col.GetComponent<HeroController>().GetKnockedOutStatus() ) ) {
				//make object float above character model's head
				transform.localScale = smallscale;
				playerThatOwns = col.gameObject;
				ownerID = playerThatOwns.GetComponent<HeroController> ().getPlayerId ();
				ownerSpawn = GameObject.FindGameObjectWithTag ("MatchManager").GetComponent<HeroManager> ().GetSpawnLocationOfPlayer (ownerID);
				isCarried = true;
				Debug.Log ("Player " + ownerID + " has taken the artifact!");
			}
			break;
		case ("SpawnRoom"):			//Player enters scoring location (spawn point)
			if (isCarried) {
				//need to check that the spawn is the right one for the player carrying the artifact
				if (Vector3.Distance (transform.position, ownerSpawn) <= 10) {
					playerThatOwns.GetComponent<HeroController> ().addScore (1);		//adds a point to scoring player, then deletes itself
					GameObject.FindGameObjectWithTag("ArtifactSpawnControl").GetComponent<ArtifactSpawn>().SpawnArtifactRandom();
					Destroy (gameObject);
				}
			}
			break;
        }
    }

    //artifact has been "dropped"
    public void droppedArtifact()
    {
		transform.position = new Vector3(playerThatOwns.transform.position.x, playerThatOwns.transform.position.y - 3, playerThatOwns.transform.position.z);
        playerThatOwns = null;
        isCarried = false;
        //scale size back up
		transform.localScale = normalscale;
    }
}
