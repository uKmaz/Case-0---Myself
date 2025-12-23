using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SoundClip
{
    public string key;             // Sesi çağırmak için isim (Örn: "Kapi", "Switch", "Metro_Ambience")
    public AudioClip clip;         // Ses dosyası
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;
    public bool loop = false;      // Döngü olsun mu? (Sadece SFX için, Müzik zaten loop)
}

[System.Serializable]
public struct StepMusicLink
{
    public string linkName;       // Sadece inspector'da karışmasın diye not
    public StoryStep storyStep;   // Hangi hikaye adımında çalsın?
    public string musicKey;       // Hangi müzik çalsın? (SoundClip key'i ile aynı olmalı)
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("--- SES KAYNAKLARI ---")]
    [SerializeField] private AudioSource musicSource; // Sadece Müzik için (Loop)
    [SerializeField] private AudioSource sfxSource;   // Tek seferlik efektler için

    [Header("--- MÜZİK KÜTÜPHANESİ ---")]
    public List<SoundClip> musicTracks; // Müzikleri buraya ekle

    [Header("--- SFX KÜTÜPHANESİ ---")]
    public List<SoundClip> sfxClips;    // Efektleri buraya ekle

    [Header("--- OTOMATİK MÜZİK DEĞİŞİMİ ---")]
    [Tooltip("Hangi StoryStep'te hangi müziğin çalacağını buradan eşleştir.")]
    public List<StepMusicLink> autoMusicChanges;

    [Header("--- AYARLAR ---")]
    public float musicFadeDuration = 1.5f; // Müzik geçiş süresi

    private void Awake()
    {
        // Singleton Yapısı
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sahne değişse de müzik kesilmesin
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // GameManager'a abone ol: Hikaye adımı değişince müziği kontrol et
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStoryStepChanged += CheckAutoMusic;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStoryStepChanged -= CheckAutoMusic;
        }
    }

    // --- SFX ÇALMA SİSTEMİ ---
    
    /// <summary>
    /// İsmi verilen sesi SFX kanalından çalar.
    /// </summary>
    public void PlaySFX(string key)
    {
        SoundClip s = sfxClips.Find(x => x.key == key);
        if (s != null && s.clip != null)
        {
            // Robotikliği kırmak için pitch'i çok az rastgele değiştir
            sfxSource.pitch = s.pitch + Random.Range(-0.05f, 0.05f);
            sfxSource.PlayOneShot(s.clip, s.volume);
        }
        else
        {
            Debug.LogWarning("Ses bulunamadı: " + key);
        }
    }

    // --- MÜZİK SİSTEMİ ---

    /// <summary>
    /// İsmi verilen müziğe yumuşak geçiş yapar.
    /// </summary>
    public void PlayMusic(string key)
    {
        SoundClip s = musicTracks.Find(x => x.key == key);
        if (s == null) return;

        // Eğer zaten bu müzik çalıyorsa baştan başlatma
        if (musicSource.clip == s.clip && musicSource.isPlaying) return;

        StartCoroutine(CrossFadeMusic(s));
    }

    /// <summary>
    /// Hikaye adımı değiştiğinde otomatik çalışır.
    /// </summary>
    private void CheckAutoMusic(StoryStep newStep)
    {
        foreach (var link in autoMusicChanges)
        {
            if (link.storyStep == newStep)
            {
                Debug.Log($"Otomatik Müzik Değişimi: {newStep} -> {link.musicKey}");
                PlayMusic(link.musicKey);
                return;
            }
        }
    }

    // Yumuşak Geçiş Coroutine'i
    private IEnumerator CrossFadeMusic(SoundClip newSound)
    {
        float startVolume = musicSource.volume;

        // 1. Mevcut müziği kıs (Fade Out)
        if (musicSource.isPlaying)
        {
            for (float t = 0; t < musicFadeDuration; t += Time.deltaTime)
            {
                musicSource.volume = Mathf.Lerp(startVolume, 0f, t / musicFadeDuration);
                yield return null;
            }
        }

        // 2. Yeni klibi tak ve ayarla
        musicSource.Stop();
        musicSource.clip = newSound.clip;
        musicSource.pitch = newSound.pitch; // Müzikte random pitch yapmıyoruz
        musicSource.loop = true;
        musicSource.Play();

        // 3. Yeni müziği aç (Fade In)
        for (float t = 0; t < musicFadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0f, newSound.volume, t / musicFadeDuration);
            yield return null;
        }
        
        musicSource.volume = newSound.volume; // Son emin olma
    }
    
    // Müziği tamamen susturmak için
    public void StopMusic()
    {
        StartCoroutine(FadeOutAndStop());
    }

    private IEnumerator FadeOutAndStop()
    {
        float startVol = musicSource.volume;
        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVol, 0f, t);
            yield return null;
        }
        musicSource.Stop();
    }
    public void PlayClip(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;

        // Hafif pitch değişimi (Robotikliği kırmak için)
        sfxSource.pitch = 1f + Random.Range(-0.05f, 0.05f);
        
        // Sesi patlat
        sfxSource.PlayOneShot(clip, volume);
    }
}