using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

    // Parameters for animator to set
    // FBMove - This is for determining forward backwards movement
    // LRMove - For determining left right movement
    //

    // Use this for initialization
    void Start()
    {

        anim = GetComponent<Animator>();
        if (!isLocalPlayer) return; // Return after setting the animator.
        // Set Up Animator
        matchManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<MatchManager>();
        prephaseManager = GameObject.FindGameObjectWithTag("MatchManager").GetComponent<PrephaseManager>();
        basicNum = 0;
        timeToReset = 1.5f;
        timeElapsed = 0;
        timeElapsedBetween = 0;
        timeBetween = 1.0f;
        attaking = 0;
        isWalking = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer || prephaseManager.IsCurrentlyInPrephase() || matchManager.HasMatchEnded()) return;

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
    // This function is for interfacing with the animator
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
    public void PlayBasicAttack()
    {
        if (!isLocalPlayer) return;
        anim.SetTrigger("MBasic");
    }

    public void SetDead(bool status) {

        if (!isLocalPlayer) return;
        anim.SetBool("isDead", status);
        if (status) anim.SetTrigger("Die");
        CmdSetDead(status);
    }

    // This function is for setting up the movement for the legs.
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
    // Commands to synchronize Animations
    [Command]
    private void CmdSetDead(bool status) {

        RpcSetDead(status);
    }
    [Command]
    private void CmdPlayBuff()
    {
        RpcPlayBuff();
    }
    [Command]
    private void CmdPlayLight()
    {
        RpcPlayLight();
    }
    [Command]
    private void CmdPlayheavy()
    {
        RpcPlayheavy();
    }
    [ClientRpc]
    private void RpcSetDead(bool status) {
        if (isLocalPlayer) return;
        anim.SetBool("isDead", status);
        if (status) anim.SetTrigger("Die");
    }
    [ClientRpc]
    private void RpcPlayBuff() {
        if (isLocalPlayer) return;
        anim.SetTrigger("Buff");
    }
    [ClientRpc]
    private void RpcPlayLight()
    {
        if (isLocalPlayer) return;
        anim.SetTrigger("MLight");
    }
    [ClientRpc]
    private void RpcPlayheavy()
    {
        if (isLocalPlayer) return;
        anim.SetTrigger("MHeavy");
    }
}
