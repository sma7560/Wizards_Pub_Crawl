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
    [SerializeField][SyncVar] private int playerId;
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
        playerId = -1;
    }

    /// <summary>
    /// Called once per frame.
    /// </summary>
    void Update()
    {
        if (!hasAuthority) return;

        // Initialize MatchManager and playerId
        if (matchManager == null && GameObject.FindGameObjectWithTag("MatchManager") != null)
        {
            matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
            SetPlayerId(matchManager.GetPlayerId());
        }
    }

    /// <summary>
    /// Causes this hero to take a specified amount of damage.
    /// Calculates how much damage the hero should take based on stats, then decrements the hero's health by the calculated amount.
    /// </summary>
    /// <param name="amount">Amount of damage this hero should take.</param>
    /// <param name="damageType">The damage type of the damage being taken.</param>
    [Command]
    public void CmdTakeDamage(int amount, DamageType damageType)
    {
        Debug.Log("Damage Check");
        // Figure out actual amount taken via a calculation.
        if (!isServer) return; // For Sanity...
        Debug.Log("I am taking " + amount + " Damage");
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
        // Since health is dealt with in integers do some rounding with the finalDamage Calculated before subtractingd.
        finalDamage = Mathf.Clamp(finalDamage, 0, int.MaxValue);    // restrict damage to [0, int.MaxValue]
        Debug.Log("Damage: " + Mathf.Round(finalDamage));
        currentHealth = currentHealth - (int)Mathf.Round(finalDamage);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);   // restrict health to [0, maxHealth]

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

        if (isServer)
        {
            RpcHeal(amount);
        }
        else
        {
            CmdHeal(amount);
        }
    }
    private void LocalHeal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);   // Restrict current health to [0, maxHealth]
    }
    [ClientRpc]
    private void RpcHeal(int amount)
    {
        if (isLocalPlayer) return;
        LocalHeal(amount);
    }
    [Command]
    private void CmdHeal(int amount)
    {
        LocalHeal(amount);
        RpcHeal(amount);
    }

    /// ----------------------------------------
    /// -               SETTERS                -
    /// ----------------------------------------
    /// Setters are implemented to synchronize the variables across all players.
    /// This is done using both ClientRpc and Command, which are both needed to synchronize the variables properly.
    /// Only the player owning this script can change variables, hence a check on hasAuthority before any variable is set.

    /// <summary>
    /// Setter for player id.
    /// </summary>
    private void SetPlayerId(int id)
    {
        if (!hasAuthority) return;  // only the player owning this hero should change its player id

        playerId = id;

        if (isServer)
        {
            RpcSetPlayerId(id);
        }
        else
        {
            CmdSetPlayerId(id);
        }
    }
    [ClientRpc]
    private void RpcSetPlayerId(int id)
    {
        if (isLocalPlayer) return;  // prevent receiving the notification you started
        playerId = id;
    }
    [Command]
    private void CmdSetPlayerId(int id)
    {
        playerId = id;
        RpcSetPlayerId(id);
    }

    /// <summary>
    /// Setter for hero type.
    /// </summary>
    public void SetHeroType(HeroType heroType)
    {
        if (!hasAuthority) return;

        this.heroType = heroType;

        if (isServer)
        {
            RpcSetHeroType(heroType);
        }
        else
        {
            CmdSetHeroType(heroType);
        }
    }
    [ClientRpc]
    private void RpcSetHeroType(HeroType heroType)
    {
        if (isLocalPlayer) return;
        this.heroType = heroType;
    }
    [Command]
    private void CmdSetHeroType(HeroType heroType)
    {
        this.heroType = heroType;
        RpcSetHeroType(heroType);
    }

    /// <summary>
    /// Setter for move speed.
    /// </summary>
    public void SetMoveSpeed(int val)
    {
        if (!hasAuthority) return;

        moveSpeed = val;

        if (isServer)
        {
            RpcSetMoveSpeed(val);
        }
        else
        {
            CmdSetMoveSpeed(val);
        }
    }
    [ClientRpc]
    private void RpcSetMoveSpeed(int val)
    {
        if (isLocalPlayer) return;
        moveSpeed = val;
    }
    [Command]
    private void CmdSetMoveSpeed(int val)
    {
        moveSpeed = val;
        RpcSetMoveSpeed(val);
    }

    /// <summary>
    /// Setter for attack speed.
    /// </summary>
    public void SetAtkSpeed(int val)
    {
        if (!hasAuthority) return;

        atkSpeed = val;

        if (isServer)
        {
            RpcSetAtkSpeed(val);
        }
        else
        {
            CmdSetAtkSpeed(val);
        }
    }
    [ClientRpc]
    private void RpcSetAtkSpeed(int val)
    {
        if (isLocalPlayer) return;
        atkSpeed = val;
    }
    [Command]
    private void CmdSetAtkSpeed(int val)
    {
        atkSpeed = val;
        RpcSetAtkSpeed(val);
    }

    /// <summary>
    /// Setter for magic defense.
    /// </summary>
    public void SetMDefence(int val)
    {
        if (!hasAuthority) return;

        mDefence = val;

        if (isServer)
        {
            RpcSetMDefence(val);
        }
        else
        {
            CmdSetMDefence(val);
        }
    }
    [ClientRpc]
    private void RpcSetMDefence(int val)
    {
        if (isLocalPlayer) return;
        mDefence = val;
    }
    [Command]
    private void CmdSetMDefence(int val)
    {
        mDefence = val;
        RpcSetMDefence(val);
    }

    /// <summary>
    /// Setter for physical defense.
    /// </summary>
    public void SetPDefence(int val)
    {
        if (!hasAuthority) return;

        pDefence = val;

        if (isServer)
        {
            RpcSetPDefence(val);
        }
        else
        {
            CmdSetPDefence(val);
        }
    }
    [ClientRpc]
    private void RpcSetPDefence(int val)
    {
        if (isLocalPlayer) return;
        pDefence = val;
    }
    [Command]
    private void CmdSetPDefence(int val)
    {
        pDefence = val;
        RpcSetMDefence(val);
    }

    /// <summary>
    /// Setter for magic attack.
    /// </summary>
    public void SetMAttack(int val)
    {
        if (!hasAuthority) return;

        mAttack = val;

        if (isServer)
        {
            RpcSetMAttack(val);
        }
        else
        {
            CmdSetMAttack(val);
        }
    }
    [ClientRpc]
    private void RpcSetMAttack(int val)
    {
        if (isLocalPlayer) return;
        mAttack = val;
    }
    [Command]
    private void CmdSetMAttack(int val)
    {
        mAttack = val;
        RpcSetMAttack(val);
    }

    /// <summary>
    /// Setter for physical attack.
    /// </summary>
    public void SetPAttack(int val)
    {
        if (!hasAuthority) return;

        pAttack = val;

        if (isServer)
        {
            RpcSetPAttack(val);
        }
        else
        {
            CmdSetPAttack(val);
        }
    }
    [ClientRpc]
    private void RpcSetPAttack(int val)
    {
        if (isLocalPlayer) return;
        pAttack = val;
    }
    [Command]
    private void CmdSetPAttack(int val)
    {
        pAttack = val;
        RpcSetPAttack(val);
    }

    /// <summary>
    /// Sets the hero's health back to full.
    /// </summary>
    public void SetFullHealth()
    {
        if (!hasAuthority) return;

        currentHealth = maxHealth;

        if (isServer)
        {
            RpcSetFullHealth();
        }
        else
        {
            CmdSetFullHealth();
        }
    }
    [ClientRpc]
    private void RpcSetFullHealth()
    {
        if (isLocalPlayer) return;
        currentHealth = maxHealth;
    }
    [Command]
    public void CmdSetFullHealth()
    {
        currentHealth = maxHealth;
        RpcSetFullHealth();
    }

    /// <summary>
    /// Sets the knocked out status of the hero.
    /// </summary>
    public void SetKnockedOut(bool isKnockedOut)
    {
        if (!hasAuthority) return;

        this.isKnockedOut = isKnockedOut;

        if (isServer)
        {
            RpcSetKnockedOut(isKnockedOut);
        }
        else
        {
            CmdSetKnockedOut(isKnockedOut);
        }
    }
    [ClientRpc]
    private void RpcSetKnockedOut(bool isKnockedOut)
    {
        if (isLocalPlayer) return;
        this.isKnockedOut = isKnockedOut;
    }
    [Command]
    public void CmdSetKnockedOut(bool isKnockedOut)
    {
        this.isKnockedOut = isKnockedOut;
        RpcSetKnockedOut(isKnockedOut);
    }

    /// <summary>
    /// Increment the score by a value.
    /// </summary>
    /// <param name="amount">Value to increment the score by.</param>
    public void IncreaseScore(int amount)
    {
        if (!hasAuthority) return;

        score += amount;

        if (isServer)
        {
            RpcIncreaseScore(amount);
        }
        else
        {
            CmdIncreaseScore(amount);
        }
    }
    [ClientRpc]
    private void RpcIncreaseScore(int amount)
    {
        if (isLocalPlayer) return;
        score += amount;
    }
    [Command]
    public void CmdIncreaseScore(int amount)
    {
        score += amount;
        RpcIncreaseScore(amount);
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
