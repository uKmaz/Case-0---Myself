using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class MainMenuManager : MonoBehaviour
{
    [Header("--- GÖRSEL AYARLAR (Sahne) ---")]
    public GameObject blackoutObj;      // Siyah olacak 2D obje (SpriteRenderer)
    
    [Header("--- UI AYARLARI (Canvas) ---")]
    public CanvasGroup menuCanvasGroup; // Textlerin olduğu Canvas veya Panel
    public float fadeDuration = 2.0f;   // İşlem süresi

    [Header("--- OLAYLAR ---")]
    public UnityEvent onFadeComplete; 

    private bool isFading = false;

    public void StartGameSequence()
    {
        // 1. NULL KONTROLÜ
        if (blackoutObj == null)
        {
            Debug.LogError("HATA: 'Blackout Obj' atanmamış!");
            return;
        }

        if (isFading) return;

        StartCoroutine(FadeAndExecuteRoutine());
    }

    private IEnumerator FadeAndExecuteRoutine()
    {
        isFading = true;
        
        SpriteRenderer blackRend = blackoutObj.GetComponent<SpriteRenderer>();
        if (blackRend == null)
        {
            Debug.LogError("HATA: Blackout objesinde SpriteRenderer yok!");
            isFading = false;
            yield break;
        }

        float timer = 0f;
        
        // Başlangıç değerlerini ayarla
        Color c = blackRend.color;
        c.a = 0f; // Siyah kare görünmez başlar
        blackRend.color = c;

        // UI başlangıçta tam görünür olsun (Eğer atanmışsa)
        if (menuCanvasGroup != null) menuCanvasGroup.alpha = 1f;

        // --- DÖNGÜ: HEM KARARMA HEM SİLİNME AYNI ANDA ---
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration; // 0 ile 1 arasında ilerleme değeri

            // 1. Siyah Kare: Şeffaftan Siyaha (0 -> 1)
            c.a = Mathf.Lerp(0f, 1f, progress);
            blackRend.color = c;

            // 2. Canvas/Text: Görünürden Şeffafa (1 -> 0)
            if (menuCanvasGroup != null)
            {
                menuCanvasGroup.alpha = Mathf.Lerp(1f, 0f, progress);
            }

            yield return null;
        }

        // --- GARANTİLEME ---
        c.a = 1f;
        blackRend.color = c; // Tam siyah

        // --- UI YOK ETME ---
        if (menuCanvasGroup != null)
        {
            menuCanvasGroup.alpha = 0f; // Önce tam görünmez yap
            
            // Textlerin olduğu Canvas objesini tamamen yok et
            Destroy(menuCanvasGroup.gameObject); 
            Debug.Log("Canvas yok edildi.");
        }

        // --- BİTİŞ & OLAY TETİKLEME ---
        Debug.Log("Sequence bitti, diğer fonksiyon çalışıyor...");

        if (onFadeComplete != null)
        {
            onFadeComplete.Invoke();
        }
    }
}