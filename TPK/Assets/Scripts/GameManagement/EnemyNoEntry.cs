using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Function to allow player to pass through invisible wall that prevents enemies from entering
public class EnemyNoEntry : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        //allows players to pass through
        if (collision.gameObject.tag == "Player")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}
