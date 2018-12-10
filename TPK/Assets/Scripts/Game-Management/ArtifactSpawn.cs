using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ArtifactSpawn : NetworkBehaviour {

	GameObject[] spawnlocations;		//list of valid locations for artifacts to spawn, to be autofilled

	public GameObject artifact;
	public IUnityService unityService;

	int maxArtifacts;					//maximum number of artifacts that will appear at once (n players - 1)

	// Use this for initialization
	void Start () {
		maxArtifacts = 1;

		spawnlocations = GameObject.FindGameObjectsWithTag ("ArtifactSpawn");

		if (unityService == null) {
			unityService = new UnityService ();
		}

		SpawnArifactRandom ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//spawns artifact at a random spawn location, to be called when an artifact is picked up
	public void SpawnArifactRandom(){
		int i = Random.Range (0, spawnlocations.Length);
		Vector3 location = spawnlocations [i].transform.position;
		CmdSpawnArtifact(location, Quaternion.identity);
	}

	//commands to communicate to the server
	[Command]
	private void CmdSpawnArtifact(Vector3 location, Quaternion rotation){
		GameObject tempArtifact;
		tempArtifact = unityService.Instantiate(artifact, location, rotation);
		NetworkServer.Spawn (tempArtifact);
	}
}
