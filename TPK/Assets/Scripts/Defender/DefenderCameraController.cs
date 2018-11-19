using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderCameraController : MonoBehaviour
{
    public IUnityService unityService;

    // Use this for initialization
    void Start()
    {
        if (unityService == null)
        {
            unityService = new UnityService();
        }
    }

    // Update is called once per frame
    void Update()
    {
        var x = unityService.GetAxis("Horizontal") * unityService.GetDeltaTime() * 50.0f;
        var z = unityService.GetAxis("Vertical") * unityService.GetDeltaTime() * 50.0f;

        transform.Translate(x, z, 0);
    }
}
