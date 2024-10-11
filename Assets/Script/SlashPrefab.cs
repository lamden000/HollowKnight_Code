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

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            EnemyBase enemy = collider.GetComponent<EnemyBase>();
            Vector2 direction = (transform.position - collider.transform.position).normalized;
            enemy.TakeDamage(damages[type], direction);

        }

    }
}
