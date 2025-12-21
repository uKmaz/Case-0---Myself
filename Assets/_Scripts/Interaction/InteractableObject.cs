using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic; // List için gerekli

public enum InteractionMethod
{
    Key_Press_F,
    Zone_Enter_Auto
}

public class InteractableObject : MonoBehaviour
{
    // --- ÖZEL DURUM TANIMI (STRUCT) ---
    // Inspector'da liste elemanı olarak görünecek kutucuk yapısı
    [System.Serializable]
    public struct StateScenario
    {
        public string scenarioName;      // Karışmasın diye isim (Örn: "Sabah Durumu")
        public StoryStep requiredStep;   // Hangi adımda çalışacak?
        public StoryStep nextStep;       // Bitince hangi adıma geçecek?
        public DialogueData dialogue;    // Hangi diyaloğu okuyacak?
        public UnityEvent onInteract;    // Ne yapacak? (Örn: Kolye sprite'ını aç)
    }

    [Header("--- GENEL AYARLAR ---")]
    [Tooltip("İşaretlenirse, özel bir state bulunamasa bile obje çalışır (Navigation modu).")]
    public bool alwaysActive = false;


    [Header("--- ETKİLEŞİM TÜRÜ ---")]
    public InteractionMethod interactMethod;

    [Header("--- GÖRSEL ---")]
    public GameObject visualCueObject;

    // --- YENİ LİSTE SİSTEMİ ---
    [Header("--- SENARYOLAR (Özel Durumlar) ---")]
    public List<StateScenario> specificStates; 

    [Header("--- VARSAYILAN AKSİYON (Fallback) ---")]
    [Tooltip("Eğer Always Active ise ve yukarıdaki hiçbir State uymuyorsa burası çalışır (Örn: Sadece Işınlanma).")]
    public UnityEvent defaultOnInteract;
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
                Interact();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isInteractable)
        {
            isPlayerInRange = true;
            if (InteractionPromptUI.Instance != null) InteractionPromptUI.Instance.Show(this.transform);
            
            if (interactMethod == InteractionMethod.Zone_Enter_Auto) Interact();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (InteractionPromptUI.Instance != null) InteractionPromptUI.Instance.Hide();
        }
    }

    // --- GÖRÜNÜRLÜK KONTROLÜ (GÜNCELLENDİ) ---
    private void CheckVisibility(StoryStep currentStep)
    {
        bool specialStateFound = false;

        // 1. Önce listede bu adım var mı diye bak
        foreach (var scenario in specificStates)
        {
            if (scenario.requiredStep == currentStep)
            {
                specialStateFound = true;
                break;
            }
        }

        if (specialStateFound)
        {
            // DURUM 1: ÖZEL HİKAYE ANI
            // Hem etkileşim açık, hem de Visual Cue (Parlama) açık.
            isInteractable = true;
            if (myCollider != null) myCollider.enabled = true;
            if (visualCueObject != null) visualCueObject.SetActive(true);
        }
        else if (alwaysActive)
        {
            // DURUM 2: NAVİGASYON MODU (Always Active)
            // Etkileşim açık AMA Visual Cue KAPALI.
            isInteractable = true;
            if (myCollider != null) myCollider.enabled = true;
            if (visualCueObject != null) visualCueObject.SetActive(false); // <-- Fark burada
        }
        else
        {
            // DURUM 3: PASİF
            isInteractable = false;
            isPlayerInRange = false; 
            if (myCollider != null) myCollider.enabled = false;
            if (visualCueObject != null) visualCueObject.SetActive(false);
        }
    }
    // --- ETKİLEŞİM MANTIĞI (GÜNCELLENDİ) ---
    public void Interact()
    {
        StoryStep currentStep = GameManager.Instance.currentStep;
        bool matchFound = false;

        // 1. Özel Senaryo Kontrolü
        foreach (var scenario in specificStates)
        {
            if (scenario.requiredStep == currentStep)
            {
                matchFound = true;
                
                if (scenario.dialogue != null) DialogueManager.Instance.StartDialogue(scenario.dialogue);
                
                scenario.onInteract.Invoke();

                if (scenario.nextStep != currentStep) 
                {
                    GameManager.Instance.UpdateStoryStep(scenario.nextStep);
                    // State değişince CheckVisibility otomatik çalışır ve Cue kapanır.
                }
                break; 
            }
        }

        // 2. Varsayılan (Navigation) Kontrolü
        if (!matchFound && alwaysActive)
        {
            // Sadece ışınlanma vs. çalışır, Visual Cue zaten kapalıdır.
            defaultOnInteract.Invoke();
        }
        
        if (InteractionPromptUI.Instance != null) InteractionPromptUI.Instance.Hide();
    }
}