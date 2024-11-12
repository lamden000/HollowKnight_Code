using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Crawlid : EnemyBase
{
    // Start is called before the first frame update
    protected override void Start()
    {      
        base.Start();       
        attackPower = 5;
    }

    void Update()
    {             
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
}
