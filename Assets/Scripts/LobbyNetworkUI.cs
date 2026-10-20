using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;
public class LobbyNetworkUI : MonoBehaviour
{
    [Header("Buton Referansları")]
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    
    [Header("IP Ayarı (Opsiyonel)")]
    [SerializeField] private TMP_InputField ipInputField;
    
    private void Start() 
    {
        if(hostButton != null) 
        {
            hostButton.onClick.AddListener(StartHostMode);
        }
        
        if(clientButton != null) 
        {
            clientButton.onClick.AddListener(StartClientMode);
        }
        
        if(ipInputField != null) 
        {
            ipInputField.text = "127.0.0.1"; // => localhost
        }
    }

    private void StartHostMode()
    {
        if (NetworkManager.Singleton != null) // sahnede network manager var mı?
        {
            NetworkManager.Singleton.StartHost();
            gameObject.SetActive(false); // baglaninca paneli kapat.
        }   
    }

    private void StartClientMode()
    {
        if (NetworkManager.Singleton != null)
        {
            if (ipInputField != null && !string.IsNullOrEmpty(ipInputField.text))
            {
                var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                if (transport != null)
                {
                    transport.ConnectionData.Address = ipInputField.text.Trim();
                }
            }

            NetworkManager.Singleton.StartClient();
            gameObject.SetActive(false); // baglaninca paneli kapat.
        }
    }
}
