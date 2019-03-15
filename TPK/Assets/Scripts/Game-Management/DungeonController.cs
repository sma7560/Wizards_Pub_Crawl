using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Updates general dungeon status during dungeon level gameplay.
/// </summary>
public class DungeonController : MonoBehaviour
{
    // Background music (during dungeon level scene)
    public AudioClip[] music;
    private AudioSource audioSource;

    // Menu & UI game objects
    public GameObject inGameMenu;
    public Button inGameMenuResumeButton;
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

        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        // Initialize pre-phase manager if it is not already initialized
        if (prephaseManager == null && GameObject.FindGameObjectWithTag("MatchManager") != null)
        {
            prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();
        }

        // Call individual update functions
        ToggleUI();
        UpdateEnemyHealthBars();
        UpdateMusic();
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
    /// Listens for toggling of the in-game menu and stats window.
    /// In-game menu can be accessed with 'ESC' key.
    /// Stats window can be accessed with 'K' key.
    /// </summary>
    private void ToggleUI()
    {
        // Toggles in-game menu when 'ESC' key pressed
        if (unityService.GetKeyDown(KeyCode.Escape))
        {
            inGameMenu.SetActive(!inGameMenu.activeSelf);
            
            // Reset in-game menu if it is now inactive
            if (!inGameMenu.activeSelf)
            {
                inGameMenuResumeButton.onClick.Invoke();
            }
        }

        // Toggles stats window when 'K' key pressed
        if (prephaseManager != null && !prephaseManager.IsCurrentlyInPrephase() && unityService.GetKeyDown(KeyCode.K))
        {
            statsWindow.SetActive(!statsWindow.activeSelf);
        }
    }

    /// <summary>
    /// Start music depending on pre-phase status.
    /// </summary>
    private void UpdateMusic()
    {
        if (prephaseManager != null)
        {
            if (prephaseManager.IsCurrentlyInPrephase() &&
                audioSource.clip != music[0])
            {
                // If in pre-phase and music is not already playing, play it
                audioSource.clip = music[0];
                audioSource.volume = AudioManager.GetVolume();
                audioSource.Play();
            }
            else if (!prephaseManager.IsCurrentlyInPrephase() &&
                      audioSource.clip != music[1])
            {
                // If in dungeon phase and music is not already playing, play it
                audioSource.clip = music[1];
                audioSource.volume = AudioManager.GetVolume();
                audioSource.Play();
            }
        }
    }

    /// <summary>
    /// Locally updates the values of all enemy health bars currently in the scene.
    /// </summary>
    private void UpdateEnemyHealthBars()
    {
        // Find all enemy objects
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

        // Loop through each enemy in the scene
        for (int i = 0; i < enemyObjects.Length; i++)
        {
            EnemyStats enemyStats = enemyObjects[i].GetComponent<EnemyStats>(); // Get enemy stats
            Transform healthBar = enemyObjects[i].transform.Find("HealthBar");  // GameObject that holds all enemy health bar info

            // Update health bar image to appropriate fill level depending on enemy's current health
            Image healthImage = healthBar.Find("Health").GetComponent<Image>();
            healthImage.fillAmount = (float)enemyStats.GetCurrentHealth() / (float)enemyStats.maxHealth;

            // Update health bar text with value of enemy's current health
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