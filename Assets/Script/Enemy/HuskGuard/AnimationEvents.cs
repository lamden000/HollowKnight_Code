using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class AnimationEvents : MonoBehaviour
{
    private AudioSource audioSource;
    public float clubImpactDistanceX;

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

    [Header("Prefab")]
    public GameObject objectPrefab;

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
        audioSource.clip = run;
        audioSource.Play();
    }

    public void ClubPrepare()
    {
        audioSource.PlayOneShot(clubPrepare);
    }

    public void ClubImpact()
    {
        audioSource.PlayOneShot(clubImpact);
        InstantiateDebris(clubImpactDistanceX);
        CameraShake.instance.ShakeCamera(1, 0.5f);
    }

    private void InstantiateDebris(float distance=0)
    {
        List<ParticleSystem> particleSystems = new List<ParticleSystem>();    
        if (transform.rotation.y != 0)
            distance = -distance;
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

        GameObject leftObject = Instantiate(objectPrefab, transform.position, Quaternion.identity);
        GameObject rightObject = Instantiate(objectPrefab, transform.position, Quaternion.identity);
        leftObject.transform.SetParent(transform);
        rightObject.transform.SetParent(transform);

        leftObject.transform.rotation=Quaternion.Euler(0,0,0);
        rightObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        rightObject.GetComponent<Animator>().SetInteger("direction",1);
        CameraShake.instance.ShakeCamera(3, 1);
        InstantiateDebris();
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
