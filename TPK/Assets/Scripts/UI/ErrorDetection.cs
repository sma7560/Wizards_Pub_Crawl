using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Used to locally instantiate error popups upon an error being detected.
/// </summary>
public class ErrorDetection : MonoBehaviour
{
    private GameObject errorPopup;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Called when OK button is pressed on the error popup. Closes the error popup.
    /// </summary>
    public void CloseErrorPopup()
    {
        Destroy(errorPopup);
        errorPopup = null;
    }

    /// <summary>
    /// Instantiates an error popup.
    /// </summary>
    private void CreateErrorPopup()
    {
        if (errorPopup != null)
        {
            Debug.Log("There are two error popups. This should not happen!");
        }

        errorPopup = (GameObject)Instantiate(Resources.Load("Menu&UI Prefabs/ErrorPopup"));
    }

    /// <summary>
    /// When "Create Match" is pressed twice on the same IP.
    /// </summary>
    private void DetectNetworkStartHostError()
    {
        
    }
}
