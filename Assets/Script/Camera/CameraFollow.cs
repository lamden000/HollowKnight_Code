using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;   
    [SerializeField] private float flipRotationTime=0.5f;

    private Coroutine turnCoroutine;

    private PlayerScript playerScript;

    private bool isFacingRight;

    private void Awake()
    {
        playerScript = playerTransform.gameObject.GetComponent<PlayerScript>();
        isFacingRight = playerScript.isFacingRight;
    }

    private void Update()
    {
        transform.position = new Vector3(playerTransform.position.x,playerTransform.position.y,-30);
    }

    public void CallTurn()
    {
        turnCoroutine = StartCoroutine(FlipLerp());
    }

    private IEnumerator FlipLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;
         
        float elapsedTime = 0;
        while (elapsedTime < flipRotationTime)
        {
            elapsedTime += Time.deltaTime;
            yRotation =Mathf.Lerp(startRotation, endRotationAmount,elapsedTime/flipRotationTime);
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }
    }

    private float DetermineEndRotation()
    {
        isFacingRight = !isFacingRight;
        if (isFacingRight)
        {
            return 0f;
        }
        else
        {
            return 180f;
        }
    }
}
