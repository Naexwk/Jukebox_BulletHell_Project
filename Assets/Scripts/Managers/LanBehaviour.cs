using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;
using System.Net;
using System.Net.Sockets;

public class LanBehaviour : NetworkBehaviour
{
	private PlayerController pc;
	private bool pcAssigned;

	[SerializeField] TextMeshProUGUI ipAddressText;
	[SerializeField] TMP_InputField ip;

	[SerializeField] string ipAddress;
	[SerializeField] UnityTransport transport;

	public GameObject gameManagerObj;
	public GameObject playButton;

	void Start()
	{
		ipAddress = "0.0.0.0";
		SetIpAddress();
	}

	// Hostea un juego y añade un GameManager
	public void StartHost() {
		NetworkManager.Singleton.StartHost();
		GetLocalIPAddress(); 
		ipAddressText.text = "Hosted at " + ipAddress.ToString();
		InstantiateGameManager();

	}

	// Inicia un cliente
	public void StartClient() {
		// Obtiene la IP local y se une al juego con esa IP
		// Esto debe cambiar a conseguir la IP del campo de texto
		ipAddress = GetLocalIPAddress(); 
		SetIpAddress();
		NetworkManager.Singleton.StartClient();

	}

	/* Gets the Ip Address of your connected network and
	shows on the screen in order to let other players join
	by inputing that Ip in the input field */
	// ONLY FOR HOST SIDE 
	public string GetLocalIPAddress() {
		var host = Dns.GetHostEntry(Dns.GetHostName());
		foreach (var ip in host.AddressList) {
			if (ip.AddressFamily == AddressFamily.InterNetwork) {
				ipAddress = ip.ToString();
				return ip.ToString();
			}
		}
		throw new System.Exception("No network adapters with an IPv4 address in the system!");
	}

	/* Sets the Ip Address of the Connection Data in Unity Transport
	to the Ip Address which was input in the Input Field */
	// ONLY FOR CLIENT SIDE
	public void SetIpAddress() {
		transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
		transport.ConnectionData.Address = ipAddress;
	}

	// Instancia un GameManager. Server-only
	public void InstantiateGameManager(){
		GameObject gameManagerPrefab;
		gameManagerPrefab = Instantiate(gameManagerObj, transform.position,Quaternion.identity);
		gameManagerPrefab.GetComponent<NetworkObject>().Spawn();
	}

	public void activatePlayButton(bool _state)
    {
        playButton.SetActive(_state);
    }

}