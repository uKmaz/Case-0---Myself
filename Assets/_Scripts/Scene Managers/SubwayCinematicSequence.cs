using UnityEngine;
using System.Collections;

public class SubwayCinematicSequence : MonoBehaviour
{
    [Header("--- ATAMALAR (Zorunlu) ---")]
    public Transform trainActor;            // Hareket edecek tren objesi
    public GameObject blackoutObj;          // Siyah kare (SpriteRenderer olmalı)
    public Camera mainCamera;               // Titreyecek kamera
    public PlayerController playerScript;   // Oyuncunun scripti (Yürütmek için)

    [Header("--- AYARLAR ---")]
    public float trainSpeed = 7f;           // Tren hızı
    public float trainMoveDuration = 4f;    // Tren ne kadar süre gidecek?
    public float playerMoveDuration = 2f;
    public float fadeSpeed = 2f;            // Blackout değişim hızı
    public float shakeIntensity = 0.1f;     // Titreme şiddeti

    [Header("--- SES ---")]
    public string shakeSoundName = "ground_shake"; // Çalacak sesin tam adı

    // Dışarıdan tetiklenecek
    public void StartSequence()
    {
        // Temel bileşenler var mı kontrol et, yoksa hiç başlama
        if (trainActor == null || blackoutObj == null || mainCamera == null)
        {
            Debug.LogError("HATA: SubwayCinematicSequence içinde eksik atamalar var! (Tren, Blackout veya Kamera)");
            return;
        }

        StartCoroutine(SequenceFlow());
    }

    private IEnumerator SequenceFlow()
    {
        // 1. SES VE KAMERA TİTREŞİMİNİ BAŞLAT
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(shakeSoundName);
        }
        else
        {
            Debug.LogWarning("UYARI: AudioManager sahnede bulunamadı, ses çalınamıyor.");
        }

        // Kamera titremesini ayrı bir rutin olarak başlat (Süre boyunca sürsün)
        StartCoroutine(CameraShakeRoutine(trainMoveDuration));

        // 2. OYUNCUYU YÜRÜT (PlayerController'da bu fonksiyon olmalı)
        if (playerScript != null)
        {
            // Oyuncuyu tren süresi boyunca sağa yürüt
            playerScript.ForceWalkRight(playerMoveDuration);
        }
        else
        {
            Debug.LogError("HATA: PlayerController atanmamış! Oyuncu yürütülemiyor.");
        }

        // 3. TREN HAREKETİ VE BLACKOUT AKIŞI
        SpriteRenderer blackSprite = blackoutObj.GetComponent<SpriteRenderer>();

        if (blackSprite == null)
        {
            Debug.LogError("HATA: Blackout objesinde 'SpriteRenderer' yok! 2D modunda olduğundan emin misin?");
            yield break; // Buradan sonrasını çalıştırma
        }

        float timer = 0f;
        
        // Bu döngü 'trainMoveDuration' kadar sürecek
        while (timer < trainMoveDuration)
        {
            float dt = Time.deltaTime;
            timer += dt;

            // A) TRENİ SAĞA İLERLET (2D Transform Hareketi)
            trainActor.Translate(Vector3.right * trainSpeed * dt);

            // B) BLACKOUT EFEKTİ (0 -> 1 -> 0 -> 1 Akışı)
            // İsteğine göre: Önce bir gel-git (0-1-0) yapıp en sonda 1'de kalacak şekilde ayarlayalım.
            // Bu biraz matematiksel, manuel kontrol edelim:
            
            // İlk yarıda (Örn: 1. saniyede) kararıp açılsın (Işık gidip gelme efekti gibi)
            // Son saniyelerde tamamen kararsın.
            
            yield return null; 
        }

        // --- MANUEL FADE AKIŞI ---
        // Döngü içinde karmaşık matematik yerine, olayları sırayla yapalım:
        
        // NOT: Yukarıdaki while döngüsü sadece treni ve süreyi yönetti. 
        // Şimdi görsel efektleri "Wait" kullanmadan, sürenin içine yedirmemiz lazım.
        // Ancak senin isteğin "gel git yapıp en son 1'de kalması".
        // Bunu daha temiz şöyle yaparız:
        
        // Tren ve Player hareketine devam ederken Coroutine'i durdurmadan efekt verelim:
        // (Bu kısım biraz hızlı olacak, tren hala giderken)
        
        // 0 -> 1 (Karar)
        yield return StartCoroutine(FadeAlpha(blackSprite, 0f, 1f, fadeSpeed * 2)); // Hızlı karar
        
        // 1 -> 0 (Açıl)
        yield return StartCoroutine(FadeAlpha(blackSprite, 1f, 0f, fadeSpeed * 2)); // Hızlı açıl

        // Bir anlık bekleme (Sahnede her şey görünüyor)
        yield return new WaitForSeconds(0.5f);

        // 4. FİNAL: SON KEZ KARAR VE ORADA KAL (0 -> 1)
        yield return StartCoroutine(FadeAlpha(blackSprite, 0f, 1f, fadeSpeed));

        // Alpha'nın kesinlikle 1 olduğundan emin ol
        Color c = blackSprite.color;
        c.a = 1f;
        blackSprite.color = c;

        // 5. OYUNU GÜNCELLE
        if (GameManager.Instance != null)
        {
            Debug.Log("Sahne Bitti, StoryStep güncelleniyor...");
             GameManager.Instance.UpdateStoryStep(StoryStep.Baslangic);
        }
        else
        {
            Debug.LogError("CRITICAL: GameManager Instance NULL! Sahne ilerleyemiyor.");
        }
    }

    // --- YARDIMCI FONKSİYONLAR ---

    // Sprite Alpha Değiştirici
    private IEnumerator FadeAlpha(SpriteRenderer rend, float start, float end, float speed)
    {
        float t = 0f;
        Color c = rend.color;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            c.a = Mathf.Lerp(start, end, t);
            rend.color = c;
            yield return null;
        }
        c.a = end;
        rend.color = c;
    }

    // Kamera Titretici
    private IEnumerator CameraShakeRoutine(float duration)
    {
        Vector3 originalPos = mainCamera.transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;

            mainCamera.transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = originalPos; // Yerine geri koy
    }
}