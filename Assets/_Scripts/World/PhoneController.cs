using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PhoneController : MonoBehaviour
{
    [Header("--- SES & RİTİM AYARLARI ---")]
    public AudioClip ringSound;       // Kısa "Zırr" sesi (Tek seferlik)
    public float shakeDuration = 1.0f; // Ne kadar süre titresin? (Sesin uzunluğu kadar yap)
    public float waitDuration = 2.0f;  // İki çaldırma arası bekleme süresi

    [Header("--- TİTREŞİM AYARLARI ---")]
    public float shakeIntensity = 0.05f; 
    
    private AudioSource audioSource;
    private Vector3 originalPos;
    private bool isShaking = false; // Şu an titriyor mu?
    private Coroutine ringCoroutine; // Döngüyü durdurmak için referans

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false; // Döngüyü biz elle yapacağız
        originalPos = transform.localPosition; 
    }

    private void Update()
    {
        // Sadece "Çalma Anında" titret
        if (isShaking)
        {
            Vector3 randomOffset = Random.insideUnitCircle * shakeIntensity;
            transform.localPosition = originalPos + randomOffset;
        }
        else
        {
            // Titremiyorsa tam yerinde dursun
            transform.localPosition = originalPos;
        }
    }

    public void StartRinging()
    {
        // Zaten çalıyorsa tekrar başlatma
        if (ringCoroutine != null) return;

        // Ritmi başlat
        ringCoroutine = StartCoroutine(RingRoutine());
    }

    public void StopRinging()
    {
        if (ringCoroutine != null)
        {
            StopCoroutine(ringCoroutine);
            ringCoroutine = null;
        }

        isShaking = false;
        audioSource.Stop();
        transform.localPosition = originalPos; // Yerine sabitle
    }

    // --- ZAMANLAMA DÖNGÜSÜ ---
    IEnumerator RingRoutine()
    {
        while (true) // Sonsuz döngü (cevaplanana kadar)
        {
            // 1. ADIM: ÇAL VE TİTRE
            audioSource.PlayOneShot(ringSound); // Sesi çal
            isShaking = true;                   // Titremeyi aç
            
            // Sesin süresi kadar bekle (veya elle girdiğin süre kadar)
            yield return new WaitForSeconds(shakeDuration);

            // 2. ADIM: SUS VE BEKLE
            isShaking = false;                  // Titremeyi kapat
            transform.localPosition = originalPos; // Düzelt
            
            // Bir sonraki çalışa kadar bekle
            yield return new WaitForSeconds(waitDuration);
        }
    }

    [ContextMenu("Test - Başlat")]
    public void TestStart() => StartRinging();
    [ContextMenu("Test - Durdur")]
    public void TestStop() => StopRinging();
}