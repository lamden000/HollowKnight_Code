using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashCollider : MonoBehaviour
{
    public ParticleSystem particleSystem; // H? th?ng h?t m� b?n ?ang s? d?ng
    public BoxCollider boxCollider;      // Collider m� b?n mu?n bao quanh h?t (ho?c d�ng BoxCollider2D cho game 2D)

    void Update()
    {
        // L?y th�ng tin v? k�ch th??c c?a Particle System
        var shape = particleSystem.shape;

        // ?i?u ch?nh k�ch th??c collider d?a tr�n k�ch th??c c?a h? th?ng h?t
        boxCollider.size = new Vector3(shape.scale.x, shape.scale.y, shape.scale.z);

        // B?n c� th? ?i?u ch?nh th�m v? tr� n?u c?n (n?u collider kh�ng ? ?�ng ch?)
        boxCollider.center = new Vector3(0, 0, 0); // ??t l?i t�m collider n?u c?n
    }
}
