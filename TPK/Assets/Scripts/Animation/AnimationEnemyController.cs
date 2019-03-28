using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEnemyController : MonoBehaviour {

    private Animator anim;

	// Use this for initialization
	void Awake () {
        anim = GetComponent<Animator>();
	}

    public void animationWalk(bool startOrStop)
    {
        //Debug.Log("Walk," + startOrStop);
        anim.SetBool("isWalking", startOrStop);
    }

    // animation for enemy attacking
    public void playAttack()
    {
        anim.SetTrigger("attack");
    }

    // animation for enemy dying
    public void deathAnimation()
    {
        anim.SetTrigger("isDead");
    }
}
