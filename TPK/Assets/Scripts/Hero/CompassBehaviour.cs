using System;
using UnityEngine;

/// <summary>
/// Controls the objective pointer/compass.
/// </summary>
public class CompassBehaviour : MonoBehaviour
{
    private Transform objective;
    private float dist;
    private Vector3 spawn;
    
    void Start()
    {
        // grabs the spawn room the player is in during the prephase
        spawn = transform.position;
    }

    /// <summary>
    /// Points to the objective when not carried, points to spawn room when it is picked up.
    /// </summary>
    void Update()
    {
		try
		{
	        objective = GameObject.FindGameObjectWithTag("Artifact").transform;
	        dist = Mathf.Abs(Vector3.Distance(objective.position, transform.position));

	        if (dist >= 4)
	        {
	            transform.LookAt(objective);
	        }
	        else
	        {
	            transform.LookAt(spawn);
	        }
		} catch (NullReferenceException e)
		{
			Debug.Log (e);
		}
    }
}
