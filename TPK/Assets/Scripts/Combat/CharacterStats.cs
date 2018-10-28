using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;

    public Stat damage;
    public Stat defence;
    public Stat attackSpeed;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int dmg)
    {
        // Calculate damage to take
        dmg -= defence.GetValue();
        dmg = Mathf.Clamp(dmg, 0, int.MaxValue);    // restrict damage to a value between [0, int.MaxValue]

        // Update current health
        currentHealth -= dmg;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);   // restrict health to a value between [0, maxHealth]
        Debug.Log(transform.name + " takes " + dmg + " damage. Current health is " + currentHealth + ".");

        // Check if character should die
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // Die in some way
        // This method is meant to be overwritten
        Debug.Log(transform.name + " died.");
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
