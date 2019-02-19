using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildDeactivate : MonoBehaviour
{

    // Use this for initialization
    int childrenMarker;
    void Start()
    {
        childrenMarker = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.GetChild(childrenMarker).gameObject.SetActive(!transform.GetChild(childrenMarker++).gameObject.activeSelf);

        }
    }
}
