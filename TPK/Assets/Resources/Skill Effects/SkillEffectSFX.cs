using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectSFX : MonoBehaviour {

	public AudioClip sfx;
	private AudioSource audSrc;

	// Use this for initialization
	void Start () {
		audSrc = GetComponent<AudioSource> ();
		audSrc.PlayOneShot(sfx);
		Destroy (gameObject, sfx.length);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
