using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI Bileşenleri")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;      // İsim Alanı
    public TextMeshProUGUI dialogueText;  // Metin Alanı
    public GameObject continueIcon;

    [Header("Ayarlar")]
    public float typingSpeed = 0.04f;
    
    [Header("Görsel Ayarlar")]
    public Image portraitImage;       // UI'daki Karakter Resmi (Canvas -> Image)
    public CharacterDatabase charDB;  // Oluşturduğumuz veritabanı dosyası

    public bool IsDialogueActive { get; private set; } = false;

    // DEĞİŞİKLİK 1: Kuyruk artık string değil, DialogueLine tutuyor
    private Queue<DialogueData.DialogueLine> lines; 
    
    private string currentFullSentence; // O an yazılan cümlenin tam hali
    private bool isTyping = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Kuyruğu yeni tiple başlat
        lines = new Queue<DialogueData.DialogueLine>();
        
        if(dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (IsDialogueActive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isTyping)
                {
                    StopAllCoroutines();
                    dialogueText.text = currentFullSentence;
                    isTyping = false;
                    if(continueIcon) continueIcon.SetActive(true);
                }
                else
                {
                    DisplayNextLine();
                }
            }
        }
    }

    public void StartDialogue(DialogueData dialogueData)
    {
        IsDialogueActive = true;
        dialoguePanel.SetActive(true);
        if(continueIcon) continueIcon.SetActive(false);

        // Kuyruğu temizle
        lines.Clear();

        // DEĞİŞİKLİK 2: Yeni struct yapısını kuyruğa ekle
        foreach (var line in dialogueData.conversation)
        {
            lines.Enqueue(line);
        }

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueData.DialogueLine currentLine = lines.Dequeue();
        
        // --- YENİ MANTIK BURADA ---

        Sprite charSprite = null;
        
        // 1. Veritabanından resmi çekmeye çalış
        if (charDB != null)
        {
            charSprite = charDB.GetPortrait(currentLine.speakerName);
        }

        if (charSprite != null)
        {
            // DURUM A: RESİM VAR (Splash Art)
            portraitImage.sprite = charSprite;
            portraitImage.gameObject.SetActive(true); // Resmi göster
            
            nameText.gameObject.SetActive(false);     // İsmi GİZLE (Çünkü resim var)
        }
        else
        {
            // DURUM B: RESİM YOK (Dış Ses / Bilinmeyen)
            portraitImage.gameObject.SetActive(false); // Resmi gizle
            
            nameText.gameObject.SetActive(true);       // İsmi GÖSTER
            nameText.text = currentLine.speakerName;   // İsmi yaz
        }
        
        // ---------------------------

        currentFullSentence = currentLine.sentence;
        
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentFullSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        if(continueIcon) continueIcon.SetActive(false);

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        if(continueIcon) continueIcon.SetActive(true);
    }

    void EndDialogue()
    {
        IsDialogueActive = false;
        dialoguePanel.SetActive(false);
    }
}