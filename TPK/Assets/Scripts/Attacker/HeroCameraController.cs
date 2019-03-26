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
    private Vector3 dungeonOffset = new Vector3(-5, 10, -5);
    private Quaternion dungeonRotation = Quaternion.Euler(50, 45, 0);
    private Vector3 prephaseOffset = new Vector3(0.1f, 3.0f, 2.5f);
    private Quaternion prephaseRotation = Quaternion.Euler(10, 180, 0);
    private Vector3 waitingRoomPosition = new Vector3(86.5f, 2.5f, 22f);
    private Quaternion waitingRoomRotation = Quaternion.Euler(10, 180, 0);

    // Use this for initialization
    void Start()
    {
        this.transform.position = dungeonOffset;
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
    }

    public void SetTarget(Transform target)
    {
        targetTransform = target;
    }
}
