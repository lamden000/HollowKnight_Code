using System.Collections;
using UnityEngine;

public class Grass : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector3 originalRotation; // Lưu trữ góc quay ban đầu của cỏ
    private bool isSwaying = false;
    private Animator animator; // Animator để quản lý animation
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); // Lấy Animator
        originalRotation = transform.localEulerAngles; // Lưu vị trí gốc

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bool hitLeft = other.transform.position.x < transform.position.x;
            OnHit(hitLeft); // Gọi hàm lắc lư
        }
    }

    public void OnHit(bool hitLeft)
    {
        if (!isSwaying)
        {
            StartCoroutine(Sway(hitLeft));
        }
    }

    private float initialSwaySpeed = 2.5f; // Tốc độ ban đầu (chậm)
    private float returnSwaySpeed = 3.5f; // Tốc độ quay lại (nhanh)
    private IEnumerator Sway(bool hitLeft)
    {

        isSwaying = true;

        // Tạm dừng Animator
        if (animator != null)
        {
            animator.enabled = false;
        }

        float direction = hitLeft ? -1 : 1; // Xác định hướng lắc lư
        float startRotation = 0f; // Góc bắt đầu (0 độ)
        float endRotation = 15f; // Góc kết thúc (10 độ)
        float duration = 2.0f; // Thời gian lắc lư

        // Giai đoạn 1: Lắc chậm ban đầu
        for (float t = 0; t < duration / 2; t += Time.deltaTime * initialSwaySpeed)
        {
            float angle = Mathf.Lerp(startRotation, endRotation, t / (duration / 2)); // Lắc lên
            transform.localEulerAngles = originalRotation + new Vector3(0, 0, direction * angle);
            yield return null;
        }

        // Giai đoạn 2: Quay lại nhanh hơn
        for (float t = 0; t < duration / 2; t += Time.deltaTime * returnSwaySpeed)
        {
            float angle = Mathf.Lerp(endRotation, startRotation, t / (duration / 2)); // Quay lại vị trí gốc
            transform.localEulerAngles = originalRotation + new Vector3(0, 0, direction * angle);
            yield return null;
        }

        

        // Bật lại Animator
        if (animator != null)
        {
            animator.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
        isSwaying = false;
    }


}
