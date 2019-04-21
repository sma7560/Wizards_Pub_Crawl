using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plays a sound effect on creation.
/// GameObject is kept alive for duration of the audio clip.
/// </summary>
public class SkillEffectSFX : MonoBehaviour {

	public AudioClip sfx;
	private AudioSource audSrc;

	// Use this for initialization
	void Start () {
		audSrc = GetComponent<AudioSource> ();
		audSrc.PlayOneShot(sfx);
		Destroy (gameObject, sfx.length);
	}

}
