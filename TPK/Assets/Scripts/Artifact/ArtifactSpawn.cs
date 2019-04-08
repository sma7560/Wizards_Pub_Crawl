using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Spawns the artifact randomly on the map.
/// Attached to EventSystem in DungeonLevel scene.
/// </summary>
public class ArtifactSpawn : NetworkBehaviour
{
    public IUnityService unityService;

    public GameObject artifact;             // Artifact prefab to spawn
    private GameObject[] spawnLocations;    // List of locations for artifacts to spawn

    void Start()
    {
        if (unityService == null)
        {
            unityService = new UnityService();
        }

        spawnLocations = GameObject.FindGameObjectsWithTag("ArtifactSpawn");

        SpawnArtifactRandom();

    }

    /// <summary>
    /// Chooses a random location to spawn the artifact.
    /// Called at beginning of game, and whenever an artifact is returned to a base.
    /// </summary>
    public void SpawnArtifactRandom()
    {
        if (!isServer) return;

        int i = Random.Range(0, spawnLocations.Length);
        Vector3 location = spawnLocations[i].transform.position;
        SpawnArtifact(location, Quaternion.identity);
    }

    /// <summary>
    /// Spawns the artifact at the specified location.
    /// </summary>
    /// <param name="location">Location to spawn the artifact at.</param>
    /// <param name="rotation">Starting rotation of the artifact.</param>
    private void SpawnArtifact(Vector3 location, Quaternion rotation)
    {
        Debug.Log("A new artifact has appeared at " + location.x + ", " + location.y + ", " + location.z);

        GameObject tempArtifact;
        tempArtifact = unityService.Instantiate(artifact, location, rotation);
        NetworkServer.Spawn(tempArtifact);
    }
}
