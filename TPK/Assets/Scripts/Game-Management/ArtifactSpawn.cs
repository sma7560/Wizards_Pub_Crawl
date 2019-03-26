using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ArtifactSpawn : NetworkBehaviour
{
    GameObject[] spawnlocations;        //list of valid locations for artifacts to spawn, to be autofilled

    public GameObject artifact;
    public IUnityService unityService;

    private GameObject spawn;
    private MatchManager matchManager;

    //int maxArtifacts;                   //maximum number of artifacts that will appear at once (players - 1)

    // Use this for initialization
    void Start()
    {
        //maxArtifacts = 1;

        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        spawnlocations = GameObject.FindGameObjectsWithTag("ArtifactSpawn");

        if (unityService == null)
        {
            unityService = new UnityService();
        }

        SpawnArtifactRandom();

    }

    //spawns artifact at a random spawn location, to be called when an artifact is picked up
    public void SpawnArtifactRandom()
    {
        if (!isServer || matchManager.HasMatchEnded()) return;  // only called on server 

        int i = Random.Range(0, spawnlocations.Length);
        spawn = spawnlocations[i];
        Vector3 location = spawn.transform.position;
        SpawnArtifact(location, Quaternion.identity);
    }

    private void SpawnArtifact(Vector3 location, Quaternion rotation)
    {
        Debug.Log("A new artifact has appeared at " + location.x + ", " + location.y + ", " + location.z);
        GameObject tempArtifact;
        tempArtifact = unityService.Instantiate(artifact, location, rotation);
        NetworkServer.Spawn(tempArtifact);
    }
}
