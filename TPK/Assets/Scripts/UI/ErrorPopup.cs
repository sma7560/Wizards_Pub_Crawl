using UnityEngine;

/// <summary>
/// Holds behaviour for error popups.
/// </summary>
public class ErrorPopup : MonoBehaviour
{
    /// <summary>
    /// Behaviour for when the OK button is pressed on an error popup.
    /// Destroys this error popup.
    /// </summary>
    public void ClosePopup()
    {
        Destroy(gameObject);
    }
}
