using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Oyun Durumu")]
    [Tooltip("Oyunun şu anki hikaye adımı.")]
    public StoryStep currentStep; 

    // Diğer scriptlerin dinleyeceği Olay (Event)
    public event Action<StoryStep> OnStoryStepChanged;

    private void Awake()
    {
        // Singleton Yapısı (Sahnede tek olduğundan emin oluyoruz)
        if (Instance == null)
        {
            Instance = this;
            // Tek scene çalışacağın için DontDestroyOnLoad şart değil ama
            // ileride restart atarsan bu obje yok olmasın diye kalsın.
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // ARTIK OTOMATİK BAŞLATMIYORUZ 🛑
        // Oyun açıldığında direkt Main Menu state'ine geçiyoruz.
        // Bu state'te player hareket edemez, sadece menü görünür.
        UpdateStoryStep(StoryStep.Menu);
    }

    public void afterBaslangic()
    {
        UpdateStoryStep(StoryStep.Yatak_Uyanis);
    }

    /// <summary>
    /// Hikayeyi bir sonraki adıma geçirir ve herkese haber verir.
    /// </summary>
    /// <param name="newStep">Geçilecek yeni adım</param>
    public void UpdateStoryStep(StoryStep newStep)
    {
        currentStep = newStep;
        Debug.Log($"<color=cyan>[GAME MANAGER]</color> Hikaye Adımı Değişti: <b>{newStep}</b>");

        // Abone olan herkese (StoryEventManager, InteractableObject vs.) haber ver
        OnStoryStepChanged?.Invoke(newStep);
    }

    // Oyun çalışırken Inspector'dan 'currentStep'i elle değiştirip,
    // Scriptin ismine sağ tıklayıp "Force Update Step" dersen oraya ışınlanır/geçersin.
    [ContextMenu("Force Update Step (Debug)")]
    public void DebugForceUpdate()
    {
        UpdateStoryStep(currentStep);
    }
}