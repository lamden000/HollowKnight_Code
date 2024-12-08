using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit_CrackController : MonoBehaviour
{
    public static Hit_CrackController instance;
    [Header("Hit_crack Particle")]
    [SerializeField] ParticleSystem backhit;
    [Header("")]
    [SerializeField] ParticleSystem hit_crack;
    [SerializeField] ParticleSystem die_effect;
    [SerializeField] ParticleSystem four_slash;

    [SerializeField] ParticleSystem last_health;
    [SerializeField] ParticleSystem low_health;

    [SerializeField] ParticleSystem left_dead;
    [SerializeField] ParticleSystem right_dead;

    [SerializeField] ParticleSystem ganhetmau;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        ResetParticleSystem(backhit);
        ResetParticleSystem(hit_crack);
        ResetParticleSystem(die_effect);
        ResetParticleSystem(four_slash);
        ResetParticleSystem(last_health);
        ResetParticleSystem(low_health);
        ResetParticleSystem(left_dead);
        ResetParticleSystem(right_dead);
        ResetParticleSystem(ganhetmau);
    }
    public void OpenEffect()
    {
        ResetParticleSystem(backhit);
        ResetParticleSystem(hit_crack);
        ResetParticleSystem(die_effect);
        ResetParticleSystem(four_slash);

        CameraShake.instance.ShakeCamera(1.5f, 0.5f);

        backhit.Play();
        hit_crack.Play();
        die_effect.Play();
        four_slash.Play();
    }
    public void LastHealthEffect()
    {
        StartCoroutine(PlayEffects());
    }

    private IEnumerator PlayEffects()
    {
        ResetParticleSystem(hit_crack);
        ResetParticleSystem(last_health);
        ResetParticleSystem(left_dead);
        ResetParticleSystem(right_dead);
        ResetParticleSystem(low_health);

        CameraShake.instance.ShakeCamera(2f, 5f);

        hit_crack.Play();
        yield return StartCoroutine(WaitForEffect(hit_crack));

        low_health.Play();
        yield return StartCoroutine(WaitForEffect(low_health));

        last_health.Play();
        yield return StartCoroutine(WaitForEffect(last_health));
        
        left_dead.Play();
        right_dead.Play();
    }

    private IEnumerator WaitForEffect(ParticleSystem effect)
    {
        while (effect.isPlaying)
        {
            float timeRemaining = effect.main.duration - effect.time;
            if (timeRemaining <= 0.05f)
            {
                break;
            }
            yield return null;
        }
    }

    public void HetmauEffect()
    {
        ResetParticleSystem(ganhetmau);

        ganhetmau.Play();
    }

    public void StopHetmauEffect()
    {
        ResetParticleSystem(ganhetmau);
    }
    private void ResetParticleSystem(ParticleSystem ps)
    {
        ps.Stop(true);
        ps.Clear();
    }
}
