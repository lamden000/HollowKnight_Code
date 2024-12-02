using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeapingHuskScript : EnemyBase
{
    public float jumpForce;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
