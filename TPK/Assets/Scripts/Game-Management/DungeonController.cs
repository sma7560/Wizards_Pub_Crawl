using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Locally updates general dungeon status during dungeon level gameplay.
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
    public GameObject scoreboard;

    public IUnityService unityService;

    private PrephaseManager prephaseManager;
    private MatchManager matchManager;
    private HeroManager heroManager;

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
        // Initialize managers if it is not already initialized
        if ( (matchManager == null || prephaseManager == null || heroManager == null) &&
             GameObject.FindGameObjectWithTag("MatchManager") != null )
        {
            matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
            prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();
            heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();
        }

        // Call individual update functions
        SetupUI();
        ToggleUI();
        UpdateAllHealthBars();
        UpdateMusic();
        UpdatePlayerNames();
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
    /// Scoreboard can be accessed with 'TAB' key.
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

        // UI elements which can only be toggled during dungeon phase
        if (prephaseManager != null && !prephaseManager.IsCurrentlyInPrephase())
        {
            // Toggles stats window when 'K' key pressed
            if (unityService.GetKeyDown(KeyCode.K))
            {
                statsWindow.SetActive(!statsWindow.activeSelf);
            }

            // Toggles scoreboard when 'TAB' key pressed
            if (unityService.GetKeyDown(KeyCode.Tab))
            {
                scoreboard.SetActive(!scoreboard.activeSelf);
            }
        }
    }

    /// <summary>
    /// Start music depending on pre-phase status.
    /// </summary>
    private void UpdateMusic()
    {
        // Do not update music if prephase manager is not initialized
        if (prephaseManager == null) return;

        if (prephaseManager.IsCurrentlyInPrephase() && audioSource.clip != music[0])
        {
            // If in pre-phase and music is not already playing, play it
            audioSource.clip = music[0];
            audioSource.volume = AudioManager.GetVolume();
            audioSource.Play();
        }
        else if (!prephaseManager.IsCurrentlyInPrephase() && audioSource.clip != music[1])
        {
            // If in dungeon phase and music is not already playing, play it
            audioSource.clip = music[1];
            audioSource.volume = AudioManager.GetVolume();
            audioSource.Play();
        }
    }

    /// <summary>
    /// Locally updates the values of all health bars currently in the scene.
    /// </summary>
    private void UpdateAllHealthBars()
    {
        // Get all monster & player objects
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        // Update all monster health bars
        foreach (GameObject enemy in enemyObjects)
        {
            EnemyStats enemyStats = enemy.GetComponent<EnemyStats>(); // Get enemy stats
            Transform healthBar = enemy.transform.Find("HealthBar");  // Transform that holds all enemy health bar info

            UpdateHealthBar(healthBar, enemyStats);
        }

        // Update all player health bars
        foreach (GameObject player in playerObjects)
        {
            HeroModel playerStats = player.GetComponent<HeroModel>(); // get enemy player stats
            Transform healthBar = player.transform.Find("HealthBar");                   // Transform that holds all player health bar info

            // Disable the health bar of current player
            if (matchManager != null &&
                healthBar != null &&
                player.GetComponent<HeroModel>().GetPlayerId() == matchManager.GetPlayerId())
            {
                healthBar.gameObject.SetActive(false);
            }

            UpdateHealthBar(healthBar, playerStats);
        }
    }

    /// <summary>
    /// Updates the individual given health bar according to the given stats.
    /// </summary>
    /// <param name="healthBar">Transform holding all health bar information to be updated.</param>
    /// <param name="stats">Stats to which the health bar will be updated to accordingly.</param>
    private void UpdateHealthBar(Transform healthBar, HeroModel stats)
    {
        // Do nothing if either the health bar or stats given is null
        if (healthBar == null || stats == null)
        {
            return;
        }

        // Update health bar image to appropriate fill level depending on current health
        Image healthImage = healthBar.Find("Health").GetComponent<Image>();
        healthImage.fillAmount = (float)stats.GetCurrentHealth() / (float)stats.GetMaxHealth();

        // Update health bar text with value of current health
        TextMeshProUGUI healthText = healthBar.Find("HealthText").GetComponent<TextMeshProUGUI>();
        healthText.text = stats.GetCurrentHealth() + "/" + stats.GetMaxHealth();

        // Keep health bar facing towards camera
        SetFacingTowardsCamera(healthBar);
    }

    /// <summary>
    /// Updates the individual given health bar according to the given stats.
    /// </summary>
    /// <param name="healthBar">Transform holding all health bar information to be updated.</param>
    /// <param name="stats">Stats to which the health bar will be updated to accordingly.</param>
    private void UpdateHealthBar(Transform healthBar, CharacterStats stats)
    {
        // Do nothing if either the health bar or stats given is null
        if (healthBar == null || stats == null)
        {
            return;
        }

        // Update health bar image to appropriate fill level depending on current health
        Image healthImage = healthBar.Find("Health").GetComponent<Image>();
        healthImage.fillAmount = (float)stats.GetCurrentHealth() / (float)stats.maxHealth;

        // Update health bar text with value of current health
        TextMeshProUGUI healthText = healthBar.Find("HealthText").GetComponent<TextMeshProUGUI>();
        healthText.text = stats.GetCurrentHealth() + "/" + stats.maxHealth;

        // Keep health bar facing towards camera
        SetFacingTowardsCamera(healthBar);
    }

    /// <summary>
    /// Sets the given transform to always face towards the camera.
    /// </summary>
    /// <param name="t">Transform to face towards the camera.</param>
    private void SetFacingTowardsCamera(Transform t)
    {
        Camera camera = Camera.current;
        if (camera != null)
        {
            Vector3 v = camera.transform.position - t.position;
            v.x = v.z = 0.0f;
            t.LookAt(camera.transform.position - v);    // Fix position towards of camera
            t.rotation = (camera.transform.rotation);   // Fix rotation towards camera
        }
    }

    /// <summary>
    /// Checks current prephase status, and sets up the prephase/player UI as needed.
    /// </summary>
    private void SetupUI()
    {
        if (prephaseManager == null) return;

        if ( prephaseManager.GetState() == PrephaseManager.PrephaseState.WaitingForPlayers &&
             GameObject.FindGameObjectWithTag("WaitingRoomUI") == null )
        {
            // Initialize waiting room UI
            Destroy(GameObject.FindGameObjectWithTag("PlayerUI"));      // Destroy player UI
            Destroy(GameObject.FindGameObjectWithTag("PrephaseUI"));    // Destroy prephase UI
            Instantiate(Resources.Load("Menu&UI Prefabs/WaitingRoom")); // Start waiting room UI
        }
        else if ( prephaseManager.GetState() == PrephaseManager.PrephaseState.RoomFull &&
                  GameObject.FindGameObjectWithTag("PrephaseUI") == null )
        {
            // Initialize prephase UI
            Destroy(GameObject.FindGameObjectWithTag("PlayerUI"));          // Destroy player UI
            Destroy(GameObject.FindGameObjectWithTag("WaitingRoomUI"));     // Destroy waiting room UI
            Instantiate(Resources.Load("Menu&UI Prefabs/PrephaseScreen"));  // Start prephase UI
        }
        else if ( !prephaseManager.IsCurrentlyInPrephase() &&
                  GameObject.FindGameObjectWithTag("PlayerUI") == null )
        {
            // Initialize player UI
            Destroy(GameObject.FindGameObjectWithTag("PrephaseUI"));    // Destroy prephase UI
            Destroy(GameObject.FindGameObjectWithTag("WaitingRoomUI")); // Destroy waiting room UI
            Instantiate(Resources.Load("Menu&UI Prefabs/PlayerUI"));    // Start player UI
        }
    }

    /// <summary>
    /// Locally updates all the player names in the scene.
    /// </summary>
    private void UpdatePlayerNames()
    {
        if (heroManager == null) return;

        // Get all player objects
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        // Update name of each player
        foreach (GameObject player in playerObjects)
        {
            // Get necessary components
            int playerId = player.GetComponent<HeroModel>().GetPlayerId();
            Transform name = player.transform.Find("Name"); // Transform that holds all player name info
            TextMeshProUGUI nameText = name.Find("NameText").GetComponent<TextMeshProUGUI>();

            // Set the name
            nameText.text = "Player " + playerId.ToString();
            nameText.color = heroManager.GetPlayerColour(playerId);

            // Keep name facing towards camera
            SetFacingTowardsCamera(name);
        }
    }
}