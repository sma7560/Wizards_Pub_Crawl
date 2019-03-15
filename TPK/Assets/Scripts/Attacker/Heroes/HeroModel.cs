using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Contains all data/state variables regarding the player character.
/// These variables are synced across the network and across all players.
/// </summary>
public class HeroModel : NetworkBehaviour
{
    // Stats
    [SyncVar] private readonly int maxHealth = 100;
    [SyncVar] private int currentHealth;
    [SyncVar] private int moveSpeed;        // multiplier for move speed (velocity)
    [SyncVar] private int atkSpeed;
    [SyncVar] private int mDefence;
    [SyncVar] private int pDefence;
    [SyncVar] private int mAttack;
    [SyncVar] private int pAttack;

    // Managers
    private MatchManager matchManager;

    // Other state variables
    [SyncVar] private int playerId;         // playerId controlling this hero
    [SyncVar] private HeroType heroType;
    [SyncVar] private bool isKnockedOut;
    [SyncVar] private int score;

    private int heroIndex;  // used for setting the character model

    /// <summary>
    /// Initialize variables.
    /// </summary>
    private void Start()
    {
        if (!hasAuthority) return;

        score = 0;
        isKnockedOut = false;
        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        SetPlayerId(matchManager.GetPlayerId());
    }

    /// <summary>
    /// Causes this hero to take a specified amount of damage.
    /// Calculates how much damage the hero should take based on stats, then decrements the hero's health by the calculated amount.
    /// This function is run on the server using data from the client which called this function.
    /// </summary>
    /// <param name="amount">Amount of damage this hero should take.</param>
    /// <param name="damageType">The damage type of the damage being taken.</param>
    [Command]
    public void CmdTakeDamage(int amount, DamageType damageType)
    {
        Debug.Log("Damage Check");
        Debug.Log("I am taking " + amount + " damage");

        // Calculate final damage taken based on stats
        float finalDamage = 0;

        switch (damageType)
        {
            case DamageType.magical:
                finalDamage = (float)(10 / mDefence) * amount;
                break;
            case DamageType.physical:
                finalDamage = (10 / pDefence) * amount;
                break;
            case DamageType.none:
                // The none case can be used to describe things such as true damage.
                finalDamage = amount;
                break;
        }

        finalDamage = Mathf.Clamp(finalDamage, 0, int.MaxValue);    // restrict damage to [0, int.MaxValue]
        currentHealth = currentHealth - (int)Mathf.Round(finalDamage);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);   // restrict health to [0, maxHealth]

        Debug.Log("Final damage: " + Mathf.Round(finalDamage));
        Debug.Log("My Current Health: " + currentHealth + "/" + maxHealth);
    }

    /// <summary>
    /// This function heals the character by a specified amount.
    /// </summary>
    /// <param name="amount">Amount of health to heal the character by.</param>
    public void Heal(int amount)
    {
        if (!hasAuthority) return;

        LocalHeal(amount);

        if (!isServer)
        {
            CmdHeal(amount);
        }
    }
    private void LocalHeal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);   // Restrict current health to [0, maxHealth]
    }
    [Command]
    private void CmdHeal(int amount)
    {
        LocalHeal(amount);
    }

    /// ----------------------------------------
    /// -               SETTERS                -
    /// ----------------------------------------
    /// Setters are implemented to synchronize the variables across all players.
    /// Synchronization is done using SyncVar and Command:
    ///     - SyncVar synchronizes the value from server to clients.
    ///     - Command synchronizes the value from client to server.
    /// Only the player owning this script can change its variables, hence a check on hasAuthority before any variable is set.

    /// <summary>
    /// Setter for player id.
    /// </summary>
    private void SetPlayerId(int id)
    {
        if (!hasAuthority) return;  // only the player owning this hero should change its player id

        playerId = id;

        if (!isServer)
        {
            CmdSetPlayerId(id);
        }
    }
    [Command]
    private void CmdSetPlayerId(int id)
    {
        playerId = id;
    }

    /// <summary>
    /// Setter for hero type.
    /// </summary>
    public void SetHeroType(HeroType heroType)
    {
        if (!hasAuthority) return;

        this.heroType = heroType;

        if (!isServer)
        {
            CmdSetHeroType(heroType);
        }
    }
    [Command]
    private void CmdSetHeroType(HeroType heroType)
    {
        this.heroType = heroType;
    }

    /// <summary>
    /// Setter for move speed.
    /// </summary>
    public void SetMoveSpeed(int val)
    {
        if (!hasAuthority) return;

        moveSpeed = val;

        if (!isServer)
        {
            CmdSetMoveSpeed(val);
        }
    }
    [Command]
    private void CmdSetMoveSpeed(int val)
    {
        moveSpeed = val;
    }

    /// <summary>
    /// Setter for attack speed.
    /// </summary>
    public void SetAtkSpeed(int val)
    {
        if (!hasAuthority) return;

        atkSpeed = val;

        if (!isServer)
        {
            CmdSetAtkSpeed(val);
        }
    }
    [Command]
    private void CmdSetAtkSpeed(int val)
    {
        atkSpeed = val;
    }

    /// <summary>
    /// Setter for magic defense.
    /// </summary>
    public void SetMDefence(int val)
    {
        if (!hasAuthority) return;

        mDefence = val;

        if (!isServer)
        {
            CmdSetMDefence(val);
        }
    }
    [Command]
    private void CmdSetMDefence(int val)
    {
        mDefence = val;
    }

    /// <summary>
    /// Setter for physical defense.
    /// </summary>
    public void SetPDefence(int val)
    {
        if (!hasAuthority) return;

        pDefence = val;

        if (!isServer)
        {
            CmdSetPDefence(val);
        }
    }
    [Command]
    private void CmdSetPDefence(int val)
    {
        pDefence = val;
    }

    /// <summary>
    /// Setter for magic attack.
    /// </summary>
    public void SetMAttack(int val)
    {
        if (!hasAuthority) return;

        mAttack = val;

        if (!isServer)
        {
            CmdSetMAttack(val);
        }
    }
    [Command]
    private void CmdSetMAttack(int val)
    {
        mAttack = val;
    }

    /// <summary>
    /// Setter for physical attack.
    /// </summary>
    public void SetPAttack(int val)
    {
        if (!hasAuthority) return;

        pAttack = val;

        if (!isServer)
        {
            CmdSetPAttack(val);
        }
    }
    [Command]
    private void CmdSetPAttack(int val)
    {
        pAttack = val;
    }

    /// <summary>
    /// Sets the hero's health back to full.
    /// </summary>
    public void SetFullHealth()
    {
        if (!hasAuthority) return;

        currentHealth = maxHealth;

        if (!isServer)
        {
            CmdSetFullHealth();
        }
    }
    [Command]
    private void CmdSetFullHealth()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Sets the knocked out status of the hero.
    /// </summary>
    public void SetKnockedOut(bool isKnockedOut)
    {
        if (!hasAuthority) return;

        this.isKnockedOut = isKnockedOut;

        if (!isServer)
        {
            CmdSetKnockedOut(isKnockedOut);
        }
    }
    [Command]
    private void CmdSetKnockedOut(bool isKnockedOut)
    {
        this.isKnockedOut = isKnockedOut;
    }

    /// <summary>
    /// Increment the score by a value.
    /// </summary>
    /// <param name="amount">Value to increment the score by.</param>
    public void IncreaseScore(int amount)
    {
        if (!hasAuthority) return;

        score += amount;

        if (!isServer)
        {
            CmdIncreaseScore(amount);
        }
    }
    [Command]
    private void CmdIncreaseScore(int amount)
    {
        score += amount;
    }

    /// <summary>
    /// This function sets the model of the hero to the given hero.
    /// Reference: https://answers.unity.com/questions/1414490/set-active-over-network.html
    /// </summary>
    /// <param name="myHero">The hero to set the model to.</param>
    public void SetModel(Hero myHero)
    {
        if (!hasAuthority) return;  // only the player owning this hero should change its model

        MatchManager matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        HeroManager heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();
        GameObject hero = heroManager.GetHeroObject(matchManager.GetPlayerId());

        int oldHeroIndex = heroIndex;

        // Update the player's model locally
        LocalSetModel(hero, myHero, oldHeroIndex);

        // Send update of player's model to all other players
        if (isServer)
        {
            RpcSetModel(hero, myHero, oldHeroIndex);
        }
        else
        {
            CmdSetModel(hero, myHero, oldHeroIndex);
        }
    }

    /// <summary>
    /// Tell server to change the model of the specified player on the client.
    /// </summary>
    /// <param name="hero">Hero object to change the model of.</param>
    /// <param name="myHero">Hero to change to.</param>
    /// <param name="oldHeroIndex">Index of the hero previously selected.</param>
    [Command]
    private void CmdSetModel(GameObject hero, Hero myHero, int oldHeroIndex)
    {
        LocalSetModel(hero, myHero, oldHeroIndex);
        RpcSetModel(hero, myHero, oldHeroIndex);
    }

    /// <summary>
    /// From server, tell all clients to change the model of the specified player.
    /// </summary>
    /// <param name="hero">Hero object to change the model of.</param>
    /// <param name="myHero">Hero to change to.</param>
    /// <param name="oldHeroIndex">Index of the hero previously selected.</param>
    [ClientRpc]
    private void RpcSetModel(GameObject hero, Hero myHero, int oldHeroIndex)
    {
        if (isLocalPlayer) return;  // prevent receiving the notification you started
        LocalSetModel(hero, myHero, oldHeroIndex);
    }

    /// <summary>
    /// Locally sets the model of the player.
    /// </summary>
    /// <param name="hero">Hero object to change the model of.</param>
    /// <param name="myHero">Hero to change to.</param>
    /// <param name="oldHeroIndex">Index of the hero previously selected.</param>
    private void LocalSetModel(GameObject hero, Hero myHero, int oldHeroIndex)
    {
        hero.transform.GetChild(oldHeroIndex).gameObject.SetActive(false);
        heroIndex = myHero.childIndex;
        heroType = myHero.heroType;
        hero.transform.GetChild(heroIndex).gameObject.SetActive(true);
    }

    /// <summary>
    /// Set a basic attack to be active.
    /// TODO: make basic attacks.
    /// </summary>
    public void SetBasicAttack()
    {

    }

    /// ----------------------------------------
    /// -               GETTERS                -
    /// ----------------------------------------
    public HeroType GetHeroType()
    {
        return heroType;
    }

    public int GetMoveSpeed()
    {
        return moveSpeed;
    }

    public int GetAtkSpeed()
    {
        return atkSpeed;
    }

    public int GetMDefence()
    {
        return mDefence;
    }

    public int GetPDefence()
    {
        return pDefence;
    }

    public int GetMAttack()
    {
        return mAttack;
    }

    public int GetPAttack()
    {
        return pAttack;
    }

    public int GetPlayerId()
    {
        return playerId;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public bool IsKnockedOut()
    {
        return isKnockedOut;
    }

    public int GetScore()
    {
        return score;
    }
}
