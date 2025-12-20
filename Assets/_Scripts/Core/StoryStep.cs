public enum StoryStep
{
    // --- BÖLÜM 1: SABAH RUTİNİ ---
    Baslangic,              // Oyunun açıldığı ilk an
    
    Yatak_Uyanis,           // Yatakta uyanma anı (F tuşu çıkar)
    Oda_Cikis,     // Yatak odası kapısı (Ok çıkar)
    
    Koridor_Serbest,                // Koridordaki herhangi bir kapının üstüne gelince F tuşu çıkar
    
    Mutfak_Dolaptan_Yemek_Al,  // Dolaptan yemek al
    Mutfak_Yemek_Ye,    // Mutfak kapısı geri dönüş (Ok çıkar)
    Mutfak_Dolaptan_Yemek2,
    Mutfak_Odadan_Cikis,    // Mutfak kapısı geri dönüş (Ok çıkar)
    Koridor_Evden_Cikis,    // Sokak kapısı (Ok çıkar)
    
    
    // --- BÖLÜM 2: SUÇ MAHALLİ (Örnek) ---
    
    
    // --- SON ---
    Final
}