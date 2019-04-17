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
    public AudioSource audioSource;

    // Menu & UI game objects
    private GameObject inGameMenu;
    private Button inGameMenuResumeButton;
    private GameObject statWindow;
    private GameObject scoreboard;

    // Buff sprites
    public Sprite atkBuff;
    public Sprite defBuff;
    public Sprite spdBuff;

    private PrephaseManager prephaseManager;
    private MatchManager matchManager;
    private HeroManager heroManager;

    /// <summary>
    /// Initialize variables.
    /// </summary>
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        CustomKeyBinding.SetupCustomKeyBindings();
    }

    void Update()
    {
        // Initialize managers if it is not already initialized
        if ((matchManager == null || prephaseManager == null || heroManager == null) &&
             GameObject.FindGameObjectWithTag("MatchManager") != null)
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
        UpdateBuffs();
    }

    /// <summary>
    /// Whether or not a menu is currently open.
    /// Used to avoid player input during gameplay while a menu is open.
    /// </summary>
    public bool IsMenuOpen()
    {
        if (inGameMenu.activeSelf || statWindow.activeSelf || scoreboard.activeSelf)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Exits the match and returns to the main menu.
    /// </summary>
    public void QuitMatch()
    {
        Debug.Log("MATCH QUIT");

        NetworkManagerExtension networkManager = GameObject.Find("NetworkManagerV2").GetComponent<NetworkManagerExtension>();

        if (networkManager != null)
        {
            networkManager.StopHost();
            networkManager.SetDoNotDisplayTimeoutError(true);
            SceneManager.LoadScene(SceneManager.GetSceneByName("Menu").buildIndex);
            networkManager.RemoveAllAnnouncements();
        }
        else
        {
            Debug.Log("ERROR: Could not find NetworkManagerExtension script!");
        }
    }

    /// <summary>
    /// Listens for toggling of the following menu/windows:
    ///   * In-game Menu
    ///   * Stats window
    ///   * Scoreboard
    /// </summary>
    private void ToggleUI()
    {
        // Toggles in-game menu
        if (Input.GetKeyDown(CustomKeyBinding.GetInGameMenuKey()))
        {
            inGameMenu.SetActive(!inGameMenu.activeSelf);

            // Reset in-game menu if it is now inactive
            if (!inGameMenu.activeSelf)
            {
                inGameMenuResumeButton = inGameMenu.transform.Find("Panel").Find("OptionsMenu").Find("ResumeButton").GetComponent<Button>();
                inGameMenuResumeButton.onClick.Invoke();
            }
        }

        // UI elements which can only be toggled during dungeon phase
        if (prephaseManager != null && !prephaseManager.IsCurrentlyInPrephase())
        {
            // Toggles stats window
            if (Input.GetKeyDown(CustomKeyBinding.GetStatWindowKey()))
            {
                statWindow.SetActive(!statWindow.activeSelf);
            }

            // Toggles scoreboard
            if (Input.GetKeyDown(CustomKeyBinding.GetScoreboardKey()))
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
        if (prephaseManager == null) return;

        if (prephaseManager.IsCurrentlyInPrephase() && audioSource.clip != music[0])
        {
            // If in pre-phase and pre-phase music is not already playing, play it
            audioSource.clip = music[0];
            audioSource.volume = AudioManager.GetBgVolume();
            audioSource.Play();
        }
        else if (!prephaseManager.IsCurrentlyInPrephase() && audioSource.clip != music[1])
        {
            // If in dungeon phase and dungeon music is not already playing, play it
            audioSource.clip = music[1];
            audioSource.volume = AudioManager.GetBgVolume();
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
            EnemyModel enemyStats = enemy.GetComponent<EnemyModel>();
            Transform healthBar = enemy.transform.Find("HealthBar");

            if (enemyStats != null)
            {
                UpdateHealthBar(healthBar, enemyStats.GetCurrentHealth(), enemyStats.GetMaxHealth());
            }
        }

        // Update all player health bars
        foreach (GameObject player in playerObjects)
        {
            HeroModel playerStats = player.GetComponent<HeroModel>();
            Transform healthBar = player.transform.Find("HealthBar");

            // Disable the health bar of current player
            if (matchManager != null &&
                healthBar != null &&
                player.GetComponent<HeroModel>().GetPlayerId() == matchManager.GetPlayerId())
            {
                healthBar.gameObject.SetActive(false);
            }

            if (playerStats != null)
            {
                UpdateHealthBar(healthBar, playerStats.GetCurrentHealth(), playerStats.GetMaxHealth());
            }
        }
    }

    /// <summary>
    /// Updates the given health bar according to the given stats.
    /// </summary>
    /// <param name="healthBar">Transform holding all health bar information to be updated.</param>
    /// <param name="currentHealth">Current health of this object.</param>
    /// /// <param name="maxHealth">Max health of this object.</param>
    private void UpdateHealthBar(Transform healthBar, int currentHealth, int maxHealth)
    {
        if (healthBar == null) return;

        // Update health bar image to appropriate fill level depending on current health
        Image healthImage = healthBar.Find("Health").GetComponent<Image>();
        healthImage.fillAmount = (float)currentHealth / (float)maxHealth;
        RenderOnTop(healthImage);

        // Update health bar text with value of current health
        TextMeshProUGUI healthText = healthBar.Find("HealthText").GetComponent<TextMeshProUGUI>();
        healthText.text = currentHealth + "/" + maxHealth;
        RenderOnTop(healthText);

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
            // Fix rotation towards camera
            t.rotation = Quaternion.LookRotation(camera.transform.forward);
        }
    }

    /// <summary>
    /// Checks current prephase status, and sets up the prephase/player UI as needed.
    /// </summary>
    private void SetupUI()
    {
        if (prephaseManager == null) return;

        if (inGameMenu == null)
        {
            // Initialize in-game UI
            inGameMenu = Instantiate(Resources.Load("Menu&UI Prefabs/In-Game Menu") as GameObject);
            inGameMenu.SetActive(false);
        }

        if (prephaseManager.GetState() == PrephaseManager.PrephaseState.WaitingForPlayers &&
             GameObject.FindGameObjectWithTag("WaitingRoomUI") == null)
        {
            // Initialize waiting room UI
            Destroy(GameObject.FindGameObjectWithTag("PlayerUI"));          // Destroy player UI
            Destroy(GameObject.FindGameObjectWithTag("PrephaseUI"));        // Destroy prephase UI
            Instantiate(Resources.Load("Menu&UI Prefabs/WaitingRoom"));     // Start waiting room UI
        }
        else if (prephaseManager.GetState() == PrephaseManager.PrephaseState.RoomFull &&
                  GameObject.FindGameObjectWithTag("PrephaseUI") == null)
        {
            // Initialize prephase UI
            Destroy(GameObject.FindGameObjectWithTag("PlayerUI"));          // Destroy player UI
            Destroy(GameObject.FindGameObjectWithTag("WaitingRoomUI"));     // Destroy waiting room UI
            Instantiate(Resources.Load("Menu&UI Prefabs/PrephaseScreen"));  // Start prephase UI
        }
        else if (!prephaseManager.IsCurrentlyInPrephase() &&
                  GameObject.FindGameObjectWithTag("PlayerUI") == null)
        {
            // Initialize player UI
            Destroy(GameObject.FindGameObjectWithTag("PrephaseUI"));        // Destroy prephase UI
            Destroy(GameObject.FindGameObjectWithTag("WaitingRoomUI"));     // Destroy waiting room UI
            Instantiate(Resources.Load("Menu&UI Prefabs/PlayerUI"));        // Start player UI

            // Initialize stat window and scoreboard
            statWindow = Instantiate(Resources.Load("Menu&UI Prefabs/StatWindow") as GameObject);
            scoreboard = Instantiate(Resources.Load("Menu&UI Prefabs/Scoreboard") as GameObject);
            statWindow.SetActive(false);
            scoreboard.SetActive(false);
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
            RenderOnTop(nameText);

            // Set the name
            nameText.text = "Player " + playerId.ToString();
            nameText.color = heroManager.GetPlayerColour(playerId);

            // Keep name facing towards camera
            SetFacingTowardsCamera(name);
        }
    }

    /// <summary>
    /// Locally updates all player buffs in the scene.
    /// </summary>
    private void UpdateBuffs()
    {
        // Get all player objects
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");

        // Update buffs of each player
        foreach (GameObject player in playerObjects)
        {
            // Get necessary components
            Transform buffs = player.transform.Find("Buffs"); // Transform that holds all buff-related info
            Image[] buffImages = new Image[3];
            buffImages[0] = buffs.Find("Buff1").GetComponent<Image>();
            buffImages[1] = buffs.Find("Buff2").GetComponent<Image>();
            buffImages[2] = buffs.Find("Buff3").GetComponent<Image>();
            HeroModel heroModel = player.GetComponent<HeroModel>();

            // Render buffs always on top
            RenderOnTop(buffImages[0]);
            RenderOnTop(buffImages[1]);
            RenderOnTop(buffImages[2]);

            // Check if player has buffs; set sprite icon for buff if they do
            int numBuffs = 0;
            if (heroModel.IsAttackBuffed())
            {
                buffImages[numBuffs].sprite = atkBuff;
                buffImages[numBuffs].color = new Color(1f, 1f, 1f, 1f);
                numBuffs++;
            }
            if (heroModel.IsDefBuffed())
            {
                buffImages[numBuffs].sprite = defBuff;
                buffImages[numBuffs].color = new Color(1f, 1f, 1f, 1f);
                numBuffs++;
            }
            if (heroModel.IsSpeedBuffed())
            {
                buffImages[numBuffs].sprite = spdBuff;
                buffImages[numBuffs].color = new Color(1f, 1f, 1f, 1f);
                numBuffs++;
            }

            // Disable unneeded buff images
            for (int i = numBuffs; i < 3; i++)
            {
                // Sets opacity to transparent
                buffImages[i].color = new Color(1f, 1f, 1f, 0f);
            }

            // Keep buff icons facing towards camera
            SetFacingTowardsCamera(buffs);
        }
    }

    /// <summary>
    /// Sets the image to always render on top.
    /// </summary>
    /// <param name="img">Image to always render on top.</param>
    private void RenderOnTop(Image img)
    {
        Material mat = img.materialForRendering;
        mat.SetInt("unity_GUIZTestMode", (int)UnityEngine.Rendering.CompareFunction.Always);
        img.material = mat;
    }

    /// <summary>
    /// Sets the text to always render on top.
    /// </summary>
    /// <param name="text">Text to always render on top.</param>
    private void RenderOnTop(TextMeshProUGUI text)
    {
        Material mat = text.materialForRendering;
        mat.SetInt("unity_GUIZTestMode", (int)UnityEngine.Rendering.CompareFunction.Always);
        text.material = mat;
    }

    /// <summary>
    /// End BGM music.
    /// </summary>
    public void endMusic()
    {
        audioSource.Stop();
    }
}