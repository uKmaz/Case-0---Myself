using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ayarlar")]
    public float moveSpeed = 5f;

    [Header("Bileşenler")]
    public Rigidbody2D rb;
    public Animator animator; // Animasyon varsa bağla, yoksa boş kalsın
    
    private Vector2 movement;
    private bool facingRight = true;

    void Update()
    {
        // Eğer diyalog varsa hareketi durdurabilirsin (İstersen burayı aktif et)
        // if (DialogueManager.Instance.IsDialogueActive) { rb.velocity = Vector2.zero; return; }

        // Girişleri al (A/D veya Sol/Sağ Ok)
        movement.x = Input.GetAxisRaw("Horizontal");

        // Animasyon parametresi gönder (Speed > 0 ise yürüme animasyonu)
        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(movement.x));
        }

        // Karakteri Döndür (Flip)
        if (movement.x > 0 && !facingRight) Flip();
        else if (movement.x < 0 && facingRight) Flip();
    }

    void FixedUpdate()
    {
        // Fiziksel hareket
        rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
    }

    // Karakterin yönünü değiştiren fonksiyon
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1; // X eksenini ters çevir
        transform.localScale = scaler;
    }
}