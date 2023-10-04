using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport.Relay;

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
	string inputJoinCode;

	async void Start()
	{
		//ipAddress = "0.0.0.0";
		//SetIpAddress();
		await UnityServices.InitializeAsync();
		AuthenticationService.Instance.SignedIn += () => {
			Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
		};
		await AuthenticationService.Instance.SignInAnonymouslyAsync();
	}

	// Hostea un juego y añade un GameManager
	public void StartHost() {
		//Debug.Log(HostGame(3));
		CreateRelay();
		
		//GetLocalIPAddress(); 
		

	}

	// Inicia un cliente
	public void StartClient() {
		// Obtiene la IP local y se une al juego con esa IP
		// Esto debe cambiar a conseguir la IP del campo de texto
		//ipAddress = GetLocalIPAddress(); 
		//ipAddress = ip.text;
		//SetIpAddress();
		inputJoinCode = ip.text;
		JoinRelay(inputJoinCode);
		
		
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

	public async void CreateRelay(){
		try {
			Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
			string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
			Debug.Log(joinCode);

			RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
			NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
			NetworkManager.Singleton.StartHost();
			ipAddressText.text = "Join Code: " + joinCode;
			InstantiateGameManager();
		} catch (RelayServiceException e){
			Debug.Log(e);
		}
		
	}

	private async void JoinRelay(string joinCode){
		try {
			Debug.Log("Joining Relay with " + joinCode);
			JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
			RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
			NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
			NetworkManager.Singleton.StartClient();
		} catch (RelayServiceException e){
			Debug.Log(e);
		}
	}



/*
public struct RelayHostData
{
    public string JoinCode;
    public string IPv4Address;
    public ushort Port;
    public Guid AllocationID;
    public byte[] AllocationIDBytes;
    public byte[] ConnectionData;
    public byte[] Key;
}

public static async Task<RelayHostData> HostGame(int maxConn)
{
    //Initialize the Unity Services engine
    await UnityServices.InitializeAsync();
    //Always autheticate your users beforehand
    if (!AuthenticationService.Instance.IsSignedIn)
    {
        //If not already logged, log the user in
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    //Ask Unity Services to allocate a Relay server
    Allocation allocation = await Unity.Services.Relay.RelayService.Instance.CreateAllocationAsync(maxConn);

    //Populate the hosting data
    RelayHostData data = new RelayHostData
    {
        // WARNING allocation.RelayServer is deprecated
        IPv4Address = allocation.RelayServer.IpV4,
        Port = (ushort) allocation.RelayServer.Port,

        AllocationID = allocation.AllocationId,
        AllocationIDBytes = allocation.AllocationIdBytes,
        ConnectionData = allocation.ConnectionData,
        Key = allocation.Key,
    };

    //Retrieve the Relay join code for our clients to join our party
    data.JoinCode = await Unity.Services.Relay.RelayService.Instance.GetJoinCodeAsync(data.AllocationID);

    return data;
}*/

}