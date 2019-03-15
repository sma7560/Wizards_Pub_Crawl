using UnityEngine;

// Carryable Artifact script
public class ArtifactController : MonoBehaviour
{
    private bool isCarried;     //is artifact carried by someone?
    private GameObject playerThatOwns; //set to player that is carrying this
    private int ownerID;
    private Vector3 ownerSpawn;

    Vector3 smallscale = new Vector3(1f, 1f, 1f);   //smaller size when carried
    Vector3 normalscale = new Vector3(2f, 2f, 2f);	//normal size on ground

    //Common, Rare, Epic, Legendary
    public string Rarity
    {
        get
        {
            return Rarity;
        }
        set
        {
            Rarity = value;
        }
    }

    //how many points artifact is worth
    public string Points
    {
        get
        {
            return Points;
        }
        set
        {
            Points = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        ownerID = -1;
        transform.localScale = normalscale;
        //artifactBody = GetComponent<Rigidbody>();
    }


    void Update()
    {
        //if being carried, update location of artifact to where the carrier is
        if (isCarried)
        {
            transform.position = new Vector3(playerThatOwns.transform.position.x, playerThatOwns.transform.position.y + 3, playerThatOwns.transform.position.z);
            if (playerThatOwns.GetComponent<HeroController>().IsKnockedOut())
            {
                //player is knocked out, so he drops the artifact
                DroppedArtifact();
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
                if (!isCarried && !(col.GetComponent<HeroController>().IsKnockedOut()))
                {
                    //make object float above character model's head
                    transform.localScale = smallscale;
                    playerThatOwns = col.gameObject;
                    ownerID = playerThatOwns.GetComponent<HeroController>().GetPlayerId();
                    ownerSpawn = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>().GetSpawnLocationOfPlayer(ownerID);
                    isCarried = true;
                    Debug.Log("Player " + ownerID + " has taken the artifact!");
                }
                break;
            case ("SpawnRoom"):         //Player enters scoring location (spawn point)
                if (isCarried)
                {
                    //need to check that the spawn is the right one for the player carrying the artifact
                    if (Vector3.Distance(transform.position, ownerSpawn) <= 10)
                    {
                        playerThatOwns.GetComponent<HeroController>().AddScore(1);      //adds a point to scoring player, then deletes itself
                        GameObject.FindGameObjectWithTag("EventSystem").GetComponent<ArtifactSpawn>().SpawnArtifactRandom();
                        Destroy(gameObject);
                    }
                }
                break;
        }
    }

    //artifact has been "dropped"
    public void DroppedArtifact()
    {
        transform.position = new Vector3(playerThatOwns.transform.position.x, playerThatOwns.transform.position.y +1f, playerThatOwns.transform.position.z);
        playerThatOwns = null;
        isCarried = false;
        //scale size back up
        transform.localScale = normalscale;
    }

    /// <returns>
    /// Returns the player ID currently carrying the artifact.
    /// </returns>
    public int GetOwnerID()
    {
        return ownerID;
    }
}