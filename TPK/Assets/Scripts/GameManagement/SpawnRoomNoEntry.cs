using UnityEngine;

/// <summary>
/// Blocks all objects from entering the area, except for players.
/// Attached to all spawn rooms on the map.
/// </summary>
public class SpawnRoomNoEntry : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Allow players to pass through
        if (collision.gameObject.tag == "Player")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}
