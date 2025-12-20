using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[System.Serializable]
public struct StoryEvent
{
    public string name;             // Editörde ne olduğunu anlaman için not (Örn: "Mutfak Geçişi")
    public StoryStep triggerStep;   // Hangi adımda çalışacak? (Örn: Koridor_Mutfak_Giris)
    public UnityEvent onStepStart;  // Ne yapacak? (Işınla, Animasyon Oynat vs.)
}

public class StoryEventManager : MonoBehaviour
{
    [Header("Olay Listesi")]
    public List<StoryEvent> storyEvents; // Burayı Inspector'dan dolduracaksın

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            // 1. Abonelik başlat (Gelecek değişiklikler için)
            GameManager.Instance.OnStoryStepChanged += CheckEvents;
            
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStoryStepChanged -= CheckEvents;
        }
    }

    private void CheckEvents(StoryStep newStep)
    {
        foreach (var storyEvent in storyEvents)
        {
            // Eğer şu anki adım, listedeki tetikleyiciyle eşleşiyorsa:
            if (storyEvent.triggerStep == newStep)
            {
                Debug.Log($"OLAY TETİKLENDİ: {storyEvent.name}");
                storyEvent.onStepStart.Invoke(); // Aksiyonları çalıştır
            }
        }
    }
}