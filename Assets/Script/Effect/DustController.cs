using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustController : EnviromentCotroller
{
    [SerializeField]
    public ParticleSystem dust;

    public override void StartDead(int hitLeft)
    {
        base.StartDead(hitLeft);
        Quaternion rotation = dust.transform.rotation;
        if (hitLeft < 0)
        {
            rotation *= Quaternion.Euler(0, 180, 0);
        }
        Instantiate(dust, transform.position, rotation);
    }
}
