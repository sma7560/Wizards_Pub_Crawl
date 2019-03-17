using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the objective pointer
/// </summary>
public class CompassBehaviour : MonoBehaviour {

	private Transform objective;
	private float dist;
	private Vector3 spawn;


	//grabs the spawn room the player is in during the prephase
	void Start () {
		spawn = transform.position;
	}
	
	//points to the objective when not carried, points to spawn room when it is picked up
	void Update () {
		objective = GameObject.FindGameObjectWithTag ("Artifact").transform;
		dist = Mathf.Abs( Vector3.Distance (objective.position, transform.position));

		if (dist >= 3)
			transform.LookAt (objective);
		else
			transform.LookAt (spawn);
	}
}
