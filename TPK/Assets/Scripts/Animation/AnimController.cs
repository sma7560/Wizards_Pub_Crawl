using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


/// <summary>
/// Animation Controller that helps synchronize the animation across the network as well as
/// control local animation states.
/// </summary>
public class AnimController : NetworkBehaviour
{
    public Animator anim;
    public float inputH;
    public float inputV;
    public Vector3 forward;
    public float headingAngle;
    public int basicNum;
    public float timeToReset;
    public float timeElapsed;
    public float timeElapsedBetween;
    public float timeBetween;
    public float attaking;
    public bool isWalking;
    public HeroType myHeroType;

    private MatchManager matchManager;
    private PrephaseManager prephaseManager;
    private DungeonController dungeonController;

    // Parameters for animator to set
    // FBMove - This is for determining forward backwards movement
    // LRMove - For determining left right movement

    // Use this for initialization
    void Start()
    {

        anim = GetComponent<Animator>();
        if (!isLocalPlayer) return; // Return after setting the animator.
        // Set Up Animator
        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();
        dungeonController = GameObject.Find("EventSystem").GetComponent<DungeonController>();
        basicNum = 0;
        timeToReset = 1.5f;
        timeElapsed = 0;
        timeElapsedBetween = 0;
        timeBetween = 1.0f;
        attaking = 0;
        isWalking = false;

    }

    // Update is called once per frame
    // In this update function, the player orientation as well as input is checked.
    // This is done to make the animations feel reactive to player input.
    void Update()
    {
        if (!isLocalPlayer || prephaseManager.IsCurrentlyInPrephase() || matchManager.HasMatchEnded() || dungeonController.IsMenuOpen()) return;

        forward = this.transform.forward;
        forward.y = 0;
        headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;
        if (headingAngle > 180f) headingAngle -= 360f; // Keeps it between -180 and 180
        //Debug.Log("Horizontal: " + Input.GetAxisRaw("Horizontal"));

        inputV = 0;
        inputH = 0;

        if (Input.GetKey(CustomKeyBinding.GetForwardKey()))
        {
            inputV++;
        }
        if (Input.GetKey(CustomKeyBinding.GetBackKey()))
        {
            inputV--;
        }
        if (Input.GetKey(CustomKeyBinding.GetRightKey()))
        {
            inputH++;
        }
        if (Input.GetKey(CustomKeyBinding.GetLeftKey()))
        {
            inputH--;
        }

        if (inputH != 0 || inputV != 0)
        {
            isWalking = true;
            anim.SetBool("isWalking", isWalking);
            SetMovementAnim();
        }
        else
        {
            isWalking = false;
            anim.SetBool("isWalking", isWalking);
        }
        // See if attacks Are being performed.
        //SetBasicAttack();

    }
    /// <summary>
    /// Function for other scripts to call to interface with the animation controller when
    /// playing a specific animation for skills based on skill type.
    /// </summary>
    public void PlayAnim(SkillType skillType)
    {
        if (!isLocalPlayer) return;
        switch (skillType)
        {
            case SkillType.buff:
                anim.SetTrigger("Buff");
                CmdPlayBuff();
                break;
            case SkillType.magicLight:
                anim.SetTrigger("MLight");
                CmdPlayLight();
                break;
            case SkillType.magicHeavy:
                anim.SetTrigger("MHeavy");
                CmdPlayheavy();
                break;
            default:
                break;
        }

    }

    /// <summary>
    /// Function for playing the basic attack animations
    /// plays on local first then if sends a command to the server to play everywhere else.
    /// </summary>
    public void PlayBasicAttack()
    {
        if (!isLocalPlayer) return;
        anim.SetTrigger("MBasic");
        CmdPlayBasic();
    }

    /// <summary>
    /// Function for playing and triggering the death animation. 
    /// This interfaces with external controllers.
    /// Takes in a boolean that represent if the player character is dead or not.
    /// True = dead, false = not dead
    /// </summary>
    public void SetDead(bool status) {

        if (!isLocalPlayer) return;
        anim.SetBool("isDead", status);
        if (status) anim.SetTrigger("Die");
        CmdSetDead(status);
    }


    /// <summary>
    /// This function is for changing the float values to make the walking animation
    /// respond to the direction the player character is facing.
    /// </summary>
    private void SetMovementAnim()
    {

        if (headingAngle < 45.0f && headingAngle > -45.0f)
        {
            // For Left Right
            if (inputH > 0)
            {
                // Set LR to 1;
                anim.SetFloat("LRMove", 1.0f);
            }
            else if (inputH < 0)
            {
                // Set LR to -1
                anim.SetFloat("LRMove", -1.0f);
            }
            else
            {
                anim.SetFloat("LRMove", 0f);
            }

            // For Forward Back
            if (inputV > 0)
            {
                // Set LR to 1;
                anim.SetFloat("FBMove", 1.0f);
            }
            else if (inputV < 0)
            {
                // Set LR to -1
                anim.SetFloat("FBMove", -1.0f);
            }
            else
            {
                anim.SetFloat("FBMove", 0.0f);
            }
        }
        else if (headingAngle < 135.0f && headingAngle > 45.0f)
        {
            // For Left Right
            if (inputV > 0)
            {
                // Set LR to 1;
                anim.SetFloat("LRMove", -1.0f);
            }
            else if (inputV < 0)
            {
                // Set LR to -1
                anim.SetFloat("LRMove", 1.0f);
            }
            else
            {
                anim.SetFloat("LRMove", 0.0f);
            }

            // For Forward Back
            if (inputH > 0)
            {
                // Set LR to 1;
                anim.SetFloat("FBMove", 1.0f);
            }
            else if (inputH < 0)
            {
                // Set LR to -1
                anim.SetFloat("FBMove", -1.0f);
            }
            else
            {
                anim.SetFloat("FBMove", 0.0f);
            }
        }
        else if (headingAngle > -135.0f && headingAngle < -45.0f)
        {
            // For Left Right
            if (inputV < 0)
            {
                // Set LR to 1;
                anim.SetFloat("LRMove", -1.0f);
            }
            else if (inputV > 0)
            {
                // Set LR to -1
                anim.SetFloat("LRMove", 1.0f);
            }
            else
            {
                anim.SetFloat("LRMove", 0.0f);
            }

            // For Forward Back
            if (inputH < 0)
            {
                // Set LR to 1;
                anim.SetFloat("FBMove", 1.0f);
            }
            else if (inputH > 0)
            {
                // Set LR to -1
                anim.SetFloat("FBMove", -1.0f);
            }
            else
            {
                anim.SetFloat("FBMove", 0.0f);
            }
        }
        else if (headingAngle < -135.0f || headingAngle > 135.0f)
        {
            // For Left Right
            if (inputH < 0)
            {
                // Set LR to 1;
                anim.SetFloat("LRMove", 1.0f);
            }
            else if (inputH > 0)
            {
                // Set LR to -1
                anim.SetFloat("LRMove", -1.0f);
            }
            else
            {
                anim.SetFloat("LRMove", 0.0f);
            }

            // For Forward Back
            if (inputV < 0)
            {
                // Set LR to 1;
                anim.SetFloat("FBMove", 1.0f);
            }
            else if (inputV > 0)
            {
                // Set LR to -1
                anim.SetFloat("FBMove", -1.0f);
            }
            else
            {
                anim.SetFloat("FBMove", 0.0f);
            }
        }
    }



    /// <summary>
    /// Setting the death state animation throughout the network.
    /// The first call is made to the server which notifies the clients.
    /// </summary>
    [Command]
    private void CmdSetDead(bool status) {

        RpcSetDead(status);
    }

    /// <summary>
    /// Setting the buff state animation throughout the network.
    /// The first call is made to the server which notifies the clients.
    /// </summary>
    [Command]
    private void CmdPlayBuff()
    {
        RpcPlayBuff();
    }

    /// <summary>
    /// Setting the Light attack state animation throughout the network.
    /// The first call is made to the server which notifies the clients.
    /// </summary>
    [Command]
    private void CmdPlayLight()
    {
        RpcPlayLight();
    }

    /// <summary>
    /// Setting the Heavy attack state animation throughout the network.
    /// The first call is made to the server which notifies the clients.
    /// </summary>
    [Command]
    private void CmdPlayheavy()
    {
        RpcPlayheavy();
    }

    /// <summary>
    /// Setting the basic state animation throughout the network.
    /// The first call is made to the server which notifies the clients.
    /// </summary>
    [Command]
    private void CmdPlayBasic() {
        RpcPlayBasic();
    }

    /// <summary>
    /// This function has the ClientRpc attribute which means it is called on all clients.
    /// This function sets the Dead state on all client animators.
    /// </summary>
    [ClientRpc]
    private void RpcSetDead(bool status) {
        if (isLocalPlayer) return;
        anim.SetBool("isDead", status);
        if (status) anim.SetTrigger("Die");
    }

    /// <summary>
    /// This function has the ClientRpc attribute which means it is called on all clients.
    /// This function triggers the buff state on all client animators.
    /// </summary>
    [ClientRpc]
    private void RpcPlayBuff() {
        if (isLocalPlayer) return;
        anim.SetTrigger("Buff");
    }

    /// <summary>
    /// This function has the ClientRpc attribute which means it is called on all clients.
    /// This function triggers the light attack state on all client animators.
    /// </summary>
    [ClientRpc]
    private void RpcPlayLight()
    {
        if (isLocalPlayer) return;
        anim.SetTrigger("MLight");
    }

    /// <summary>
    /// This function has the ClientRpc attribute which means it is called on all clients.
    /// This function triggers the heavy attack state on all client animators.
    /// </summary>
    [ClientRpc]
    private void RpcPlayheavy()
    {
        if (isLocalPlayer) return;
        anim.SetTrigger("MHeavy");
    }

    /// <summary>
    /// This function has the ClientRpc attribute which means it is called on all clients.
    /// This function triggers the Basic attack state on all client animators.
    /// </summary>
    [ClientRpc]
    private void RpcPlayBasic() {
        if (isLocalPlayer) return;
        anim.SetTrigger("MBasic");
    }
}
