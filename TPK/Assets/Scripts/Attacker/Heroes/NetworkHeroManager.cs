using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


//This script is attached to heros/characters to communicate with the server. 
public class NetworkHeroManager : NetworkBehaviour
{
    // Character Stats.
    [SyncVar] public readonly int maxHealth = 100; // Stays at 100?
    [SyncVar] public int currentHealth;
    [SerializeField] [SyncVar] private int moveSpeed; // This will act as a multiplyer for movement speed(Velocity)
    [SerializeField] [SyncVar] private int atkSpeed;
    [SerializeField] [SyncVar] private int mDefence;
    [SerializeField] [SyncVar] private int pDefence;
    [SerializeField] [SyncVar] private int mAttack;
    [SerializeField] [SyncVar] private int pAttack;

    public int modAtkSpeed;
    public int modMDefence;
    public int modPDefence;
    public int modMAttack;
    public int modPAttack;


    public int heroIndex;
    public HeroType heroType; // Might need this for later

    // Basic Getters and setters for stats
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

    // Should this be server only or network only? Server side setters
    [Command]
    public void CmdSetMoveSpeed(int val)
    {
        moveSpeed = val;
    }

    [Command]
    public void CmdSetAtkSpeed(int val)
    {
        atkSpeed = val;
    }

    [Command]
    public void CmdSetMDefence(int val)
    {
        mDefence = val;
    }

    [Command]
    public void CmdSetPDefence(int val)
    {
        pDefence = val;
    }

    [Command]
    public void CmdSetMAttack(int val)
    {
        mAttack = val;
    }

    [Command]
    public void CmdSetPAttack(int val)
    {
        pAttack = val;
    }

    //Local
    public void SetMoveSpeed(int val)
    {
        //moveSpeed = val;
        CmdSetMoveSpeed(val);
    }

    public void SetAtkSpeed(int val)
    {
        //atkSpeed = val;
        CmdSetAtkSpeed(val);
    }
    public void SetMDefence(int val)
    {
        //mDefence = val;
        CmdSetMDefence(val);
    }
    public void SetPDefence(int val)
    {
        //pDefence = val;
        CmdSetPDefence(val);
    }
    public void SetMAttack(int val)
    {
        //mAttack = val;
        CmdSetMAttack(val);
    }
    public void SetPAttack(int val)
    {
        //pAttack = val;
        CmdSetPAttack(val);
    }

    // Getters and Setters for the stat modifiers
    public int GetModAtkSpeed()
    {
        return modAtkSpeed;
    }
    public int GetModMDefence()
    {
        return modMDefence;
    }
    public int GetModPDefence()
    {
        return modPDefence;
    }
    public int GetModMAttack()
    {
        return modMAttack;
    }
    public int GetModPAttack()
    {
        return modPAttack;
    }

    // Setters
    public void SetModAtkSpeed(int val)
    {
        modAtkSpeed = val;
    }
    public void SetModMDefence(int val)
    {
        modMDefence = val;
    }
    public void SetModPDefence(int val)
    {
        modPDefence = val;
    }
    public void SetModMAttack(int val)
    {
        modMAttack = val;
    }
    public void SetModPAttack(int val)
    {
        modPAttack = val;
    }

    /// <summary>
    /// THIS IS CURRENTLY BROKEN FOR UPDATING MODEL ON CLIENT.
    /// This function should set up which model should be loaded.
    /// </summary>
    /// <param name="myHero">The hero to set the model of.</param>
    public void SetModel(Hero myHero)
    {
        MatchManager matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        HeroManager heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();

        // Setup the hero object we want to change the model of
        GameObject hero = heroManager.GetHeroObject(matchManager.GetPlayerId());
        CmdSetModel(hero, myHero);
        //if (!isServer && isLocalPlayer)
        //{
        //    Debug.Log("SetModel() called for client");

        //    // Get managers
        //    MatchManager matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        //    HeroManager heroManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<HeroManager>();

        //    // Setup the hero object we want to change the model of
        //    GameObject hero = heroManager.GetHeroObject(matchManager.GetPlayerId());
        //    CmdSetModel(hero, myHero, matchManager.GetPlayerId());
        //}
        //else if (isServer)
        //{
        //    LocalSetModel(transform, myHero);
        //}
    }

    /// <summary>
    /// Tell server to change the model of the specified player.
    /// </summary>
    /// <param name="myHero">Hero to change to.</param>
    /// <param name="playerId">Player to change the model of.</param>
    [Command]
    private void CmdSetModel(GameObject hero, Hero myHero)
    {
        Debug.Log("CmdSetModel called");
        //LocalSetModel(hero.transform, myHero);
        RpcSetModel(hero, myHero);
    }

    [ClientRpc]
    private void RpcSetModel(GameObject hero, Hero myHero)
    {

        //Debug.Log("RPC() called for Player " + hero.GetComponent<HeroController>().getPlayerId());
        hero.transform.GetChild(heroIndex).gameObject.SetActive(false);
        heroIndex = myHero.childIndex;
        heroType = myHero.heroType;
        hero.transform.GetChild(heroIndex).gameObject.SetActive(true);
    }

    /// <summary>
    /// Set the model of the specified hero object.
    /// </summary>
    /// <param name="hero">Hero object which we want to change the model of.</param>
    /// <param name="myHero">Model to change the hero to.</param>
    private void LocalSetModel(Transform hero, Hero myHero)
    {
        Debug.Log("LocalSetModel() called for Player " + hero.gameObject.GetComponent<HeroController>().GetPlayerId());
        hero.GetChild(heroIndex).gameObject.SetActive(false);
        heroIndex = myHero.childIndex;
        heroType = myHero.heroType;
        hero.GetChild(heroIndex).gameObject.SetActive(true);
    }

    // Set a basic attack to be active
    // TODO make basic attacks 
    public void SetBasicAttack()
    {

    }

    // This should only work on server. By giving it a damage amount it 
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
        Debug.Log("Damage: " + Mathf.Round(finalDamage));
        currentHealth = currentHealth - (int)Mathf.Round(finalDamage);

        Debug.Log("My Current Health: " + currentHealth + "/" + maxHealth);
    }

    // Function for healing back to full health.
    public void SetFullHealth()
    {
        CmdSetFullHealth();
    }

    [Command]
    public void CmdSetFullHealth()
    {
        Debug.Log("Setting to Full Health");
        currentHealth = maxHealth;
    }

    // This function is meant to heal the character
    public void Heal(int amount)
    {
        if (!isServer) return;
        currentHealth += amount;
        // Defining max health as max health.
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

}
