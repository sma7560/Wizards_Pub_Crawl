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
    private Rigidbody heroRigidbody;
    private bool isKnockedOut;
    private int reviveCount;
    private int deathTimer = 4; //default death timer

    public IUnityService unityService;
    public CharacterCombat heroCombat;
    private CharacterStats heroStats;
    private Transform characterTransform;
    private CharacterMovement characterMovement;

    private Score score;        // current score of the player

    // Use this for initialization
    void Start()
    {
        if (!localTest && !hasAuthority)
        {
            return;
        }

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

        characterTransform = GetComponent<Transform>();

        StartCamera();
        StartUI();

        score = new Score();    // initialize score value
    }

    // Update is called once per frame
    void Update()
    {
        // This function runs on all heroes

        if (!hasAuthority && !localTest)
        {
            return;
        }

        // Character Movement if hero is not knocked out
        if (!isKnockedOut)
        {
            heroRigidbody.velocity = characterMovement.Calculate(unityService.GetAxisRaw("Horizontal"), unityService.GetAxisRaw("Vertical"));
        }

        UpdateUI();

        // Perform an attack
        if (unityService.GetKeyDown(KeyCode.Space))
        {
            heroCombat.CmdAttack();
        }
    }

    private void StartCamera()
    {
        GameObject.FindGameObjectWithTag("MainCamera").SetActive(false);
        cam = Instantiate(heroCam);
        cam.GetComponent<HeroCameraController>().setTarget(this.transform);
    }

    private void StartUI()
    {
        Debug.Log("Attacker UI is active.");
        Instantiate(attackerUI);

        // Set health bar image to full
        Image healthImage = GameObject.FindGameObjectWithTag("Health").GetComponent<Image>();
        healthImage.fillAmount = 1;
    }

    private void UpdateUI()
    {
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
            StartCoroutine(reviveTimer(deathTimer));
        }
    }

    //when player is revived by another player
    private void respawn()
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
    private IEnumerator reviveTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        respawn();
    }

    //return if knocked out or not
    public bool GetKnockedOutStatus()
    {
        return isKnockedOut;
    }
}
