using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class AnimationEvents : MonoBehaviour
{
    private AudioSource audioSource;
    public int clubImpactDistanceX;

    [Header("Sounds")]
    public AudioClip wakeUp;
    public AudioClip run;
    public AudioClip clubPrepare;
    public AudioClip clubImpact;
    public AudioClip jumpPrepare;
    public AudioClip jumpImpact;
    public AudioClip die;

    [Header("Effect")]
    public ParticleSystem debris;
    public ParticleSystem debrisPower;

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
        List<ParticleSystem> particleSystems = new List<ParticleSystem>();
        int distance = clubImpactDistanceX;
        if (transform.rotation.y != 0)
           distance=-distance;
        particleSystems.Add(Instantiate(debris, transform.position + new Vector3(distance, 1, 0), Quaternion.identity));
        particleSystems.Add(Instantiate(debrisPower, transform.position + new Vector3(distance, 0, 0), Quaternion.identity));
        foreach (ParticleSystem particle in particleSystems)
        {
            Destroy(particle.gameObject, 0.5f);
        }
    }

    public void JumpPrepare() {
        audioSource.PlayOneShot(jumpPrepare);
    }

    public void JumpImpact()
    {
        List<ParticleSystem> particleSystems = new List<ParticleSystem>();
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
