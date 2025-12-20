using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartDialogue(DialogueData dialogue)
    {
        Debug.Log("Diyalog Başladı: " + dialogue.speakerName);
        
        // Burada normalde UI (Text) işlemlerini yapacağız.
        // Şimdilik test için konsola yazdırıyoruz.
        foreach (string sentence in dialogue.sentences)
        {
            Debug.Log(sentence);
        }
    }
}