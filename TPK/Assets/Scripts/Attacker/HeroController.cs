using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// Contains logic for the hero.
/// </summary>
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
    public GameObject playerUI;     // Player UI for dungeon crawling phase

    // For unit testing
    public bool localTest;
    public IUnityService unityService;

    // For setting up character direction
    private Camera view;
    private Plane ground;
    private float rayLength;
    private Vector3 pointToLookAt;

    private readonly int deathTimer = 4;    // default death timer
    private bool isDungeonReady = false;

    private GameObject cam;
    private Rigidbody heroRigidbody;
    private PrephaseManager prephaseManager;
    private MatchManager matchManager;
    private CharacterMovement characterMovement;
    private NetworkHeroManager networkHeroManager;
    private BasicAttack battack;
    private TestAnimConrtoller animate;

    // State variables
    [SerializeField][SyncVar] private int playerId;
    private bool isKnockedOut;
    [SyncVar] private Score score = new Score();      // current score of the player

    // Use this for initialization
    void Start()
    {
        if (!isLocalPlayer && !localTest) return;

        // Initialize variables
        if (unityService == null)
        {
            unityService = new UnityService();
        }
        
        isKnockedOut = false;

        characterMovement = new CharacterMovement(10.0f);
        ground = new Plane(Vector3.up, Vector3.zero);

        heroRigidbody = GetComponent<Rigidbody>();
        networkHeroManager = GetComponent<NetworkHeroManager>();
        battack = GetComponent<BasicAttack>();
        animate = GetComponent<TestAnimConrtoller>();

        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();
        SetPlayerId(matchManager.GetPlayerId());

        // Run startup functions
        StartCamera();
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer && !localTest) return;

        // Only allow controls if hero is not knocked out, game is currently not in prephase, and match has not ended
        if (!isKnockedOut && !prephaseManager.IsCurrentlyInPrephase() && !matchManager.HasMatchEnded())
        {
            // TODO:
            // This will be changed later but setting up the basic attack here. this should be moved to the endphase.
            // For setting up 
            if (!isDungeonReady)
            {
                isDungeonReady = true;
                battack.CmdSetAttackParameters();
                animate.myHeroType = networkHeroManager.heroType;
            }

            // Perform character movement controls
            heroRigidbody.velocity = characterMovement.Calculate(unityService.GetAxisRaw("Horizontal"), unityService.GetAxisRaw("Vertical"));
            PerformRotation();

            // Perform an attack
            if (unityService.GetKeyDown(KeyCode.Space))
            {
                battack.PerformAttack();
                animate.PlayBasicAttack();
            }
        }
        
        // Perform individual update functions
        UpdateUI();

        // Check for current health status
        if (networkHeroManager.currentHealth <= 0)
        {
            KnockedOut();
        }
    }

    /// <summary>
    /// Sets the player rotation.
    /// </summary>
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
    /// This function disables the main view camera in charge of capturing the UI.
    /// </summary>
    private void StartCamera()
    {
        // TODO:
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
            healthImage.fillAmount = (float)networkHeroManager.currentHealth / (float)networkHeroManager.maxHealth;
            TextMeshProUGUI healthText = GameObject.FindGameObjectWithTag("HealthText").GetComponent<TextMeshProUGUI>();
            healthText.text = networkHeroManager.currentHealth + "/" + networkHeroManager.maxHealth;

            // Update the score on UI
            TextMeshProUGUI scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
            scoreText.text = score.GetScore().ToString();
        }
    }

    /// <summary>
    /// Sets the current status of the hero to knocked out.
    /// </summary>
    private void KnockedOut()
    {
        if (!isKnockedOut)
        {
            isKnockedOut = true;
            transform.Rotate(90, 0, 0); // rotate transform sideways to show knocked out

            Debug.Log("Player " + playerId + " is knocked out.");

            // start timer for length of time that character remains knocked out
            StartCoroutine(KnockOutTimer());
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
        networkHeroManager.SetFullHealth();

        // get spawn location of player
        HeroManager heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();
        transform.position = heroManager.GetSpawnLocationOfPlayer(playerId);

        Debug.Log("Player " + playerId + " spawned at " + heroManager.GetSpawnLocationOfPlayer(playerId));
    }

    /// <summary>
    /// Timer which waits for the default amount of time a player remains dead, then spawns the player again.
    /// </summary>
    private IEnumerator KnockOutTimer()
    {
        yield return new WaitForSeconds(deathTimer);
        Spawn();
    }

    /// <returns>
    /// Returns true if the player is currently knocked out, else returns false.
    /// </returns>
    public bool IsKnockedOut()
    {
        return isKnockedOut;
    }
    
    /// <returns>
    /// Returns the current player id of the hero (taken from MatchManager).
    /// </returns>
    public int GetPlayerId()
    {
        return playerId;
    }

    /// <summary>
    /// Increments this player's score by a specified amount.
    /// </summary>
    /// <param name="s">Amount to increment the score by.</param>
    public void AddScore(int s)
    {
        score.IncreaseScore(s);
    }

    /// <returns>
    /// Returns the score of the current player.
    /// </returns>
    public int GetScore()
    {
        return score.GetScore();
    }

    private void SetPlayerId(int id)
    {
        if (!hasAuthority) return;  // only the player owning this hero should change its player id

        playerId = id;

        if (isServer)
        {
            RpcSetPlayerId(id);
        }
        else
        {
            CmdSetPlayerId(id);
        }
    }

    [ClientRpc]
    private void RpcSetPlayerId(int id)
    {
        if (isLocalPlayer) return;  // prevent receiving the notification you started
        playerId = id;
    }

    [Command]
    private void CmdSetPlayerId(int id)
    {
        playerId = id;
        RpcSetPlayerId(id);
    }
}
