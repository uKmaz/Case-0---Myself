using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterDB", menuName = "DuoCeng/Character Database")]
public class CharacterDatabase : ScriptableObject
{
    [System.Serializable]
    public struct CharacterProfile
    {
        public string characterId; // TXT'de yazdığın isim (Örn: "Dedektif")
        public Sprite portrait;    // Ekranda çıkacak resim
    }

    public List<CharacterProfile> characters;

    // İsme göre resmi bulan fonksiyon
    public Sprite GetPortrait(string name)
    {
        foreach (var charProfile in characters)
        {
            // Büyük-küçük harf duyarsız kontrol (Emre == emre)
            if (charProfile.characterId.Trim().ToLower() == name.Trim().ToLower())
            {
                return charProfile.portrait;
            }
        }
        return null; // Bulamazsa null döner
    }
}