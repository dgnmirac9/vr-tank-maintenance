using Unity.Netcode;
using UnityEngine;

public class AvatarRigBinder : NetworkBehaviour
{
    [Header("Gizlenecek Kafa Modeli")]
    [SerializeField] private GameObject headVisualMesh; // Mor kafa / gözlük objesi

    private Transform m_MainCameraTransform;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            // 1. Sahnedeki kask kamerasını bul
            if (Camera.main != null)
                m_MainCameraTransform = Camera.main.transform;

            // 2. Kendi ekranımızda mor kafayı gizle (görüşümüzü kapatmasın)
            if (headVisualMesh != null)
                headVisualMesh.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        // Sadece bu kaskın sahibi bizsek kafa pozisyonunu kaskla birebir eşitle
        if (IsOwner && m_MainCameraTransform != null)
        {
            transform.position = m_MainCameraTransform.position;
            transform.rotation = m_MainCameraTransform.rotation;
        }
    }
}