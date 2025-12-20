using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "New Dialogue", menuName = "DuoCeng/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [Header("--- HIZLI IMPORT ---")]
    [Tooltip("Diyalogları yazdığın .txt dosyasını buraya sürükle")]
    public TextAsset textFile;
    
    [System.Serializable]
    public struct DialogueLine
    {
        public string speakerName;     // Kim konuşuyor? (Dedektif / Tanık)
        [TextArea(3, 10)]
        public string sentence;        // Ne diyor?
    }

    public List<DialogueLine> conversation; // Cümleler listesi
    
    // --- SİHİRLİ KISIM ---
    // Bu fonksiyon Inspector'da sağ tıklayınca çalışır.
    // TXT dosyasını okur ve aşağıdaki listeyi otomatik doldurur.
    [ContextMenu("TXT'den Aktar (Import)")]
    public void ImportFromTextFile()
    {
        if (textFile == null)
        {
            Debug.LogError("Lütfen önce 'Text File' kutusuna bir .txt dosyası sürükleyin!");
            return;
        }

        conversation = new List<DialogueLine>();
        
        // Satır satır oku
        string[] lines = textFile.text.Split('\n');

        foreach (string line in lines)
        {
            // Boş satırları atla
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Split(':');

            if (parts.Length >= 2)
            {
                DialogueLine newLine = new DialogueLine();
                newLine.speakerName = parts[0].Trim(); // İsim (Boşlukları temizle)
                newLine.sentence = parts[1].Trim();    // Cümle (Boşlukları temizle)
                
                conversation.Add(newLine);
            }
        }

        Debug.Log($"Başarıyla {conversation.Count} satır diyalog aktarıldı!");
    }
}