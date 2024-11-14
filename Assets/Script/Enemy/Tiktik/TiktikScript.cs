using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TiktikScript : EnemyBase
{

    protected override void Start()
    {
        base.Start();
	}

	void Update()
    {    
    }
  
    protected override void Die(int attackDirection)
    {
		rb.gravityScale = 1;
		base.Die(attackDirection);
    }

}
