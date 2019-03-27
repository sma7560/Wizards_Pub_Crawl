using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Stats related to the character this script is attached to.
/// </summary>
public class CharacterStats : NetworkBehaviour
{
    public bool localTest;  // used for unit testing
    
    [SerializeField] [SyncVar] private int currentHealth;

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
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Causes the current character take damage and lose health.
    /// </summary>
    /// <param name="dmg">Amount of damage the character should take.</param>
    [Command]
    public void CmdTakeDamage(int dmg)
    {
        if (!localTest && !isServer) return;

        // Calculate damage to take
        //dmg -= defence.GetValue();
        dmg = Mathf.Clamp(dmg, 0, int.MaxValue);    // restrict damage to a value between [0, int.MaxValue]

        // Update current health
        currentHealth -= dmg;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);   // restrict health to a value between [0, maxHealth]
        Debug.Log(transform.name + " takes " + dmg + " damage. Current health is " + currentHealth + ".");
    }

    /// <returns>
    /// Getter for current health.
    /// </returns>
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    /// <summary>
    /// Setter for current health.
    /// </summary>
    /// <param name="health">Value to set current health to.</param>
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

    //default death function. Will be overwritten for enemies
    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
