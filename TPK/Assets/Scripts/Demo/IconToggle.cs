using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconToggle : MonoBehaviour
{
    //GameObject iconPanel;
    // Use this for initialization

    public void ToggleActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

}
