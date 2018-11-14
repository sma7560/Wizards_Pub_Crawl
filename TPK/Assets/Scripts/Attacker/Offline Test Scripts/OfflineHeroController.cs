using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfflineHeroController : MonoBehaviour {
    public GameObject attackerUI;
    private float moveSpeed;
    private Rigidbody heroRigidbody;

    private CharacterStats heroStats;
    private CharacterCombat heroCombat;

    private Camera mainCam;
    private Plane ground;
    private float rayLength;
    private Vector3 pointToLookAt;
    // Use this for initialization
    void Start () {
        moveSpeed = 5.0f;
        heroRigidbody = GetComponent<Rigidbody>();
        heroStats = GetComponent<CharacterStats>();
        heroCombat = GetComponent<CharacterCombat>();
        StartUI();

        // Camera stuff
        Camera.main.GetComponent<HeroCameraController>().setTarget(transform);
        mainCam = Camera.main.GetComponent<Camera>();
        ground = new Plane(Vector3.up, Vector3.zero);
    }
	
	// Update is called once per frame
	void Update () {
        CharacterMovement();
        SetRotation();
        //UpdateUI();
        Debug.DrawLine(transform.position, transform.forward * 20 + transform.position, Color.red);


    }
    private void StartUI() {
        Debug.Log("Attacker UI is active.");
        Instantiate(attackerUI);

        // Set health bar image to full
        Image healthImage = GameObject.FindGameObjectWithTag("Health").GetComponent<Image>();
        healthImage.fillAmount = 1;
    }
    private void SetRotation() {
        Ray cameraRay = mainCam.ScreenPointToRay(Input.mousePosition);
        if (ground.Raycast(cameraRay, out rayLength)) {
            pointToLookAt = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLookAt, Color.blue);
            transform.LookAt(new Vector3(pointToLookAt.x, transform.position.y, pointToLookAt.z));
        }
    }
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
    private void UpdateUI()
    {
        // Update health bar and text
        Image healthImage = GameObject.FindGameObjectWithTag("Health").GetComponent<Image>();
        healthImage.fillAmount = (float)heroStats.GetCurrentHealth() / (float)heroStats.maxHealth;
        //TextMeshProUGUI healthText = GameObject.FindGameObjectWithTag("HealthText").GetComponent<TextMeshProUGUI>();
        //healthText.text = heroStats.GetCurrentHealth() + "/" + heroStats.maxHealth;
    }
}
