using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [Header("Bileşenler")]
    public Animator playerAnimator; // Karakterin animatörü varsa buraya sürükle
    
    private Rigidbody2D rb;
    private PlayerController controller;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
    }

    // --- 1. IŞINLANMA (Teleport) ---
    // StoryEventManager'dan hedef objeyi (SpawnPoint) sürükleyeceksin
    public void TeleportTo(Transform targetPoint)
    {
        if (targetPoint != null)
        {
            transform.position = targetPoint.position;
            
            // Işınlanınca fiziksel savrulmayı sıfırla
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }
    }

    // --- 2. ANİMASYON ---
    // String olarak animasyon adını yazacaksın (Örn: "GetUp")
    public void PlayAnimationTrigger(string triggerName)
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger(triggerName);
        }
    }

    // --- 3. PARENT İŞLEMLERİ ---
    // Yataktan kalkarken veya bir şeyden ayrılırken
    public void DetachFromParent()
    {
        transform.SetParent(null);
    }

    // Bir aracın veya yatağın içine girerken
    public void SetNewParent(Transform newParent)
    {
        transform.SetParent(newParent);
        transform.localPosition = Vector3.zero; // Tam ortasına oturt
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }

    // --- 4. HAREKET KİLİTLEME ---
    // Ara sahnelerde oyuncu yürüyemesin diye (True = Kilitli, False = Serbest)
    public void SetMovementLock(bool isLocked)
    {
        if (controller != null)
        {
            // Scripti kapatırsak input alamaz
            controller.enabled = !isLocked; 
            
            // Kilitlenince olduğu yerde dursun, kaymasın
            if (isLocked && rb != null) rb.linearVelocity = Vector2.zero;
        }
    }
    
    // --- 5. USER UYUTMA ---
    // TRUE UYKUDA / FALSE UYANIK (AKTİF)
    // Ara sahnelerde oyuncu yürüyemesin diye (True = Kilitli, False = Serbest)
    public void SetPlayerActiveState(bool isActive)
    {
        // 1. Görüntüyü Aç/Kapa
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = isActive;

        // 2. Kontrolü Aç/Kapa (Yürüyemesin)
        var ctrl = GetComponent<PlayerController>();
        if (ctrl != null) ctrl.enabled = isActive;

        // 3. Fiziksel olarak durdur (Kaymasın)
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null && !isActive) rb.linearVelocity = Vector2.zero;
    }
}