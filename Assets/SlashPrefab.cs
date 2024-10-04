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
        public Sprite value;
    }

    public List<KeyValue> keys;
    private Dictionary<string , Sprite> sprites;
    SpriteRenderer spriteRenderer;
    public float spinSpeed = 100f;
    public AudioClip attackSound;     
    private AudioSource audioSource;  

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        sprites = new Dictionary<string , Sprite>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        foreach (KeyValue kv in keys)
        {
            sprites[kv.key] = kv.value;
        }

        // Kiểm tra giá trị trong Dictionary
        foreach (var kvp in sprites)
        {
            Debug.Log("Key: " + kvp.Key + " - Value: " + kvp.Value);
        }
    }

    private void Update()
    {
        transform.Rotate(0, 0,- spinSpeed * Time.deltaTime);
    }
    public void Instantiate(string type)
    {
        spriteRenderer.sprite = sprites[type];
        audioSource.PlayOneShot(attackSound);
    }
}
