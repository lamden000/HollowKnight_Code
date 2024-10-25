using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactorx;
    public float parallaxFactory;
    public void Move(float deltax, float deltay)
    {
        Vector3 newPos = transform.localPosition;
        newPos.x -= deltax * parallaxFactorx;
        newPos.y -= deltay * parallaxFactory;
        transform.localPosition = newPos;
    }

}