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
    public GameObject attackerUI;
    public bool localTest;

    private GameObject cam;

    // For setting up character direction.
    private Camera view;
    private Plane ground;
    private float rayLength;
    private Vector3 pointToLookAt;


    private Rigidbody heroRigidbody;
    private bool isKnockedOut;
    private int reviveCount;
    private int deathTimer = 4; //default death timer

    public IUnityService unityService;
    public CharacterCombat heroCombat;
    private CharacterStats heroStats;
    private Transform characterTransform;
    private CharacterMovement characterMovement;
    private Prephase prephase;

    private Score score;        // current score of the player

    // Use this for initialization
    void Start()
    {
        //if (!localTest && !hasAuthority && !isLocalPlayer)
        //{
        //    return;
        //}
        if (!isLocalPlayer && !localTest) return;

        isKnockedOut = false;
        characterMovement = new CharacterMovement(10.0f);

        // Set variables
        if (unityService == null)
        {
            unityService = new UnityService();
        }
        heroRigidbody = GetComponent<Rigidbody>();
        heroStats = GetComponent<CharacterStats>();
        heroCombat = GetComponent<CharacterCombat>();
        prephase = GameObject.Find("NetworkManagerV2").GetComponent<Prephase>();

        characterTransform = GetComponent<Transform>();
        ground = new Plane(Vector3.up, Vector3.zero);
        StartCamera();
        SetupUI();

        score = new Score();    // initialize score value
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer && !localTest) return;

        // Only allow character movement if hero is not knocked out & game is currently not in prephase
        if (!isKnockedOut && !prephase.IsCurrentlyInPrephase())
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
    // This function disables the main view camera in charge of capturing the UI
    private void StartCamera()
    {
        // There is a bug here.
        GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
        cam = Instantiate(heroCam);
        cam.GetComponent<HeroCameraController>().setTarget(this.transform);
        view = cam.GetComponent<Camera>();
    }

    /// <summary>
    /// Initialize the necessary UI depending on current state of the game.
    /// Ie. Setup prephase UI if game is currently in prephase, or attacker UI if game is currently in dungeon crawling stage.
    /// </summary>
    private void SetupUI()
    {
        // Check prephase status and setup UI accordingly
        if (prephase.IsCurrentlyInPrephase())
        {
            // Currently in prephase stage
            // Setup prephase UI
            Instantiate(prephase.prephaseUI);
        }
        else
        {
            // Currently not in prephase stage
            // Setup attacker UI
            Instantiate(attackerUI);

            // Set health bar image to full
            Image healthImage = GameObject.FindGameObjectWithTag("Health").GetComponent<Image>();
            healthImage.fillAmount = 1;
        }
    }

    private void UpdateUI()
    {
        // Check prephase status and update UI accordingly
        if (prephase.IsCurrentlyInPrephase())
        {
            // Currently in prephase stage
            // Update prephase UI
            prephase.UpdatePrephaseUI();
        }
        else
        {
            // Currently not in prephase stage
            // Update health bar and text
            Image healthImage = GameObject.FindGameObjectWithTag("Health").GetComponent<Image>();
            healthImage.fillAmount = (float)heroStats.GetCurrentHealth() / (float)heroStats.maxHealth;
            TextMeshProUGUI healthText = GameObject.FindGameObjectWithTag("HealthText").GetComponent<TextMeshProUGUI>();
            healthText.text = heroStats.GetCurrentHealth() + "/" + heroStats.maxHealth;

            //if (heroStats.currentHealth <= 0)
            //{
            //    KnockedOut();
            //}

            // Update the score on UI
            TextMeshProUGUI scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
            scoreText.text = score.GetScore().ToString();
        }

    }

    // Might be removed *** 
    public void KillMe()
    {
        CmdKillMe();
        unityService.Destroy(gameObject);
        Debug.Log("Player died)");
    }

    [Command]
    private void CmdKillMe()
    {
        NetworkServer.Destroy(gameObject);
    }

    //when health reaches 0, character is knocked out
    private void KnockedOut()
    {
        if (!isKnockedOut)
        {
            isKnockedOut = true;
            transform.gameObject.tag = "knockedOutPlayer"; //change tag so enemies won't go after it anymore
            characterTransform.Rotate(90, 0, 0); //turn sideways to show knocked out

            Debug.Log("Player knocked out");
            //starts timer until character dies
            StartCoroutine(ReviveTimer(deathTimer));
        }
    }

    //when player is revived by another player
    private void Respawn()
    {
        Debug.Log("Player respawned");
        isKnockedOut = false;
        heroStats.currentHealth = heroStats.maxHealth;
        transform.gameObject.tag = "Player";
        characterTransform.Rotate(-90, 0, 0);

        HeroState heroState = GameObject.Find("EventSystem").GetComponent<HeroState>();

        //gets spawn location of player
        /*TODO 
         * change value passed into getSpawnLocationOfPlayer after player overhaul
         * "1" should become whatever player it is
         * */
        characterTransform.position = heroState.getSpawnLocationOfPlayer(1);

    }

    //increment how long until player respawns
    private IEnumerator ReviveTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        Respawn();
    }

    //return if knocked out or not
    public bool GetKnockedOutStatus()
    {
        return isKnockedOut;
    }
}
