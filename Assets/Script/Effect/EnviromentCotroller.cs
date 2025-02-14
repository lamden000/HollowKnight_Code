using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class EnviromentCotroller : MonoBehaviour
{
    public ParticleSystem particlesystem;
    public float height;
    public Sprite sprite;
    private Transform trans;
    private SpriteRenderer spriteRenderer;
    public bool isDead = false;
    private void Start()
    {
        trans = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public virtual void StartDead(int hitLeft)
    {
        isDead = true;
        spriteRenderer.sprite = sprite;
        Vector3 spawnPosition = trans.position;

        spawnPosition.y += height;

        Quaternion rotation = particlesystem.transform.rotation;
        if (hitLeft < 0)
        {
            rotation *= Quaternion.Euler(0, 180, 0);
        }
        ParticleSystem spawnedParticle = Instantiate(particlesystem, spawnPosition, rotation);

        StartCoroutine(DestroyParticleWhenDone(spawnedParticle));
    }
    IEnumerator DestroyParticleWhenDone(ParticleSystem ps)
    {
        if (ps != null)
        {
            yield return new WaitWhile(() => ps.IsAlive(true));
            Destroy(ps.gameObject);
        }
        else
        {
            Debug.LogWarning("No ParticleSystem found on instantiated object!");
        }
    }
}
