using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// HeroCameraController: camera spawned when hero prefab is spawned (HeroCamera is instantiated in HeroController.cs)
// Cameras should only be spawned locally?
public class HeroCameraController : MonoBehaviour
{

    private PrephaseManager prephaseManager;
    private Transform targetTransform;
    private Vector3 dungeonOffset = new Vector3(0, 8, -5);
    private Quaternion dungeonRotation = Quaternion.Euler(45, 0, 0);
    private Vector3 prephaseOffset = new Vector3(0.1f, 1.5f, 2.5f);
    private Quaternion prephaseRotation = Quaternion.Euler(10, 180, 0);

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

        if (prephaseManager.IsCurrentlyInPrephase())
        {
            desiredPosition = targetTransform.position + prephaseOffset;
            desiredRotation = prephaseRotation;
            GetComponent<Camera>().orthographic = false;
        }
        else
        {
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
