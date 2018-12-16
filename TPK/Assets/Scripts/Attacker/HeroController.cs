using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// HeroController: script for controlling attacker/hero character.
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(CharacterCombat))]
[RequireComponent(typeof(Transform))]
public class HeroController : NetworkBehaviour
{
    public GameObject heroCam;
    public GameObject playerUI; // Player UI for dungeon crawling phase
    public bool localTest;      // for unit testing to allow the game to run locally

    private int playerId;         // id of the player
    private GameObject cam;

    // For setting up character direction
    private Camera view;
    private Plane ground;
    private float rayLength;
    private Vector3 pointToLookAt;

    private Rigidbody heroRigidbody;
    private bool isKnockedOut;
    private readonly int deathTimer = 4; //default death timer

    private PrephaseManager prephaseManager;
    public IUnityService unityService;
    public CharacterCombat heroCombat;
    private CharacterStats heroStats;
    private CharacterMovement characterMovement;
    private Score score;        // current score of the player

    // Use this for initialization
    void Start()
    {
        if (!isLocalPlayer && !localTest) return;

        // Initialize variables
        playerId = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>().GetPlayerId();
        isKnockedOut = false;
        characterMovement = new CharacterMovement(10.0f);
        if (unityService == null)
        {
            unityService = new UnityService();
        }
        heroRigidbody = GetComponent<Rigidbody>();
        heroStats = GetComponent<CharacterStats>();
        heroCombat = GetComponent<CharacterCombat>();
        prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();
        ground = new Plane(Vector3.up, Vector3.zero);
        score = new Score();

        // Run startup functions
        StartCamera();
        SetupUI();
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer && !localTest) return;

        // Only allow character movement if hero is not knocked out & game is currently not in prephase
        if (!isKnockedOut && !prephaseManager.IsCurrentlyInPrephase())
        {
            // Perform character movement controls
            heroRigidbody.velocity = characterMovement.Calculate(unityService.GetAxisRaw("Horizontal"), unityService.GetAxisRaw("Vertical"));
            PerformRotation();
        }

        Debug.DrawLine(transform.position, transform.forward * 20 + transform.position, Color.red);
        UpdateUI();

        // Perform an attack
        if (unityService.GetKeyDown(KeyCode.Space))
        {
            heroCombat.CmdAttack();
        }
    }

    // This function is used to get player direction.
    private void PerformRotation()
    {
        Ray cameraRay = view.ScreenPointToRay(Input.mousePosition);
        if (ground.Raycast(cameraRay, out rayLength))
        {
            pointToLookAt = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLookAt, Color.blue);
            transform.LookAt(new Vector3(pointToLookAt.x, transform.position.y, pointToLookAt.z));

        }
    }

    /// <summary>
    /// This function disables the main view camera in charge of capturing the UI
    /// </summary>
    private void StartCamera()
    {
        // There is a bug here.
        GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
        cam = Instantiate(heroCam);
        cam.GetComponent<HeroCameraController>().SetTarget(this.transform);
        view = cam.GetComponent<Camera>();
    }

    /// <summary>
    /// Initialize the necessary UI depending on current state of the game.
    /// Ie. Setup prephase UI if game is currently in prephase, or attacker UI if game is currently in dungeon crawling stage.
    /// </summary>
    private void SetupUI()
    {
        // Check prephase status and setup UI accordingly
        if (prephaseManager.IsCurrentlyInPrephase())
        {
            // Setup prephase UI
            Instantiate(prephaseManager.prephaseUI).SetActive(true);
        }
        else
        {
            // Setup attacker UI
            Instantiate(playerUI);

            // Set health bar image to full
            Image healthImage = GameObject.FindGameObjectWithTag("Health").GetComponent<Image>();
            healthImage.fillAmount = 1;
        }
    }

    /// <summary>
    /// Updates the player UI during dungeon crawling phase.
    /// </summary>
    private void UpdateUI()
    {
        if (!prephaseManager.IsCurrentlyInPrephase())
        {
            if (GameObject.FindGameObjectWithTag("Health") == null)
            {
                SetupUI();
            }

            // Update health bar and text
            Image healthImage = GameObject.FindGameObjectWithTag("Health").GetComponent<Image>();
            healthImage.fillAmount = (float)heroStats.GetCurrentHealth() / (float)heroStats.maxHealth;
            TextMeshProUGUI healthText = GameObject.FindGameObjectWithTag("HealthText").GetComponent<TextMeshProUGUI>();
            healthText.text = heroStats.GetCurrentHealth() + "/" + heroStats.maxHealth;

            if (heroStats.currentHealth <= 0)
            {
                KnockedOut();
            }

            // Update the score on UI
            TextMeshProUGUI scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
            scoreText.text = score.GetScore().ToString();
        }

    }

    //when health reaches 0, character is knocked out
    private void KnockedOut()
    {
        if (!isKnockedOut)
        {
            isKnockedOut = true;
            transform.gameObject.tag = "knockedOutPlayer";  //change tag so enemies won't go after it anymore
            transform.Rotate(90, 0, 0);                     //turn sideways to show knocked out

            Debug.Log("Player " + playerId + " is knocked out");

            // starts timer for length of time that character remains knocked out
            StartCoroutine(KnockOutTimer(deathTimer));
        }
    }

    /// <summary>
    /// Spawns the player at their spawn location.
    /// </summary>
    private void Spawn()
    {
        // Flip the character back to standing position if they were previously knocked out
        if (isKnockedOut)
        {
            transform.Rotate(-90, 0, 0);
        }

        // Reset variables
        isKnockedOut = false;
        heroStats.currentHealth = heroStats.maxHealth;
        transform.gameObject.tag = "Player";

        HeroManager heroState = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();

        //gets spawn location of player
        transform.position = heroState.GetSpawnLocationOfPlayer(playerId);

        Debug.Log("Player " + playerId + " spawned at " + heroState.GetSpawnLocationOfPlayer(playerId));
    }

    //increment how long until player respawns
    private IEnumerator KnockOutTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        Spawn();
    }

    //return if knocked out or not
    public bool GetKnockedOutStatus()
    {
        return isKnockedOut;
    }
}
