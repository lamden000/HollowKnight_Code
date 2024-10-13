using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor;

    public void Move(float deltax, float deltay)
    {
        Vector3 newPos = transform.localPosition;
        newPos.x -= deltax * parallaxFactor;
        newPos.y -= deltay * parallaxFactor;
        transform.localPosition = newPos;
    }

}