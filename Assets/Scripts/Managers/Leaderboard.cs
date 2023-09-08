using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using Unity.Netcode;
public class Leaderboard : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private GameObject[] players;
    private GameObject[] deadPlayers;
    
   
    [SerializeField] private GameObject player3;
    [SerializeField] private GameObject player4;
    [SerializeField] private TMP_Text[] playerNames;
    [SerializeField] private TMP_Text[] playerScores;


    public void distributePoints(){
        /*
        deadPlayers = GameObject.FindGameObjectsWithTag("Dead Player");
        players = GameObject.FindGameObjectsWithTag("Player");
        int allPlayers = players.Length + deadPlayers.Length;
        if (allPlayers > 2) {
            player3.SetActive(true);
        }
        if (allPlayers > 3) {
            player4.SetActive(true);
        }
        //List <int> indices = new List <int> ();
        List <int> privatePoints = new List <int> ();
        for (int i = 0; i < GameManager.localPoints.Count; i++) {
            privatePoints.Add(GameManager.localPoints[i]);
            //indices.Add(i);
        }
        var sorted = privatePoints
            .Select((x, i) => new KeyValuePair<int, int>(x, i))
            .OrderBy(x => x.Key)
            .ToList();
        List<int> B = sorted.Select(x => x.Key).ToList();
        List<int> idx = sorted.Select(x => x.Value).ToList();

        for (int i = 0; i < allPlayers; i++) {
            Debug.Log("B: " + B[i] + " idx: " + idx[i]);
        }

        gameManager.changeHandledLeaderboard();*/

        







/*        int[] indices = new int[GameManager.localPoints.Count];
        int[] privatePoints = new int[GameManager.localPoints.Count];
        privatePoints = GameManager.localPoints.ToArray();
        for (int i = 0; i < indices.Length; i++) indices[i] = i;
        Array.Sort(privatePoints, indices);
        Array.Reverse(privatePoints);
        Array.Reverse(indices);

        for (int i = 0; i < players.Length; i++) {
            playerNames[i].text = "Player " + indices[i];
            playerScores[i].text = "" + privatePoints[i];
        }*/
    }
}
