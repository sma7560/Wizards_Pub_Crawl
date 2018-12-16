using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Used to update dungeon status during dungeon level gameplay.
/// </summary>
public class DungeonController : MonoBehaviour
{

    public GameObject inGameMenuObject;
    public IUnityService unityService;

    // Use this for initialization
    void Start()
    {
        if (unityService == null)
        {
            unityService = new UnityService();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Toggles in-game menu when Esc key pressed
        if (unityService.GetKeyDown(KeyCode.Escape))
        {
            inGameMenuObject.SetActive(!inGameMenuObject.activeSelf);
        }

        UpdateEnemyHealthBars();
    }

    public void QuitMatch()
    {
        Debug.Log("MATCH QUIT");
        SceneManager.LoadScene(0);
    }

    // Updates the statuses of all enemy health bars currently in the scene
    private void UpdateEnemyHealthBars()
    {
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy"); // Find all enemy objects

        for (int i = 0; i < enemyObjects.Length; i++)
        {
            EnemyStats enemyStats = enemyObjects[i].GetComponent<EnemyStats>(); // Get enemy stats
            Transform healthBar = enemyObjects[i].transform.Find("HealthBar");

            // Update health bar image
            Image healthImage = healthBar.Find("Health").GetComponent<Image>();
            healthImage.fillAmount = (float)enemyStats.GetCurrentHealth() / (float)enemyStats.maxHealth;

            // Update health bar text
            TextMeshProUGUI healthText = healthBar.Find("HealthText").GetComponent<TextMeshProUGUI>();
            healthText.text = enemyStats.GetCurrentHealth() + "/" + enemyStats.maxHealth;

            // Keep health bar facing towards camera
            Camera camera = Camera.current;
            if (camera != null)
            {
                Vector3 v = camera.transform.position - healthBar.position;
                v.x = v.z = 0.0f;
                healthBar.LookAt(camera.transform.position - v);    // Fix position towards of camera
                healthBar.rotation = (camera.transform.rotation);   // Fix rotation towards camera
            }
        }
    }

}