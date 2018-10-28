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
public class HeroController : NetworkBehaviour
{
    public GameObject heroCam;
    private GameObject cam;
    public GameObject attackerUI;
    private float moveSpeed;
    private Rigidbody heroRigidbody;

    private CharacterStats heroStats;
    private CharacterCombat heroCombat;

    // Use this for initialization
    void Start()
    {
        if (!hasAuthority)
        {
            return;
        }

        moveSpeed = 5.0f;
        heroRigidbody = GetComponent<Rigidbody>();
        heroStats = GetComponent<CharacterStats>();
        heroCombat = GetComponent<CharacterCombat>();

        StartCamera();
        StartUI();
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

        // Perform an attack
        if (Input.GetKeyDown(KeyCode.Space))
        {
            heroCombat.Attack();
        }
    }

    // Hero character movement
    private void CharacterMovement()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
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
    }
}
