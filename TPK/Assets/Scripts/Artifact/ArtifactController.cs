using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// Contains logic regarding the artifact.
/// Attached to the Artifact prefab.
/// </summary>
public class ArtifactController : NetworkBehaviour
{
    // Artifact info
    [SerializeField] private bool isCarried;             // whether or not the artifact is currently being carried by a player
    private GameObject playerThatOwns;  // if artifact held, the player that is currently holding the artifact
    [SerializeField] private int ownerID;                // if artifact held, the player id of the player that is currently holding the artifact
    private Vector3 ownerSpawn;         // the spawn location of the last player that held this artifact
    private RarityType rarity;

    // Player info
    private int playerBaseSpeed;        // base speed of player carrying the artifact
    private int playerSlowSpeed;	    // 25% move speed reduction to be applied on pickup

    // Artifact size
    private Vector3 smallScale = new Vector3(1.25f, 1.25f, 1.25f);  // size used when carried (smaller)
    private Vector3 normalScale = new Vector3(2f, 2f, 2f);	        // size used when artifact is on the ground (larger)
    [SyncVar(hook = "SetScale")]private Vector3 currentScale;
	// Match Manager for player ID and announcements
	private GameObject matchManager;

	private NavMeshAgent agent;
	private GameObject[] spawnLocations;
	private int originalSpot;
	private int newSpot;

    /// <summary>
    /// Rarity of the artifact.
    /// </summary>
    public enum RarityType
    {
        Common,
        Rare,
        Epic,
        Legendary
    }

    /// <summary>
    /// Initialize variables.
    /// </summary>
    void Start()
    {
        ownerID = -1;
        transform.localScale = normalScale;
        rarity = RarityType.Common;

		matchManager = GameObject.FindGameObjectWithTag ("MatchManager");
		agent = GetComponent<NavMeshAgent> ();

		//navmesh agent isolated to Host
		if (!isServer) 
		{
			agent.enabled = false;
			return;
		}
		spawnLocations = GameObject.FindGameObjectsWithTag("ArtifactSpawn");

		//picking destination, can't be the same one as before
		newSpot = Random.Range(0, spawnLocations.Length);
		while (newSpot == originalSpot)
			newSpot = Random.Range(0, spawnLocations.Length);

		originalSpot = newSpot;
		agent.SetDestination (spawnLocations [newSpot].transform.position);
    }

    void Update()
    {
        if (!isServer) return;
        // if being carried, update location of artifact to where the carrier is
		if (isCarried) 
		{
			transform.position = new Vector3 (playerThatOwns.transform.position.x, playerThatOwns.transform.position.y + 3.5f, playerThatOwns.transform.position.z);
			if (playerThatOwns.GetComponent<HeroModel> ().IsKnockedOut ())
			{
                // player is knocked out, so he drops the artifact
                Debug.Log("Player " + ownerID + " died");
				DropArtifact ();
			}
		} else
			patrol ();
    }

    /// <summary>
    /// Checks the following cases through collision detection:
    ///     1. player picks up artifact
    ///     2. player returns artifact to spawn
    /// </summary>
    /// <param name="col">Collider that artifact has collided with.</param>
    private void OnTriggerEnter(Collider col)
    {
		if (!isServer)
			return;

        switch (col.gameObject.transform.tag)
        {
            case ("Player"):
                // Check case where player picks up artifact
                // Ensure that artifact is not already carried and player is not knocked out
                if (!isCarried && !col.GetComponent<HeroModel>().IsKnockedOut())
                {
					

                    // Make object float above character model's head
                    currentScale = smallScale;
                    //transform.localScale = smallScale;
                    playerThatOwns = col.gameObject;
					playerThatOwns.GetComponent<HeroModel> ().drunk (true);	

					//disable navmesh agent
					agent.enabled = false;

                    // Set owner ID and spawn point
                    ownerID = playerThatOwns.GetComponent<HeroModel>().GetPlayerId();
                    ownerSpawn = matchManager.GetComponent<HeroManager>().GetSpawnLocationOfPlayer(ownerID);
                    isCarried = true;
                    playerThatOwns.GetComponent<PlayerSoundController>().PlayArtifactSound();

                    // Slow down the player on pickup
                    playerBaseSpeed = playerThatOwns.GetComponent<HeroModel>().GetBaseMoveSpeed();
                    playerSlowSpeed = (int)(playerBaseSpeed * 0.25);
                    playerThatOwns.GetComponent<HeroModel>().SetCurrentMoveSpeed(
                        playerThatOwns.GetComponent<HeroModel>().GetCurrentMoveSpeed() - playerSlowSpeed);

                    // Broadcast that player has acquired the artifact
                    matchManager.GetComponent<AnnouncementManager>().BroadcastAnnouncementAleAcquired(ownerID);

					
                }
                break;
            case ("SpawnRoom"):
                // Checks case where player enters scoring location (spawn point)
                if (isCarried)
                {
                    Debug.Log("Scoring for player: " + ownerID);
                    // Ensure that this spawn is the right one for the player carrying the artifact
                    if (Vector3.Distance(transform.position, ownerSpawn) <= 10)
                    {
						playerThatOwns.GetComponent<HeroModel> ().drunk (false);
						

                        // Undo character slowdown
                        playerThatOwns.GetComponent<HeroModel>().SetCurrentMoveSpeed(
                            playerThatOwns.GetComponent<HeroModel>().GetCurrentMoveSpeed() + playerSlowSpeed);
                        playerBaseSpeed = 0;
                        playerSlowSpeed = 0;

                        //play sound for succesfully retrieving artifact
                        playerThatOwns.GetComponent<PlayerSoundController>().PlayArtifactSound();

                        // Broadcast that player has scored the artifact
                        matchManager.GetComponent<AnnouncementManager>().BroadcastAnnouncementAleScored(ownerID);

                        // Increase the player's score
                        playerThatOwns.GetComponent<HeroModel>().IncreaseScore(GetScore());

						

                        // Spawn another artifact
                        GameObject.FindGameObjectWithTag("EventSystem").GetComponent<ArtifactSpawn>().SpawnArtifactRandom();

                        // Destroy itself
                        Destroy(gameObject);
                    }
                }
                break;
        }
    }

    private void SetScale(Vector3 scale) {
        currentScale = scale;
        transform.localScale = scale;

    }

    /// <summary>
    /// Called when the artifact is dropped.
    /// </summary>
    private void DropArtifact()
    {
		if (!isServer) return;


        agent.enabled = true;
        // Broadcast the announcement that artifact is dropped
        matchManager.GetComponent<AnnouncementManager>().BroadcastAnnouncementAleDropped(ownerID);

        // Set position of artifact on the ground where player has died
        transform.position = new Vector3(playerThatOwns.transform.position.x, playerThatOwns.transform.position.y + 1f, playerThatOwns.transform.position.z);

        // Undo character slowdown
        playerThatOwns.GetComponent<HeroModel>().SetCurrentMoveSpeed(
            playerThatOwns.GetComponent<HeroModel>().GetCurrentMoveSpeed() + playerSlowSpeed);
        playerBaseSpeed = 0;
        playerSlowSpeed = 0;
        playerThatOwns.GetComponent<PlayerSoundController>().PlayDeathSound();

		playerThatOwns.GetComponent<HeroModel> ().drunk (false);

        // Reset owning player variables
        playerThatOwns = null;
        ownerID = -1;
        isCarried = false;

        // Scale size of artifact back up
        //transform.localScale = normalScale;
        currentScale = normalScale;
    }

	private void patrol()
	{
		if (agent.enabled && agent.remainingDistance <= 1)
		{
			//picking new spot to go to
			//can't be 2 same spots in a row
			newSpot = Random.Range (0, spawnLocations.Length);
			while (newSpot == originalSpot)
				newSpot = Random.Range (0, spawnLocations.Length);

			originalSpot = newSpot;
			agent.SetDestination (spawnLocations [newSpot].transform.position);
		}

	}

    /// <returns>
    /// Returns the player ID currently carrying the artifact.
    /// </returns>
    public int GetOwnerID()
    {
        return ownerID;
    }

    /// <returns>
    /// Returns the amount of score the artifact will reward based on its rarity.
    /// </returns>
    private int GetScore()
    {
        switch (rarity)
        {
            case RarityType.Common:
                return 100;
            case RarityType.Epic:
                return 250;
            case RarityType.Legendary:
                return 500;
            case RarityType.Rare:
                return 1000;
            default:
                Debug.Log("ERROR: Invalid rarity type of artifact!");
                return 0;
        }
    }

	public void Index(int index)
	{
		originalSpot = index;
	}
		
}