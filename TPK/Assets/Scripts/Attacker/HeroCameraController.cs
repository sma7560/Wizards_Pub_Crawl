using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serves as the player's camera during pre-phase and dungeon phase.
/// Camera spawned locally when hero prefab is spawned.
/// HeroCamera is instantiated in HeroController.cs.
/// </summary>
public class HeroCameraController : MonoBehaviour
{
    private PrephaseManager prephaseManager;
    private Transform targetTransform;

    // Camera positioning
    private Vector3 dungeonOffset = new Vector3(-10, 20, -10);
    private Quaternion dungeonRotation = Quaternion.Euler(50, 45, 0);
    private Vector3 prephaseOffset = new Vector3(0.1f, 3.0f, 2.5f);
    private Quaternion prephaseRotation = Quaternion.Euler(10, 180, 0);
    private Vector3 waitingRoomPosition = new Vector3(85f, 2.5f, 22f);
    private Quaternion waitingRoomRotation = Quaternion.Euler(10, 180, 0);
	private Camera cam;

    // Use this for initialization
    void Start()
    {
        this.transform.position = dungeonOffset;
		cam = GetComponent<Camera> ();
        prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();
    }

    void Update()
    {
        if (targetTransform == null) return;

        Vector3 desiredPosition;
        Quaternion desiredRotation;

        if (prephaseManager.GetState() == PrephaseManager.PrephaseState.RoomFull)
        {
            // Pre-phase
            targetTransform.rotation = Quaternion.Euler(0, 0, 0);   // reset hero to face towards camera
            desiredPosition = targetTransform.position + prephaseOffset;
            desiredRotation = prephaseRotation;
            GetComponent<Camera>().orthographic = false;
        }
        else if (prephaseManager.GetState() == PrephaseManager.PrephaseState.WaitingForPlayers)
        {
            // Waiting Room
            desiredPosition = waitingRoomPosition;
            desiredRotation = waitingRoomRotation;
            GetComponent<Camera>().orthographic = false;
        }
        else
        {
            // Dungeon phase
            desiredPosition = targetTransform.position + dungeonOffset;
            desiredRotation = dungeonRotation;
            GetComponent<Camera>().orthographic = true;
        }

        transform.position = desiredPosition;
        transform.rotation = desiredRotation;

		//mousewheel zoom and reset
		if (Input.GetAxis ("Mouse ScrollWheel") > 0 && cam.orthographicSize > 5)
			cam.orthographicSize -= 1;
		else if (Input.GetAxis ("Mouse ScrollWheel") < 0 && cam.orthographicSize < 16)
			cam.orthographicSize += 1;
		else if (Input.GetMouseButton (2))
			cam.orthographicSize = 12;
    }

    public void SetTarget(Transform target)
    {
        targetTransform = target;
    }
}
