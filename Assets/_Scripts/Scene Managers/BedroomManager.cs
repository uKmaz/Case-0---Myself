using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Listeler için gerekli
using UnityEngine.Events; // Eventler için gerekli

public class BedroomManager : MonoBehaviour
{
    // --- ÖZEL VERİ TİPİMİZ ---
    [System.Serializable] // Bu satır sayesinde Inspector'da içini görebilirsin
    public class StateScenario
    {
        public string senaryoAdi;           // Karışmasın diye isim (Örn: "Sabah Uyanış")
        public StoryStep hedefState;        // Hangi state'e geçiş bu?
        public float fadeSuresi = 2.0f;     // Bu geçişe özel süre
        public UnityEvent ozelEventler;     // Sadece bu state'e geçişte çalışacak fonksiyonlar
    }

    [Header("--- AYARLAR ---")]
    public GameObject blackoutObj;          // Siyah kare
    
    // Listemiz: Inspector'dan istediğin kadar senaryo ekle
    public List<StateScenario> senaryolar; 

    private bool isFading = false;

    // --- DIŞARIDAN ÇAĞIRILACAK FONKSİYON ---
    public void StartSequenceByIndex(int stateIndex)
    {
        // Gelen tam sayıyı (0, 1, 2...) Enum'a çeviriyoruz (Casting)
        StoryStep donusturulenState = (StoryStep)stateIndex;

        // Asıl fonksiyonu çağırıyoruz
        StartSequence(donusturulenState);
    }
    public void StartSequence(StoryStep istenenState)
    {
        if (blackoutObj == null)
        {
            Debug.LogError("HATA: Blackout Objesi atanmamış!");
            return;
        }

        if (isFading) return;

        // Listeden istenen state'e uygun senaryoyu buluyoruz
        StateScenario secilenSenaryo = senaryolar.Find(x => x.hedefState == istenenState);

        if (secilenSenaryo != null)
        {
            StartCoroutine(SenaryoyuOynat(secilenSenaryo));
        }
        else
        {
            Debug.LogError($"HATA: '{istenenState}' için bir senaryo tanımlanmamış! Listeyi kontrol et.");
        }
    }
    
    private IEnumerator SenaryoyuOynat(StateScenario senaryo)
    {
        isFading = true;
        
        SpriteRenderer rend = blackoutObj.GetComponent<SpriteRenderer>();
        if (rend == null) yield break; // Güvenlik önlemi

        // --- ADIM 1: KARARMA (Senaryoya özel süreyle) ---
        float timer = 0f;
        Color c = rend.color;
        c.a = 0f;
        rend.color = c;

        while (timer < senaryo.fadeSuresi)
        {
            timer += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, timer / senaryo.fadeSuresi);
            rend.color = c;
            yield return null;
        }

        c.a = 1f;
        rend.color = c;

        // --- ADIM 2: STATE GÜNCELLEME ---
        Debug.Log($"Fade bitti. State güncelleniyor: {senaryo.hedefState}");
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateStoryStep(senaryo.hedefState);
        }

        // --- ADIM 3: ÖZEL EVENTLERİ ÇALIŞTIR ---
        // Sadece bu senaryoya ait olanları çalıştırır
        if (senaryo.ozelEventler != null)
        {
            senaryo.ozelEventler.Invoke();
        }

        // isFading false yapmıyoruz, çünkü sahne değişiyor veya kararmış kalıyor.
        // Eğer geri açılacaksa burada isFading = false yapabilirsin.
    }
}