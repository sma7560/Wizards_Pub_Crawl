using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attached to in-game menu prefab.
/// </summary>
public class InGameMenu : MonoBehaviour
{
    /// <summary>
    /// On click functionality for quit button
    /// </summary>
    public void QuitButton()
    {
        GameObject.Find("EventSystem").GetComponent<DungeonController>().QuitMatch();
    }
}
