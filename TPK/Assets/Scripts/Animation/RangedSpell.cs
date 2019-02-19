using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSpell : MonoBehaviour
{
    public GameObject Target;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Target != null)
        {
            Vector3 targetPosition = new Vector3(Target.transform.position.x,
                Target.transform.position.y, Target.transform.position.z);
            //Projectile go toward enemy
            this.transform.LookAt(targetPosition);
            //Distance between projectile and enemy
            float distance2 = Vector3.Distance(Target.transform.position, this.transform.position);
            //if projecitle hit enemy
            if (distance2 > 2.0f)
            {
                transform.Translate(Vector3.forward * 30.0f * Time.deltaTime);
            }
            else
            {
                HitTarget();
            }

        }
    }
    void HitTarget()
    {
        Destroy(this.gameObject);
    }
}
