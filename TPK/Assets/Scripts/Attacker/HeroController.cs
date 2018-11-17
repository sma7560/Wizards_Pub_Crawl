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
    private GameObject cam;
    public GameObject attackerUI;
    private float moveSpeed;
    private Rigidbody heroRigidbody;
    private bool isKnockedOut;
    private int reviveCount;
    private int deathTimer;

    private CharacterStats heroStats;
    private CharacterCombat heroCombat;
    private Transform characterTransform;

    // Use this for initialization
    void Start()
    {
        if (!hasAuthority)
        {
            return;
        }

        moveSpeed = 20.0f;
        heroRigidbody = GetComponent<Rigidbody>();
        heroStats = GetComponent<CharacterStats>();
        heroCombat = GetComponent<CharacterCombat>();
        characterTransform = GetComponent<Transform>();
        StartCamera();
        StartUI();
        isKnockedOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        // This function runs on all heroes

        if (!hasAuthority)
        {
            return;
        }

        CharacterMovement();
        UpdateUI();

        //if character is knocked out, check knocked-out specific interactions
        if (isKnockedOut)
        {
            checkKnockedOutStatus();
        }

        // Perform an attack
        if (Input.GetKeyDown(KeyCode.Space))
        {
            heroCombat.CmdAttack();
        }
    }

    // Hero character movement
    private void CharacterMovement()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            if (isKnockedOut)
            {
                return; //cannot move if knocked out
            }
            heroRigidbody.velocity = new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed, 0, Input.GetAxisRaw("Vertical") * moveSpeed);
        }
        else
        {
            heroRigidbody.velocity = new Vector3(0, 0, 0);
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
            knockedOut();
        }
    }


    public void KillMe()
    {
        CmdKillme();
        Destroy(gameObject);
        Debug.Log("Player died)");
    }

    [Command]
    private void CmdKillme()
    {
        NetworkServer.Destroy(gameObject);
    }

    //when health reaches 0, character is knocked out
    public void knockedOut()
    {
        if (!isKnockedOut)
        {
            isKnockedOut = true;
            transform.gameObject.tag = "knockedOutPlayer"; //change tag so enemies won't go after it anymore
            deathTimer = 0;
            characterTransform.Rotate(90, 0, 0); //turn sidewiwse to show knocked out

            //starts timer until character dies
            StartCoroutine(addDeathTimer());
        }
    }

    //when player is revived by another player
    public void revived()
    {
        isKnockedOut = false;
        transform.gameObject.tag = "Player";
        characterTransform.Rotate(-90, 0, 0);
        reviveCount = 0;
    }

    //knocked-out specific interactions
    public void checkKnockedOutStatus()
    {
        //dies if deathtimer reaches 50
        if (deathTimer >= 50)
        {
            StopCoroutine(addDeathTimer());
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
                StartCoroutine(addReviveTimer());
                if (reviveCount >= 100)
                {
                    //revive player is revive meter is filled up
                    StopCoroutine(addReviveTimer());
                    revived();
                }
            }
        }
    }

    //regenerate energy
    IEnumerator addDeathTimer()
    {
        while (true)
        {
            deathTimer += 5; // increase death timer by 5 every tick
            yield return new WaitForSeconds(0.5f); //amount of time between clicks

        }
    }

    IEnumerator addReviveTimer()
    {
        while (true)
        {
            reviveCount += 5; // increase revive timer by 5 every tick
            Debug.Log("reviving at " + reviveCount);
            yield return new WaitForSeconds(0.5f); //amount of time between clicks

        }
    }

    //return if knocked out or not
    public bool getStatus()
    {
        return isKnockedOut;
    }
}
