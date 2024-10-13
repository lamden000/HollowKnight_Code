using UnityEngine;

[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(float deltaX, float deltaY);
    public ParallaxCameraDelegate onCameraTranslate;

    private float oldPositionx;
    private float oldPositiony;
    void Start()
    {
        oldPositionx = transform.position.x;
        oldPositiony = transform.position.y;
    }

    void Update()
    {
        if (transform.position.x != oldPositionx)
        {
            if (onCameraTranslate != null)
            {
                float delta = oldPositionx - transform.position.x;
                onCameraTranslate(delta,0);
            }

            oldPositionx = transform.position.x;
        }
        if (transform.position.y != oldPositiony)
        {
            if (onCameraTranslate != null)
            {
                float delta = oldPositiony - transform.position.y;
                onCameraTranslate(0, delta);
            }

            oldPositiony = transform.position.y;
        }
    }
}