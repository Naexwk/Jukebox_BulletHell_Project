using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RoundManager : MonoBehaviour
{

    
    private GameObject[] players;
    private GameObject[] cameraTargets;

    public GameObject LanScreen;
    public GameObject PlayButton;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void hideLanScreen(){
        LanScreen.SetActive(false);
    }

    public void activatePlayButton(bool _state){
        PlayButton.SetActive(_state);
    }


    public void StartRound(){
        //int i = 0;
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerController>().spawnPlayerClientRpc();
            player.GetComponent<PlayerController>().startCameraClientRpc();
        }


        hideLanScreen();
    }
}
