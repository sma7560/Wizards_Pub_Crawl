using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Holds behaviour for error popups.
/// </summary>
public class ErrorPopup : MonoBehaviour
{
    /// <summary>
    /// Behaviour for when the OK button is pressed on an error popup.
    /// </summary>
    public void ClosePopup()
    {
        Destroy(gameObject);
    }
}
