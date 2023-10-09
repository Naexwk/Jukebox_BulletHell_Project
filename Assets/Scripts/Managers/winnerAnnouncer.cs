using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class winnerAnnouncer : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private TMP_Text winnerText;

    void OnEnable()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        winnerText.text = "Player " + (gameManager.networkLeaderboard[0]+1) + " wins!";
    }

}
