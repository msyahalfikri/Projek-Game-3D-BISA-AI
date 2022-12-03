using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioSource source;
    public AudioClip walkingSound, hurtSound, jumpSound;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayWalkingSound()
    {
        if (!source.isPlaying)
        {
            source.PlayOneShot(walkingSound);
        }
        else
        {
            source.Stop();
            source.PlayOneShot(walkingSound);
        }
    }

    public void StopWalkingSound()
    {
        source.Stop();
    }

    public void PlayJumpSound()
    {
        if (!source.isPlaying)
        {
            source.PlayOneShot(jumpSound);
        }
        else
        {
            source.Stop();
            source.PlayOneShot(hurtSound);
        }
    }

    public void PlayHurtSound()
    {
        if (!source.isPlaying)
        {
            source.PlayOneShot(hurtSound);
        }
        else
        {
            source.Stop();
            source.PlayOneShot(hurtSound);
        }
    }
}
