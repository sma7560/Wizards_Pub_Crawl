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

    public void playAttack()
    {
        Debug.Log("Attacking");
        anim.SetTrigger("attack");
    }
}
