using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class SlashPrefab : MonoBehaviour
{
    [System.Serializable]
    public class KeyValue
    {
        public string key;
        public Sprite sprite;
        public int damage;
    }

    public List<KeyValue> keys;
    private Dictionary<string , Sprite> sprites;
    private Dictionary<string, int> damages;
    SpriteRenderer spriteRenderer;
    public float spinSpeed = 100f;
    public AudioClip attackSound;     
    private AudioSource audioSource;
    private string type;

    [Header("Effect")]
    public GameObject slashgrass;
    public GameObject slashenemy;
    public GameObject slashwall;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        sprites = new Dictionary<string , Sprite>();
        damages = new Dictionary<string , int>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        foreach (KeyValue kv in keys)
        {
            sprites[kv.key] = kv.sprite;
            damages[kv.key] = kv.damage;
        }

    }

    private void Update()
    {
        transform.Rotate(0, 0,- spinSpeed * Time.deltaTime);
    }

    public void Instantiate(string type)
    {
        this.type = type;
        spriteRenderer.sprite = sprites[type];
        audioSource.PlayOneShot(attackSound);
    }
    private void Grass()
    {
        
        GameObject effect = Instantiate(slashgrass, transform.position, Quaternion.identity);
        if (transform.rotation.y < 0)
        {
            var main = effect.GetComponent<ParticleSystem>().main;
            main.flipRotation = 0;
        }
        Destroy(effect, 0.1f);
    }
    private void Enemy()
    {

        GameObject effect = Instantiate(slashenemy, transform.position, Quaternion.identity);
        if (transform.rotation.y < 0)
        {
            var main = effect.GetComponent<ParticleSystem>().main;
            main.flipRotation = 0;
        }
        Destroy(effect, 0.1f);
    }
    private void Wall()
    {

        GameObject effect = Instantiate(slashwall, transform.position, transform.rotation);
        Destroy(effect, 0.1f);
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            
            EnemyBase enemy = collider.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                Enemy();
                Vector2 direction = (transform.position - collider.transform.position).normalized;
                enemy.TakeDamage(damages[type], direction);
            }

        }
        else if (collider.CompareTag("Grass"))
        {
            Grass();
        }
        
    }

}

