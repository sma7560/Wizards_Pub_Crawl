using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// HeroCameraController: camera spawned when hero prefab is spawned (HeroCamera is instantiated in HeroController.cs)
// Cameras should only be spawned locally?
public class HeroCameraController : MonoBehaviour
{

    private Transform targetTransform;
    private float smoothspeed = 0.5f;
    private Vector3 offset = new Vector3(0, 8, -5);

    // Use this for initialization
    void Start()
    {
        this.transform.position = offset;
    }

    void Update()
    {
        Vector3 desired = targetTransform.position + offset;
        //Vector3 smoothed = Vector3.Lerp(this.transform.position, desired, smoothspeed);
        transform.position = desired;   // desired does not cause camera stutter when character is moving -Susan
    }

    public void setTarget(Transform target)
    {
        targetTransform = target;
    }
}
