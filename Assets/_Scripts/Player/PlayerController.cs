using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ayarlar")]
    public float moveSpeed = 5f;

    [Header("Bileşenler")]
    public Rigidbody2D rb;
    public Animator animator;         // Detective Sprite üzerindeki animator
    public Animator criminalAnimator; // Criminal Sprite üzerindeki animator
    public ParticleSystem faceSplashEffect;
    
    private Vector2 movement;
    private bool isCinematicMode = false;

    void Update()
    {
        if (isCinematicMode) return;
        // 1. DİYALOG KONTROLÜ
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
        {
            movement = Vector2.zero; 
            rb.linearVelocity = Vector2.zero; 

            // Hangi animator aktifse onu durdur
            if (animator.gameObject.activeInHierarchy) animator.SetBool("IsMoving", false);
            if (criminalAnimator.gameObject.activeInHierarchy) criminalAnimator.SetBool("IsMoving", false);
            
            return; 
        }
    
        // 2. INPUT OKUMA
        float moveInput = Input.GetAxisRaw("Horizontal");
        movement = new Vector2(moveInput, 0); 

        // 3. ANİMASYON KONTROLÜ
        bool IsMoving = (moveInput != 0);

        // Dedektif aktifse onun animasyonunu yönet
        if (animator.gameObject.activeInHierarchy)
        {
            animator.SetBool("IsMoving", IsMoving);
        }

        // Criminal aktifse onun animasyonunu yönet
        if (criminalAnimator.gameObject.activeInHierarchy)
        {
            criminalAnimator.SetBool("IsMoving", IsMoving);
        }
    
        // 4. YÖN DÖNDÜRME
        if (moveInput > 0) 
        {
            // Sağa bak
            if (animator.gameObject.activeInHierarchy) animator.transform.localScale = Vector3.one;
            if (criminalAnimator.gameObject.activeInHierarchy) criminalAnimator.transform.localScale = Vector3.one;
        }
        else if (moveInput < 0) 
        {
            // Sola bak
            if (animator.gameObject.activeInHierarchy) animator.transform.localScale = new Vector3(-1, 1, 1);
            if (criminalAnimator.gameObject.activeInHierarchy) criminalAnimator.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    
    void FixedUpdate()
    {
        if (isCinematicMode) return;
        
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = new Vector2(movement.x * moveSpeed, rb.linearVelocity.y);
    }

    // Bu eski fonksiyonu kullanmıyorsan silebilirsin, scale yöntemini kullanıyorsun çünkü.
    /*
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1; 
        transform.localScale = scaler;
    }
    */
    
    public void ForceWalkRight(float duration)
    {
        StartCoroutine(AutoWalkRoutine(duration));
    }

    private IEnumerator AutoWalkRoutine(float duration)
    {
        isCinematicMode = true;
        float timer = 0f;
        this.rb.simulated = false;
        // Sadece aktif olan animatörü yürüt
        if (animator.gameObject.activeInHierarchy) animator.SetBool("IsMoving", true);
        if (criminalAnimator.gameObject.activeInHierarchy) criminalAnimator.SetBool("IsMoving", true);
    
        while (timer < duration)
        {
            timer += Time.deltaTime;
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Süre bitince yine sadece aktif olanı durdur
        if (animator.gameObject.activeInHierarchy) animator.SetBool("IsMoving", false);
        if (criminalAnimator.gameObject.activeInHierarchy) criminalAnimator.SetBool("IsMoving", false);
        isCinematicMode  = false;
        this.rb.simulated = true;
    }

    public void faceWash()
    {
        if (faceSplashEffect == null)
        {
            Debug.LogWarning("Particle System atanmamış!");
            return;
        }

        if (!faceSplashEffect.isPlaying)
        {
            Debug.Log("Yüz yıkanıyor...");
            faceSplashEffect.Play();
        }
    }
}