using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class CameraTarget : NetworkBehaviour
{
    private GameObject[] players;
    private GameObject chosenPlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartCam () {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkObject>().OwnerClientId == gameObject.GetComponent<NetworkObject>().OwnerClientId) {
                chosenPlayer = player;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (chosenPlayer != null) {
            
            gameObject.transform.position = chosenPlayer.transform.position;
        }
    }
}
