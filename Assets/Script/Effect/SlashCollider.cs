using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashCollider : MonoBehaviour
{
    public ParticleSystem particleSystem; // H? th?ng h?t mà b?n ?ang s? d?ng
    public BoxCollider boxCollider;      // Collider mà b?n mu?n bao quanh h?t (ho?c dùng BoxCollider2D cho game 2D)

    void Update()
    {
        // L?y thông tin v? kích th??c c?a Particle System
        var shape = particleSystem.shape;

        // ?i?u ch?nh kích th??c collider d?a trên kích th??c c?a h? th?ng h?t
        boxCollider.size = new Vector3(shape.scale.x, shape.scale.y, shape.scale.z);

        // B?n có th? ?i?u ch?nh thêm v? trí n?u c?n (n?u collider không ? ?úng ch?)
        boxCollider.center = new Vector3(0, 0, 0); // ??t l?i tâm collider n?u c?n
    }
}
