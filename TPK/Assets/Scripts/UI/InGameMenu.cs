using UnityEngine;

/// <summary>
/// Behaviour for In-game Menu.
/// Attached to the In-game Menu prefab.
/// </summary>
public class InGameMenu : MonoBehaviour
{
    /// <summary>
    /// On-click functionality for the quit button.
    /// </summary>
    public void QuitButton()
    {
        GameObject.Find("EventSystem").GetComponent<DungeonController>().QuitMatch();
    }
}
