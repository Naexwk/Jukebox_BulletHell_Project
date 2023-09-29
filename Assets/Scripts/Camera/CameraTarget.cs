using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
public class CameraTarget : NetworkBehaviour
{
    private GameObject[] players;
    private GameObject chosenPlayer;

    public bool lockOnPlayer = false;
    Vector3 worldMousePos;
    Vector3 pos;
    float posX;
    float posY;
    private Camera _mainCamera;
    bool lastLockOnValue = false;
    float helper;

    // Buscar al jugador cuyo ID corresponda al del cameraTarget
    public void StartCam () {
        players = GameObject.FindGameObjectsWithTag("Player");
        _mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
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
        if (lockOnPlayer != lastLockOnValue) {
            if (lockOnPlayer) {
                gameObject.transform.position = chosenPlayer.transform.position;
                //Debug.Log("triggered");
            }

            lastLockOnValue = lockOnPlayer;
        }
        if (chosenPlayer != null && lockOnPlayer) {
            worldMousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            //worldMousePos = Input.mousePosition;
            posX = (chosenPlayer.transform.position.x + worldMousePos.x)/2;
            posY = (chosenPlayer.transform.position.y + worldMousePos.y)/2;


            helper = Math.Abs((chosenPlayer.transform.position.x) - (posX));
            //Debug.Log("X: " + helper);
            if (helper > 6f) {
                if (posX > chosenPlayer.transform.position.x) {
                    posX = chosenPlayer.transform.position.x + 6f;
                } else {
                    posX = chosenPlayer.transform.position.x - 6f;
                }
            }
            //Debug.Log("PosX: " + posX);
            
            helper = Math.Abs((chosenPlayer.transform.position.y) - (posY));
            //Debug.Log("Y: " + helper);
            if (helper > 1.5f) {
                if (posY > chosenPlayer.transform.position.y) {
                    posY = chosenPlayer.transform.position.y + 1.5f;
                } else {
                    posY = chosenPlayer.transform.position.y - 1.5f;
                }
            }
            //Debug.Log("X: " + posX + " Y: " + posY);

            pos = new Vector3(posX,posY,0f);
            //Debug.Log("X: " + pos.x + " Y: " + pos.y);
            gameObject.transform.position = pos;
        } else {
            gameObject.transform.position = new Vector3(0f, 0f, 0f);
        }
    }

}
