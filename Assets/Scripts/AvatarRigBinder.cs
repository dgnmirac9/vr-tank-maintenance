using Unity.Netcode;
using UnityEngine;

public class AvatarRigBinder : NetworkBehaviour
{
    [Header("Görseller")]
    [SerializeField] private GameObject headVisualMesh;

    [Header("Ağ Kuklası Parçaları (Network Avatar)")]
    [SerializeField] private Transform networkHead;
    [SerializeField] private Transform networkLeftHand;
    [SerializeField] private Transform networkRightHand;

    // Yerel VR Donanım Referansları
    private Transform m_LocalCamera;
    private Transform m_LocalLeftController;
    private Transform m_LocalRightController;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            // Sahnedeki yerel XR Origin parçalarını otomatik bul
            if (Camera.main != null)
            {
                m_LocalCamera = Camera.main.transform;
                
                // XR Origin altındaki Controller objelerini yakala
                var xrOrigin = Camera.main.transform.GetComponentInParent<Unity.XR.CoreUtils.XROrigin>();
                if (xrOrigin != null)
                {
                    m_LocalLeftController = xrOrigin.transform.Find("Camera Offset/Left Controller");
                    m_LocalRightController = xrOrigin.transform.Find("Camera Offset/Right Controller");
                }
            }

            // Kendi kafa mesh'imizi gizle
            if (headVisualMesh != null)
                headVisualMesh.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        if (!IsOwner) return;

        // Kafa takibi
        if (m_LocalCamera != null && networkHead != null)
        {
            networkHead.position = m_LocalCamera.position;
            networkHead.rotation = m_LocalCamera.rotation;
        }

        // Sol el takibi
        if (m_LocalLeftController != null && networkLeftHand != null)
        {
            networkLeftHand.position = m_LocalLeftController.position;
            networkLeftHand.rotation = m_LocalLeftController.rotation;
        }

        // Sağ el takibi
        if (m_LocalRightController != null && networkRightHand != null)
        {
            networkRightHand.position = m_LocalRightController.position;
            networkRightHand.rotation = m_LocalRightController.rotation;
        }
    }
}   