using UnityEngine;
using UnityEngine.Networking;

public class CharacterStats : NetworkBehaviour
{
    public bool localTest;

    [SyncVar]
    public int currentHealth; //Making it public for now to view in inpsector.

    public int maxHealth;
    public Stat damage;
    public Stat defence;
    public Stat attackSpeed;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0) {
            Destroy(gameObject);
        }
    }

    [Command]
    public void CmdTakeDamage(int dmg)
    {
        if (!localTest && !isServer)
        {
            return;
        }

        // Calculate damage to take
        dmg -= defence.GetValue();
        dmg = Mathf.Clamp(dmg, 0, int.MaxValue);    // restrict damage to a value between [0, int.MaxValue]

        // Update current health
        currentHealth -= dmg;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);   // restrict health to a value between [0, maxHealth]
        Debug.Log(transform.name + " takes " + dmg + " damage. Current health is " + currentHealth + ".");
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetCurrentHealth(int health)
    {
        if (health > 0)
        {
            currentHealth = health;
        }
        else
        {
            currentHealth = 0;
        }
    }
}
