using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyNetworkUI : MonoBehaviour
{
    [Header("UI Buton Referansları")]
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    [Header("Ağ Ayarları")]
    [SerializeField] private TMP_InputField ipInputField;

    private const ushort NetworkPort = 7777;
    private const string DefaultLocalIP = "127.0.0.1";

    private void Awake()
    {
        // Buton tıklamalarını kod seviyesinde garantiye al
        if (hostButton != null)
            hostButton.onClick.AddListener(OnClickStartHost);

        if (clientButton != null)
            clientButton.onClick.AddListener(OnClickStartClient);
    }

    private void Start()
    {
        if (ipInputField != null && string.IsNullOrEmpty(ipInputField.text))
        {
            ipInputField.text = DefaultLocalIP;
        }
    }

    public void OnClickStartHost()
    {
        Debug.Log("[LobbyNetworkUI] Host başlatılıyor...");
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        if (transport != null)
        {
            transport.SetConnectionData(DefaultLocalIP, NetworkPort, "0.0.0.0");
        }

        NetworkManager.Singleton.StartHost();
        HideUI();
    }

    public void OnClickStartClient()
    {
        Debug.Log("[LobbyNetworkUI] Client bağlanıyor...");
        var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        if (transport != null)
        {
            string targetIP = string.IsNullOrWhiteSpace(ipInputField.text) ? DefaultLocalIP : ipInputField.text.Trim();
            transport.SetConnectionData(targetIP, NetworkPort);
        }

        NetworkManager.Singleton.StartClient();
        HideUI();
    }

    private void HideUI()
    {
        gameObject.SetActive(false);
    }
    
}