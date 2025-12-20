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
        // 1. DİYALOG KONTROLÜ
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
        {
            // KRİTİK NOKTA: Hareket vektörünü sıfırla ki FixedUpdate yanlış hatırlamasın
            movement = Vector2.zero; 
            
            // Fiziği durdur
            rb.linearVelocity = Vector2.zero; 
            
            // Animasyonu durdur
            if(animator) animator.SetFloat("Speed", 0); 
            
            return; // Buradan çık, aşağıya inip input okuma
        }
        
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
        // İkinci bir güvenlik önlemi: Diyalog varsa fizikte de güç uygulama
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Normal Hareket
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