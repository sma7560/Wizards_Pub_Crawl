using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Used to update dungeon status during dungeon level gameplay.
/// </summary>
public class DungeonController : MonoBehaviour
{
    // Menu objects
    public GameObject inGameMenu;
    public GameObject statsWindow;

    public IUnityService unityService;

    private PrephaseManager prephaseManager;

    /// <summary>
    /// Initialize variables.
    /// </summary>
    void Start()
    {
        if (unityService == null)
        {
            unityService = new UnityService();
        }
    }

    /// <summary>
    /// Updates dungeon status and listens for menu toggling.
    /// </summary>
    void Update()
    {
        // Initialize pre-phase manager if it is not already initialized
        if (prephaseManager == null && GameObject.FindGameObjectWithTag("MatchManager") != null)
        {
            prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();
        }

        // Toggles in-game menu when Esc key pressed
        if (unityService.GetKeyDown(KeyCode.Escape))
        {
            inGameMenu.SetActive(!inGameMenu.activeSelf);
        }

        // Toggles stats window when K key pressed
        if (prephaseManager != null && !prephaseManager.IsCurrentlyInPrephase() && unityService.GetKeyDown(KeyCode.K))
        {
            statsWindow.SetActive(!statsWindow.activeSelf);
        }

        UpdateEnemyHealthBars();
    }

    /// <summary>
    /// Exits the match and returns to the main menu.
    /// </summary>
    public void QuitMatch()
    {
        Debug.Log("MATCH QUIT");
        GameObject.Find("NetworkManagerV2").GetComponent<NetworkManagerExtension>().StopHost();
        SceneManager.LoadScene(SceneManager.GetSceneByName("Menu").buildIndex);
    }

    /// <summary>
    /// Sets prephaseManager once MatchManager object is loaded.
    /// </summary>
    private IEnumerator SetupPrephaseManager()
    {
        yield return new WaitUntil(IsMatchManagerFound);
        prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();
    }

    /// <returns>
    /// Returns whether or not MatchManager object has been found.
    /// </returns>
    private bool IsMatchManagerFound()
    {
        return (GameObject.FindGameObjectWithTag("MatchManager") != null);
    }

    // Updates the statuses of all enemy health bars currently in the scene
    private void UpdateEnemyHealthBars()
    {
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy"); // Find all enemy objects

        for (int i = 0; i < enemyObjects.Length; i++)
        {
            EnemyStats enemyStats = enemyObjects[i].GetComponent<EnemyStats>(); // Get enemy stats
            Transform healthBar = enemyObjects[i].transform.Find("HealthBar");

            // Update health bar image
            Image healthImage = healthBar.Find("Health").GetComponent<Image>();
            healthImage.fillAmount = (float)enemyStats.GetCurrentHealth() / (float)enemyStats.maxHealth;

            // Update health bar text
            TextMeshProUGUI healthText = healthBar.Find("HealthText").GetComponent<TextMeshProUGUI>();
            healthText.text = enemyStats.GetCurrentHealth() + "/" + enemyStats.maxHealth;

            // Keep health bar facing towards camera
            Camera camera = Camera.current;
            if (camera != null)
            {
                Vector3 v = camera.transform.position - healthBar.position;
                v.x = v.z = 0.0f;
                healthBar.LookAt(camera.transform.position - v);    // Fix position towards of camera
                healthBar.rotation = (camera.transform.rotation);   // Fix rotation towards camera
            }
        }
    }
}