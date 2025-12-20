using UnityEngine;

public class MainCameraActions : MonoBehaviour
{

    /// <summary>
    /// Kamerayı belirtilen noktaya anında ışınlar.
    /// </summary>
    /// <param name="targetPosition">Gidilecek nokta (SpawnPoint)</param>
    public void TeleportCamera(Transform targetPosition)
    {
        if (targetPosition == null) return;

        // 1. Önce kameranın kendi pozisyonunu değiştir (Z eksenini koruyarak)
        Vector3 newPos = targetPosition.position;
        newPos.z = transform.position.z; // Kameranın Z'si bozulmasın (-10)
        transform.position = newPos;
        
    }
}