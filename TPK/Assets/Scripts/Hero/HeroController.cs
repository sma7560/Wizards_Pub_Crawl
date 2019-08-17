using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Controls the player character.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BasicAttack))]
[RequireComponent(typeof(AnimController))]
[RequireComponent(typeof(HeroModel))]
public class HeroController : NetworkBehaviour
{
    public IUnityService unityService = new UnityService();

    [SerializeField] private GameObject heroCam;
    [SerializeField] private GameObject compass;

    // For setting up character direction
    private Camera view;
    private Plane ground;
    private float rayLength;
    private Vector3 pointToLookAt;

    private readonly float atkCoolDown = 0.5f;
    private readonly int deathTimer = 4;    // default death timer
    private bool isDungeonReady = false;

    private GameObject cam;
    private Rigidbody heroRigidbody;
    private PrephaseManager prephaseManager;
	private HeroManager heroManager;
    private MatchManager matchManager;
    private DungeonController dungeonController;
	private HeroModel heroModel;
	private CapsuleCollider col;
    private BasicAttack battack;
    private AnimController animate;
    private Vector3 tempVelocity;
    private float nextActiveTime;

    /// <summary>
    /// Initialize variables.
    /// </summary>
    void Start()
    {
        if (!isLocalPlayer)
        {
            //disable other player's audioListener
            GetComponent<AudioListener>().enabled = false;
            return;
        }
        Vector3 floor = new Vector3(0, 1.7f, 0);
        ground = new Plane(Vector3.up, floor);

        heroRigidbody = GetComponent<Rigidbody>();
		heroModel = GetComponent < HeroModel >();
        battack = GetComponent<BasicAttack>();
        animate = GetComponent<AnimController>();
		col = GetComponent<CapsuleCollider> ();

		heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();
        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();
        dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();
        nextActiveTime = 0;
        // Run startup functions
        StartCamera();
        Spawn();
    }

    /// <summary>
    /// Called once per frame.
    /// </summary>
    void Update()
    {
        if (!isLocalPlayer) return;

        // Only allow controls under certain conditions
        if (!heroModel.IsKnockedOut() &&
            !prephaseManager.IsCurrentlyInPrephase() &&
            !matchManager.HasMatchEnded() &&
            !dungeonController.IsMenuOpen())
        {
            // TODO:
            // This will be changed later but setting up the basic attack here. this should be moved to the endphase.
            // For setting up 
            if (!isDungeonReady)
            {
                isDungeonReady = true;
                animate.myHeroType = heroModel.GetHeroType();
            }

            // Perform character movement controls
            bool forwardPressed = unityService.GetKey(CustomKeyBinding.GetForwardKey());
            bool backPressed = unityService.GetKey(CustomKeyBinding.GetBackKey());
            bool leftPressed = unityService.GetKey(CustomKeyBinding.GetLeftKey());
            bool rightPressed = unityService.GetKey(CustomKeyBinding.GetRightKey());
            tempVelocity = heroModel.GetCharacterMovement().Calculate(forwardPressed, backPressed, leftPressed, rightPressed);
            tempVelocity.y = heroRigidbody.velocity.y;
            heroRigidbody.velocity = tempVelocity;
            PerformRotation();

            // Perform a basic attack Has small attack cooldowmn.
            if (unityService.GetKey(CustomKeyBinding.GetBasicAttackKey()))
            {
                if (Time.time > nextActiveTime)
                {
                    nextActiveTime = Time.time + atkCoolDown;
                    StartCoroutine(PerformBasicAttack());
                }
            }
        }

        // Check current health status
        if (!prephaseManager.IsCurrentlyInPrephase() &&
            heroModel.GetCurrentHealth() <= 0)
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
    /// This function disables the main view camera and enables HeroCamera.
    /// </summary>
    private void StartCamera()
    {
        GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
        cam = Instantiate(heroCam);
        cam.GetComponent<HeroCameraController>().SetTarget(transform);
        view = cam.GetComponent<Camera>();
    }

    /// <summary>
    /// Called when the player should be knocked out.
    /// </summary>
    private void KnockOut()
    {
		if (heroModel.IsKnockedOut()) return;

		//resets player velocity
		heroRigidbody.velocity = new Vector3 (0, heroRigidbody.velocity.y, 0);

        // Set status and death animation
        heroModel.SetKnockedOut(true);
        animate.SetDead(true);

        // Start timer for length of time that character remains knocked out
        StartCoroutine(KnockOutTimer());
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
        transform.position = heroManager.GetSpawnLocationOfPlayer(matchManager.GetPlayerId());
    }

    /// <summary>
    /// Timer which waits for the default amount of time a player remains dead, then spawns the player again.
    /// </summary>
    private IEnumerator KnockOutTimer()
    {
        yield return new WaitForSeconds(deathTimer);
        Spawn();
    }

    /// <summary>
    /// Performs a basic attack.
    /// </summary>
    private IEnumerator PerformBasicAttack()
    {
        animate.PlayBasicAttack();
        yield return new WaitForSeconds(0.25f);
        battack.PerformAttack();
    }
}
