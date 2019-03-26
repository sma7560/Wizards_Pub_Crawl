/**using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class PlayerSound : MonoBehaviour
{
    public AudioClip[] playerSounds = new AudioClip[9];

    void Start()
    {
        while (true)
        {
            StartCoroutine(PlaySounds());
        }
    }


    IEnumerator PlaySounds()
    {

        if (Input.GetAxis("Vertical") != 0f)
        {
            audio.clip = playerSounds[0];
            if (!audio.isPlaying)
            {
                audio.Play();
            }
            else if (audio.isPlaying)
            {
                yield return new WaitForSeconds(1);
                audio.Stop();
            }
        }
    }
**/