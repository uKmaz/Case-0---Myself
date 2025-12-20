using UnityEngine;
using UnityEngine.Events; // UnityEvent için gerekli

// Seçim yapacağımız ENUM
public enum InteractionMethod
{
    Key_Press_F,     // Yanına gelip F'ye basınca
    Zone_Enter_Auto  // İçine girince otomatik
}

public class InteractableObject : MonoBehaviour
{
    [Header("--- HİKAYE AYARLARI ---")]
    public bool alwaysActive = false;
    public StoryStep activeStep;
    public StoryStep nextStep;
    

    [Header("--- ETKİLEŞİM TÜRÜ ---")]
    [Tooltip("Zone: İçinden geçince çalışır. Key: F tuşu ister.")]
    public InteractionMethod interactMethod; 
    

    [Header("--- İÇERİK ---")]
    public DialogueData dialogue;

    [Header("--- GÖRSEL ---")]
    [Tooltip("Adım aktif olduğunda UZAKTAN DA GÖRÜNECEK obje.")]
    public GameObject visualCueObject; 

    // --- YENİ EKLENEN KISIM ---
    [Header("--- ÖZEL AKSİYONLAR ---")]
    [Tooltip("F Tuşuna basıldığı (veya etkileşim olduğu) AN yapılacaklar.")]
    public UnityEvent onInteract; 
    // ---------------------------

    private bool isInteractable = false;
    private bool isPlayerInRange = false; 
    private Collider2D myCollider;

    private void Awake()
    {
        myCollider = GetComponent<Collider2D>();
        if(myCollider) myCollider.isTrigger = true; 
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStoryStepChanged += CheckVisibility;
            CheckVisibility(GameManager.Instance.currentStep);
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStoryStepChanged -= CheckVisibility;
    }

    private void Update()
    {
        if (isInteractable && isPlayerInRange && interactMethod == InteractionMethod.Key_Press_F)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("BAŞARILI! Etkileşim Başlıyor...");
                Interact();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isInteractable)
        {
            isPlayerInRange = true;
            if (InteractionPromptUI.Instance != null)
            {
                InteractionPromptUI.Instance.Show(this.transform);
            }
            if (interactMethod == InteractionMethod.Zone_Enter_Auto)
            {
                Interact();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (InteractionPromptUI.Instance != null)
            {
                InteractionPromptUI.Instance.Hide();
            }
        }
    }

    private void CheckVisibility(StoryStep currentStoryStep)
    {
        if (currentStoryStep == activeStep)
        {
            isInteractable = true;
            if (myCollider != null) myCollider.enabled = true;
            if (visualCueObject != null) visualCueObject.SetActive(true);
        }
        else
        {
            isInteractable = false;
            isPlayerInRange = false; 
            if (myCollider != null) myCollider.enabled = false;
            if (visualCueObject != null) visualCueObject.SetActive(false);
        }
    }

    public void Interact()
    {
        // 1. Önce Diyalog (Varsa)
        if (dialogue != null) DialogueManager.Instance.StartDialogue(dialogue);

        // --- 2. YENİ: Özel Aksiyonları Çalıştır ---
        // Buraya Inspector'dan eklediğin her şey (Animasyon, Ses vs.) burada çalışır.
        onInteract.Invoke(); 
        // -----------------------------------------

        // 3. En son State'i değiştir (Çünkü State değişince obje kendini kapatabilir)
        if (activeStep != nextStep) GameManager.Instance.UpdateStoryStep(nextStep);
        
        // 4. Görseli kapat
        if (visualCueObject != null) visualCueObject.SetActive(false);
        
        if (InteractionPromptUI.Instance != null) InteractionPromptUI.Instance.Hide();
    }
}