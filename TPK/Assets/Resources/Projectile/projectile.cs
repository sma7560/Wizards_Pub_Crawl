/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour {
    public float moveSpeed = 3f;
    public int damage = 5;
    public bool collided = false;
    

    private void FixedUpdate()
    {
        if (collided)
        {
            Destroy(gameObject);
        }
        else
        {
            // Better way to move tranform
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
        }
    }
    private void Fire()
    {
        

        

        if (character != null)
        {
            character.TakeDamage(damage);
        }


        GetComponent<Collider2D>().enabled = false;
        collided = true;
    }
}*/
