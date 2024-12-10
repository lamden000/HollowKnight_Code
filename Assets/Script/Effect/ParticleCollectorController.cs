using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollectorController : MonoBehaviour
{
    ParticleSystem ps;
    List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnParticleTrigger()
    {
        int triggerParticles = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);

        for (int i = 0; i < triggerParticles; i++)
        {
            ParticleSystem.Particle p = particles[i];
            p.remainingLifetime = 0;
            Debug.Log("We collected 1 particle");
            particles[i] = p;
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
    }

}
