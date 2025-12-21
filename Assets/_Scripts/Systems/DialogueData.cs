using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor; // Editör komutlarını kullanmak için gerekli
#endif

[CreateAssetMenu(fileName = "New Dialogue", menuName = "DuoCeng/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [Header("--- HIZLI IMPORT ---")]
    [Tooltip("Diyalogları yazdığın .txt dosyasını buraya sürükle")]
    public TextAsset textFile;
    
    [System.Serializable]
    public struct DialogueLine
    {
        public string speakerName;     // Kim konuşuyor?
        [TextArea(3, 10)]
        public string sentence;        // Ne diyor?
    }

    public List<DialogueLine> conversation; 

    [ContextMenu("TXT'den Aktar (Import)")]
    public void ImportFromTextFile()
    {
        if (textFile == null)
        {
            Debug.LogError("HATA: Lütfen önce 'Text File' kutusuna bir .txt dosyası sürükleyin!");
            return;
        }

        conversation = new List<DialogueLine>();
        
        // Windows ve Mac satır sonu uyumluluğu için temizlik
        string fileContent = textFile.text.Replace("\r\n", "\n"); 
        string[] lines = fileContent.Split('\n');

        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            // İlk ':' işaretinden böl (Saat 12:30 gibi ifadeler bozulmasın diye limit: 2)
            string[] parts = line.Split(new char[] { ':' }, 2);

            if (parts.Length >= 2)
            {
                DialogueLine newLine = new DialogueLine();
                newLine.speakerName = parts[0].Trim(); 
                newLine.sentence = parts[1].Trim();    
                
                conversation.Add(newLine);
            }
        }

        Debug.Log($"Başarıyla {conversation.Count} satır diyalog aktarıldı!");

        // --- İŞTE EKSİK OLAN SİHİRLİ KOD ---
        // Unity'e diyoruz ki: "Bu obje üzerinde değişiklik yaptım, bunu kaydet ve Inspector'ı yenile."
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
}