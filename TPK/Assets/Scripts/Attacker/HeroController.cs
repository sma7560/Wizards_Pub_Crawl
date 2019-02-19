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
[RequireComponent(typeof(BasicAttack))]
[RequireComponent(typeof(TestAnimConrtoller))]
[RequireComponent(typeof(NetworkHeroManager))]
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
    //public CharacterCombat heroCombat;
    //private CharacterStats heroStats;
    private CharacterMovement characterMovement;
    private NetworkHeroManager heroManager;
    private BasicAttack battack;
    private TestAnimConrtoller animate;
    private Score score = new Score();        // current score of the player

    private bool isDungeonReady = false;

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

        heroManager = GetComponent<NetworkHeroManager>();

        // These two will be replaced with the network hero manager.
        //heroStats = GetComponent<CharacterStats>();
        //heroCombat = GetComponent<CharacterCombat>();

        battack = GetComponent<BasicAttack>();
        animate = GetComponent<TestAnimConrtoller>();

        prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();
        ground = new Plane(Vector3.up, Vector3.zero);
        //score = new Score();

        // Run startup functions
        StartCamera();
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer && !localTest) return;

        // Only allow character movement if hero is not knocked out & game is currently not in prephase
        if (!isKnockedOut && !prephaseManager.IsCurrentlyInPrephase())
        {
            // This will be changed later but setting up the basic attack here. this should be moved to the endphase.
            // For setting up 
            if (!isDungeonReady)
            {
                isDungeonReady = true;
                battack.CmdSetAttackParameters();
                animate.myHeroType = heroManager.heroType;

            }

            // Perform character movement controls
            heroRigidbody.velocity = characterMovement.Calculate(unityService.GetAxisRaw("Horizontal"), unityService.GetAxisRaw("Vertical"));
            PerformRotation();
        }

        Debug.DrawLine(transform.position, transform.forward * 20 + transform.position, Color.red);
        UpdateUI();

        // Perform an attack
        if (unityService.GetKeyDown(KeyCode.Space))
        {
            battack.PerformAttack();
            //heroCombat.CmdAttack();
            animate.PlayBasicAttack();
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
            Destroy(GameObject.FindGameObjectWithTag("PlayerUI"));      // Destroy player UI
            Instantiate(prephaseManager.prephaseUI).SetActive(true);    // Start prephase UI
        }
        else
        {
            Destroy(GameObject.FindGameObjectWithTag("PrephaseUI"));    // Destroy prephase UI
            Instantiate(playerUI).SetActive(true);                      // Start player UI

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
        if (prephaseManager.IsCurrentlyInPrephase())
        {
            // Setup UI if Prephase UI is not found
            if (GameObject.FindGameObjectWithTag("PrephaseUI") == null)
            {
                SetupUI();
            }
        }
        else
        {
            // Setup UI if Player UI is not found
            if (GameObject.FindGameObjectWithTag("PlayerUI") == null)
            {
                SetupUI();
            }

            // Update health bar and text
            Image healthImage = GameObject.FindGameObjectWithTag("Health").GetComponent<Image>();
            healthImage.fillAmount = (float)heroManager.currentHealth / (float)heroManager.maxHealth;
            TextMeshProUGUI healthText = GameObject.FindGameObjectWithTag("HealthText").GetComponent<TextMeshProUGUI>();
            healthText.text = heroManager.currentHealth + "/" + heroManager.maxHealth;

            if (heroManager.currentHealth <= 0)
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
        //heroManager.currentHealth = heroManager.maxHealth;
        heroManager.SetFullHealth();
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

    //return playerID
    public int GetPlayerId()
    {
        return playerId;
    }

    //add score
    public void AddScore(int s)
    {
        score.IncreaseScore(s);
    }

    //return score
    public int GetScore()
    {
        return score.GetScore();
    }
}
