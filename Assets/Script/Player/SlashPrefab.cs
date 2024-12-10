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
    private GameObject player;
    private int baseSoulGet=11;
    [SerializeField] private float knockBackForce=11;

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
        player = GameObject.FindGameObjectWithTag("Player");
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
            if (enemy != null && !enemy.isDead)
            {
                CameraShake.instance.ShakeCamera(0.7f, 0.5f);
                Enemy();
                Vector2 force = player.transform.position - collider.transform.position;
                int direction = force.x > 0 ? -1 : 1;
                enemy.TakeDamage(damages[type], direction, knockBackForce);
                player.GetComponent<PlayerScript>().SetSoul(baseSoulGet);
            }

        }
        else if (collider.CompareTag("Grass"))
        {
            Grass gr = collider.GetComponent<Grass>();
            EnviromentCotroller con = gr.GetComponent<EnviromentCotroller>();
            if (!con.isDead)
            {
                Grass();
                Vector2 force = player.transform.position - collider.transform.position;
                int direction = force.x > 0 ? -1 : 1;
                con.StartDead(direction);
            }
        }
        else if (collider.CompareTag("Environment"))
        {
            EnviromentCotroller con = collider.GetComponent<EnviromentCotroller>();
            if (con != null && !con.isDead)
            {
                Vector2 force = player.transform.position - collider.transform.position;
                int direction = force.x > 0 ? -1 : 1;
                con.StartDead(direction);
                CameraShake.instance.ShakeCamera(0.5f, 0.5f);
            }
        }
    }

}

