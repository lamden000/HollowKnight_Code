using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [Header("Movement Particle")]
    [SerializeField] ParticleSystem movementParticle;
    [Header("")]
    [SerializeField] ParticleSystem fallParticle;
    [SerializeField] ParticleSystem touchParticle;
    [Range(0, 10)]
    [SerializeField] float occurAfterVelocity;

    [Range(0, 0.2f)]
    [SerializeField] float dustFormationPeriod;

    [SerializeField] Rigidbody2D playerrb;
    float counter;
    bool isGround = true;

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if(Mathf.Abs(playerrb.velocity.x) > occurAfterVelocity && isGround)
        {
            if (counter > dustFormationPeriod)
            {
                movementParticle.Play();
                counter = 0;
            }
        }
    }

    public void PlayTouchParticle()
    {
        touchParticle.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            fallParticle.Play();
            isGround = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            isGround = false;
        }
    }
}
