using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// Contains controls of the player character.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CharacterCombat))]
[RequireComponent(typeof(BasicAttack))]
[RequireComponent(typeof(TestAnimConrtoller))]
[RequireComponent(typeof(HeroModel))]
public class HeroController : NetworkBehaviour
{
    public GameObject heroCam;
    public GameObject compass;

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
    private HeroModel heroModel;
    private BasicAttack battack;
    private TestAnimConrtoller animate;
    private Vector3 tempVelocity;

    /// <summary>
    /// Initialize variables.
    /// </summary>
    void Start()
    {
        if (!isLocalPlayer && !localTest) return;
        
        if (unityService == null)
        {
            unityService = new UnityService();
        }

        characterMovement = new CharacterMovement(10.0f);
        ground = new Plane(Vector3.up, Vector3.zero);

        heroRigidbody = GetComponent<Rigidbody>();
        heroModel = GetComponent<HeroModel>();
        battack = GetComponent<BasicAttack>();
        animate = GetComponent<TestAnimConrtoller>();

        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();

        // Run startup functions
        StartCamera();
        Spawn();
    }
    
    /// <summary>
    /// Called once per frame.
    /// </summary>
    void Update()
    {
        if (!isLocalPlayer && !localTest) return;

        // Only allow controls if hero is not knocked out, game is currently not in prephase, and match has not ended
        if (!heroModel.IsKnockedOut() && !prephaseManager.IsCurrentlyInPrephase() && !matchManager.HasMatchEnded())
        {
            // TODO:
            // This will be changed later but setting up the basic attack here. this should be moved to the endphase.
            // For setting up 
            if (!isDungeonReady)
            {
                isDungeonReady = true;
                battack.CmdSetAttackParameters();
                animate.myHeroType = heroModel.GetHeroType();
            }

            // Perform character movement controls
            tempVelocity = characterMovement.Calculate(unityService.GetAxisRaw("Horizontal"), unityService.GetAxisRaw("Vertical"));
            tempVelocity.y = heroRigidbody.velocity.y;
            heroRigidbody.velocity = tempVelocity;
            PerformRotation();

            // Perform an attack
            if (Input.GetMouseButtonDown(0))
            {
                //animate.PlayBasicAttack();
                StartCoroutine(AttackSpawn());
            }
        }

        // Check current health status
        if (!prephaseManager.IsCurrentlyInPrephase() && heroModel.GetCurrentHealth() <= 0)
        {
            KnockOut();
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
        GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
        cam = Instantiate(heroCam);
        cam.GetComponent<HeroCameraController>().SetTarget(this.transform);
        view = cam.GetComponent<Camera>();
    }

    /// <summary>
    /// Called when the player should be knocked out.
    /// </summary>
    private void KnockOut()
    {
        // Do nothing if hero is already knocked out
        if (heroModel.IsKnockedOut()) return;

        // Set status and death animation
        heroModel.SetKnockedOut(true);
        animate.SetDead(true);

        // Start timer for length of time that character remains knocked out
        StartCoroutine(KnockOutTimer());

        Debug.Log("Player " + matchManager.GetPlayerId() + " is knocked out.");
    }

    /// <summary>
    /// Spawns the player at their spawn location.
    /// </summary>
    private void Spawn()
    {
        // Reset animation back to alive animation
        if (heroModel.IsKnockedOut())
        {
            animate.SetDead(false);
        }

        // Show the compass again
        Instantiate(compass, transform);

        // Reset variables
        heroModel.SetKnockedOut(false);
        heroModel.SetFullHealth();

        // Set player location back to spawn point
        HeroManager heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();
        transform.position = heroManager.GetSpawnLocationOfPlayer(matchManager.GetPlayerId());

        Debug.Log("Player " + matchManager.GetPlayerId() + " spawned at " + transform.position);
    }

    /// <summary>
    /// Timer which waits for the default amount of time a player remains dead, then spawns the player again.
    /// </summary>
    private IEnumerator KnockOutTimer()
    {
        yield return new WaitForSeconds(deathTimer);
        Spawn();
    }

    private IEnumerator AttackSpawn()
    {
        animate.PlayBasicAttack();
        yield return new WaitForSeconds(0.25f);
        battack.PerformAttack();
    }
}
