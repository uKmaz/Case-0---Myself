using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    public static InteractionPromptUI Instance;

    [Header("Görsel Ayarlar")]
    public GameObject uiPanel;      // F Tuşu Görseli (Canvas içindeki Image veya World Space Sprite)
    public float heightOffset = 1.5f; // Objenin ne kadar tepesinde dursun?
    
    [Header("Animasyon")]
    public float hoverSpeed = 4f;
    public float hoverRange = 0.1f;

    private Transform currentTarget; // Şu an hangi objenin tepesindeyiz?
    private bool isShown = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (uiPanel != null) uiPanel.SetActive(false);
    }

    private void LateUpdate()
    {
        // Eğer gösterilecek bir hedef varsa, onu takip et
        if (isShown && currentTarget != null && uiPanel != null)
        {
            // Hedefin pozisyonunu al
            Vector3 targetPos = currentTarget.position;

            // Yukarı aşağı süzülme efekti (Animasyon)
            float hoverY = Mathf.Sin(Time.time * hoverSpeed) * hoverRange;

            // UI'ı hedefin tepesine yerleştir
            uiPanel.transform.position = targetPos + new Vector3(0, heightOffset + hoverY, 0);
        }
    }

    // Bir objenin yanına gelince çağırılır
    public void Show(Transform target)
    {
        currentTarget = target;
        isShown = true;
        if (uiPanel != null) uiPanel.SetActive(true);
    }

    // Uzaklaşınca çağırılır
    public void Hide()
    {
        currentTarget = null;
        isShown = false;
        if (uiPanel != null) uiPanel.SetActive(false);
    }
}