using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class CameraTarget : NetworkBehaviour
{
    private GameObject[] players;
    private GameObject chosenPlayer;

    public bool lockOnPlayer = false;

    // Buscar al jugador cuyo ID corresponda al del cameraTarget
    public void StartCam () {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            if (player.GetComponent<NetworkObject>().OwnerClientId == gameObject.GetComponent<NetworkObject>().OwnerClientId) {
                chosenPlayer = player;
            }
        }
    }

    // Seguir al jugador seleccionado
    void Update()
    {
        if (chosenPlayer != null && lockOnPlayer) {
            gameObject.transform.position = chosenPlayer.transform.position;
        } else {
            gameObject.transform.position = new Vector3(0f, 0f, 0f);
        }
    }
}
