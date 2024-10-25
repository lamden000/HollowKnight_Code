using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TiktikScript : EnemyBase
{
	public Vector2[] points;  // Array of manually input points

	public float deathForce = 200f;
	public LayerMask surfaceLayer;
	// Start is called before the first frame update

    protected override void Start()
    {
        base.Start();
        attackPower = 5;
		if (points.Length > 0)
		{
            transform.position = points[0];
		}
	}

	void Update()
    {    
    }
  
    protected override void Die(Vector2 attackDirection)
    {
        animator.SetTrigger("Dead");

        rb.velocity = Vector2.zero;
		rb.AddForce(attackDirection.normalized * deathForce);
		rb.gravityScale = 1;

		Destroy(gameObject, 3);
    }

	// Optional: Visualize the points in the Scene view
	void OnDrawGizmos()
	{
		// Draw points as small spheres and lines between points
		if (points != null && points.Length > 0)
		{
			for (int i = 0; i < points.Length; i++)
			{
				Gizmos.color = Color.green;
				// Draw lines between consecutive points
				if (i > 0)
				{
					Gizmos.DrawLine(points[i - 1], points[i]);
				}
				else
                {
					Gizmos.color = Color.red;
				}

				if (i == points.Length - 1)
				{
					Gizmos.DrawLine(points[0], points[i]);
				}
				Gizmos.DrawSphere(points[i], 0.1f);

			}
		}
	}
}
