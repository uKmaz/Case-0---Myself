using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "DuoCeng/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public string speakerName; // Konuşan (Dedektif, ???)
    
    [TextArea(3, 10)]
    public string[] sentences; // Cümleler
}