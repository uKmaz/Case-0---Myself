import os

# Klasör adı
folder_name = "Oyun_Diyaloglari_Sirali"

# Klasör yoksa oluştur
if not os.path.exists(folder_name):
    os.makedirs(folder_name)

# Dosya isimleri (Numaralı) ve içerikleri
dialogues = {
    "01_Metro_Night.txt": 
"""Suit:Son metroyu kaçırmasam iyi olur.
Suit:Hey sen de kimsin?""",

    "02_House_Morning.txt": 
"""Dedektif:Artık uyanmalıyım.""",

    "03_Phone_Ring.txt": 
"""Police Officer:Günaydın dedektif. Dün gece metroda bir cinayet işlenmiş…
Police Officer:Olay yerinde seni bekliyoruz.
Dedektif:Tamamdır, birazdan ordayım.""",

    "04_Corridor_1.txt": 
"""Dedektif:Banyoya gidip hazırlanayım.""",

    "05_Bathroom_1.txt": 
"""Dedektif:Yüzümü yıkamalıyım.
Dedektif:Bu kırık aynayı artık değiştimeliyim.
Dedektif:Oh, iyi geldi.""",

    "06_Bathroom_2.txt": 
"""Dedektif:Bu mendil de neyin nesi?
Dedektif:Buna ayıracak vaktim yok, çıkmalıyım.""",

    "07_Metro_Morning.txt": 
"""Polis:Geciktin!
Dedektif:Sana da günaydın eski dostum.
Polis:Yardımcın orda bekliyor, detayları ondan öğrenebilirsin.""",

    "08_Metro_Morning_Talk.txt": 
"""Asistan:Günaydın dedektif. 
Dedektif:Günaydın. Neler olmuş anlat hemen.
Asistan:Kurban 35 yaşında, erkek. Dün gece iş arkadaşlarıyla bir şeyler içmiş…
Asistan:Evine dönmek için metro beklediği sırada katilimiz gelmiş…
Asistan:Olay gece yarısı gerçekleşmiş. Katilimiz adamı öldürmek için özellikle son metroyu beklemiş.
Dedektif:Tamamdır Umut. Teşekkürler, bundan sonrasında ben devam ederim.""",

    "09_Before_First_Sleep.txt": 
"""Dedektif:Yorucu bir gün oldu. Sıradan bir cinayet vakası ama sebebini biraz daha araştırmalıyım. 
Dedektif:Son zamanlarda bu kadar sık suç işlenmesinin bir ortak noktası olmalı.""",

    "10_Jewelry_Store_Night.txt": 
"""Suçlu:Ben de seni arıyordum. İşte buradasın. """,

    "11_Second_Morning.txt": 
"""Dedektif:Yeni bir sabah. Çok yorgunum.
Police Officer:Günaydın dedektif. Suçlular bizim gibi uyumuyor…
Police Officer:Şehir merkezindeki mücevher dükkanı soyulmuş. Olay yerinde seni bekliyoruz.
Dedektif:Tamamdır, birazdan ordayım.""",

    "12_Living_Room.txt": 
"""Dedektif:Bu kolye nerden geldi buraya? 
Dedektif:Annem bırakmış olmalı. Hafta sonu geldiğinde unutmuş galiba.
Dedektif:Mutfağa gidip bir şeyler atıştırayım. Sonra çıkarım.""",

    "13_Kitchen_1.txt": 
"""Dedektif:Çok acıkmışım. Hızlıca bir şeyler yesem iyi olur.""",

    "14_Kitchen_2.txt": 
"""Dedektif:Gitme vakti.""",

    "15_Jewelry_Store_Morning.txt": 
"""Kuyumcu:Ah dedektif iyi ki geldin.
Dedektif:Selamlar.
Asistan:Günaydın dedektif. Hemen neler olduğunu anlatayım.
Dedektif:Dinliyorum.
Asistan:Dün gece yarısı bir hırsız gelip vitrinden birkaç mücevher ve çok değerli bir kolyeyi çalmış… 
Asistan:Alarm çalmaya başlayınca da kimseye yakalanmadan hızla olay yerinden ayrılmış.
Dedektif:Kamera kayıtlarını incelediniz mi?
Asistan:Evet, kar maskeli bir erkek. Zayıf ve fazla uzun sayılmaz… Önceki cinayetteki şüpheliyle benziyorlar.
Asistan:Herhangi bir delil için incelemeye devam ediyoruz. """,

    "16_Second_Night_Sleep.txt": 
"""Dedektif:Çalınan ziynet eşyaları çok para ediyor olmalı… 
Dedektif:Hepsini bir an önce bulsak iyi olur…
Dedektif:Bugünlük bu kadar yeter. Yarın düşünmeye kaldığım yerden devam ederim.""",

    "17_Dumpster_Night_1.txt": 
"""Evsiz:Bu saatlerde buralarda gezmemelisin genç adam.""",

    "18_Dumpster_Night_2.txt": 
"""Evsiz:Ne yaptığını sanıyorsun sen. UZAK DUR BENDEN!""",

    "19_Third_Morning_1.txt": 
"""Dedektif:Gözlüğüm nerede benim, hiçbir şey göremiyorum. """,

    "20_Third_Morning_2.txt": 
"""Police Officer:Günaydın dedektif. Gece yine bir cinayet işlenmiş…
Police Officer:Seninle sürekli gittiğimiz barın arkasındaki ara sokakta. Bir evsiz öldürülmüş…
Police Officer:Olay yerine hızlı gel, lanet olasıca şimdiden kokmaya başlamış.""",

    "21_Corridor_2.txt": 
"""Dedektif:Sürekli yorgun uyanmak fazla sinir bozucu. En kısa zamanda bir doktora gitmeliyim.
Dedektif:Gözlüksüz önümü görmek çok zor. Acaba nereye koydum?
Dedektif:Aramak için vaktim yok çıkmalıyım.""",

    "22_Dumpster_1.txt": 
"""Polis:NERDE KALDIN!? Sabahtan beri seni bekliyoruz.
Dedektif:Yarı kör bir şekilde yol bulmak kolay olmuyor ihtiyar. 
Dedektif:Anlat Umut, neler oldu?
Asistan:Kamera kayıtlarını izlediğimizde yine aynı kar maskeli adamı gördük. Son üç gündür peş peşe suç işliyor.
Polis:Arkasında kamera kayıtları dışında hiçbir iz bırakmadan hepsinden sıyrıldı.
Polis:Bi elime geçirirsem-""",

    "23_Dumpster_2.txt": 
"""Asistan:Dedektif!
Dedektif:Noldu?
Asistan:Bu gözlük sizin değil mi?
Dedektif:E-evet.
Asistan:…
Polis:…
Polis:KALDIR KOLLARINI.!"""
}

# Dosyaları oluştur
print("--- Sıralı Dosya Oluşturma Başladı ---")
for filename, content in dialogues.items():
    file_path = os.path.join(folder_name, filename)
    with open(file_path, "w", encoding="utf-8") as f:
        f.write(content.strip()) 
    print(f"Oluşturuldu: {filename}")

print(f"\nBaşarılı! Dosyalar '{folder_name}' klasöründe 01'den 23'e kadar sıralandı.")