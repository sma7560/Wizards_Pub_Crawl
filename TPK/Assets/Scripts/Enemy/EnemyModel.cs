using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Stats of this enemy.
/// Attached to an enemy GameObject.
/// </summary>
public class EnemyModel : NetworkBehaviour
{
    // Stats
    [SyncVar] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private int damage;
    [SerializeField] private int attackSpeed;
    [SerializeField] private int movementSpeed;
    [SerializeField] private int lookRadius;        // radius where enemy can detect players
    [SerializeField] private int attackRange;       // range where enemy can attack players

    // Item drops
    [SerializeField] private GameObject[] droppableItems = new GameObject[3];
    [SerializeField] private int dropRate;

    // Idle movement
    [SerializeField] private float idleRange;
    [SerializeField] private float idleHowOftenDirectionChanged;

    [SerializeField]
    private AudioClip deathSound;
    [SerializeField]
    private AudioClip attackSound;
    private AudioSource source;

    
    private bool isDying;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        isDying = false;
    }

    void Update()
    {
        if (currentHealth <= 0 && !isDying)
        {
            PlayDeathSound();
            StartCoroutine(DeathSequence());
        }
    }

    /// <summary>
    /// Causes the current character take damage and lose health.
    /// </summary>
    /// <param name="dmg">Amount of damage the character should take.</param>
    [Command]
    public void CmdTakeDamage(int dmg)
    {
        // Calculate damage to take
        dmg = Mathf.Clamp(dmg, 0, int.MaxValue);    // restrict damage to a value between [0, int.MaxValue]

        // Update current health
        currentHealth -= dmg;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);   // restrict health to a value between [0, maxHealth]
    }

    /// <summary>
    /// Behaviour upon enemy death.
    /// Animates enemy's death and randomly drops item, then destroys enemy object.
    /// </summary>
    private IEnumerator DeathSequence()
    {
        isDying = true;
		GetComponent<CapsuleCollider> ().enabled = false;
        GetComponent<Animator>().SetTrigger("isDead");

        // Wait for animation to finish before deleting gameobject
        yield return new WaitForSeconds(3);

        DropItem();
        Destroy(gameObject);
    }

    private void PlayDeathSound()
    {
        source.PlayOneShot(deathSound);
    }

    [ClientRpc]
    private void RpcPlayAttackSound()
    {
        source.PlayOneShot(attackSound, 0.1f);
    }

    public IEnumerator soundDelayForAnimationSync()
    {
        yield return new WaitForSeconds(0.3f);
        RpcPlayAttackSound();
    }

    /// <summary>
    /// Random chance to drop an item upon death.
    /// </summary>
    private void DropItem()
    {
        if (!isServer) return;
        
        int randChance = Random.Range(0, 101);
        if (randChance > 100 - dropRate)
        {
            GameObject monsterDrop = DetermineItemDrop();
            Vector3 itemPosition = transform.position;
            itemPosition.y = itemPosition.y + 0.7f;
            GameObject itemDrop = Instantiate(monsterDrop, itemPosition, Quaternion.Euler(0, 0, 0));
            NetworkServer.Spawn(itemDrop);
        }
    }

    /// <returns>
    /// Returns one of the potential item drops at random.
    /// </returns>
    private GameObject DetermineItemDrop()
    {
        int randItem = Random.Range(0, 101);

        if (randItem > 40)
        {
            return droppableItems[0];
        }
        else if (randItem > 20)
        {
            return droppableItems[1];
        }
        else
        {
            return droppableItems[2];
        }
    }

    /// ----------------------------------------------
    /// SETTERS
    /// ----------------------------------------------
    
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

    /// ----------------------------------------------
    /// GETTERS
    /// ----------------------------------------------

    public float GetIdleRange()
    {
        return idleRange;
    }

    public float GetIdleHowOftenDirectionChanged()
    {
        return idleHowOftenDirectionChanged;
    }

    public int GetMovementSpeed()
    {
        return movementSpeed;
    }

    public bool IsDying()
    {
        return isDying;
    }
    
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetDamage()
    {
        return damage;
    }

    public int GetAttackSpeed()
    {
        return attackSpeed;
    }

    public int GetLookRadius()
    {
        return lookRadius;
    }

    public int GetAttackRange()
    {
        return attackRange;
    }
}
