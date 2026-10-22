using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;

public class PlayerSpawnManager : MonoBehaviour
{
    [Header("Doğuş Noktaları")]
    [SerializeField] private List<Transform> spawnPositions = new List<Transform>();

    private int m_CurrentSpawnIndex = 0;

    private void Start()
    {
        //Sadece host oyuncu doğuş yerini belirleme yetkisine sahip
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
        }
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        //oyuncunun bağlantısını onayla
        response.Approved = true;
        response.CreatePlayerObject = true;
        
        //Sıradaki doğuş sırasını ata
        if (spawnPositions.Count > 0)
        {
            Transform targetSpawn = spawnPositions[m_CurrentSpawnIndex % spawnPositions.Count];
            response.Position = targetSpawn.position;
            response.Rotation = targetSpawn.rotation;
            
            m_CurrentSpawnIndex++;
        }
        else
        {
            response.Position = Vector3.zero;
            response.Rotation = Quaternion.identity;
        }

        response.Pending = false;
    }
}
