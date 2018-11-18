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
    private int deathTimer;

    public IUnityService unityService;
    public CharacterCombat heroCombat;
    private CharacterStats heroStats;
    private Transform characterTransform;
    private CharacterMovement characterMovement;

    // Use this for initialization
    void Start()
    {
        if (!hasAuthority && !localTest)
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

        //if character is knocked out, check knocked-out specific interactions
        if (isKnockedOut)
        {
            CheckKnockedOutStatus();
        }

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
    }

    public void KillMe()
    {
        CmdKillMe();
        Destroy(gameObject);
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
            deathTimer = 0;
            characterTransform.Rotate(90, 0, 0); //turn sideways to show knocked out

            //starts timer until character dies
            StartCoroutine(AddDeathTimer());
        }
    }

    //when player is revived by another player
    private void Revived()
    {
        isKnockedOut = false;
        transform.gameObject.tag = "Player";
        characterTransform.Rotate(-90, 0, 0);
        reviveCount = 0;
    }

    //knocked-out specific interactions
    private void CheckKnockedOutStatus()
    {
        //dies if deathtimer reaches 50
        if (deathTimer >= 50)
        {
            StopCoroutine(AddDeathTimer());
            KillMe();
        }

        //find nearby players
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        int numPlayers = playerObjects.Length;
        Transform[] targets = new Transform[numPlayers];
        for (int i = 0; i < numPlayers; i++)
        {
            targets[i] = playerObjects[i].GetComponent<Transform>();
        }

        // check if any players nearby
        for (int i = 0; i < numPlayers; i++)
        {
            float distance = Vector3.Distance(targets[i].position, transform.position);
            if (distance < 2)
            {
                //if player in range, fill up revive meter
                StartCoroutine(AddReviveTimer());
                if (reviveCount >= 100)
                {
                    //revive player is revive meter is filled up
                    StopCoroutine(AddReviveTimer());
                    Revived();
                }
            }
        }
    }

    //regenerate energy
    private IEnumerator AddDeathTimer()
    {
        while (true)
        {
            deathTimer += 5; // increase death timer by 5 every tick
            yield return new WaitForSeconds(0.5f); //amount of time between clicks

        }
    }

    private IEnumerator AddReviveTimer()
    {
        while (true)
        {
            reviveCount += 5; // increase revive timer by 5 every tick
            Debug.Log("reviving at " + reviveCount);
            yield return new WaitForSeconds(0.5f); //amount of time between clicks

        }
    }

    //return if knocked out or not
    public bool GetKnockedOutStatus()
    {
        return isKnockedOut;
    }
}
