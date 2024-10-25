using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    private AudioSource audioSource;
    [Header("Sounds")]
    public AudioClip wakeUp;
    public AudioClip run;
    public AudioClip clubPrepare;
    public AudioClip clubImpact;
    public AudioClip jumpPrepare;
    public AudioClip jumpImpact;
    public AudioClip die;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void WakeUp()
    {
        audioSource.PlayOneShot(wakeUp);
    }    

    public void Run()
    {
        audioSource.loop = true;
        audioSource.clip=run;
        audioSource.Play();
    }

    public void ClubPrepare()
    {
        audioSource.PlayOneShot(clubPrepare);
    }    

    public void ClubImpact()
    {
        audioSource.PlayOneShot(clubImpact);
    }

    public void JumpPrepare() {
        audioSource.PlayOneShot(jumpPrepare);
    }

    public void JumpImpact()
    {
        audioSource.PlayOneShot(jumpImpact);
    }

    public void Die()
    {
        audioSource.PlayOneShot(die);
    }

    public void Stop()
    {
        audioSource.Stop();
    }
}
