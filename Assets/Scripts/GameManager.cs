using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    #region Game Manager Variables
    public static GameManager Instance;
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;
    //public NetworkVariable<GameState> NetworkState = new NetworkVariable<GameState>();
    public NetworkVariable<bool> GameStarted = new NetworkVariable<bool>();
    #endregion

    #region Round Manager Variables   
    private GameObject[] players;
    private GameObject[] cameraTargets;
    //public GameObject playButton;
    private bool RoundSection, LeaderboardSection, PurchasePhase;
    public float RoundTime, leaderboardTime, purchaseTime;
    public float currentRoundTime, currentLeaderboardTime, currentPurchaseTime;
    private TMP_Text timeText;
    #endregion

    //private NetworkVariable<int> exampleVariable = new NetworkVariable<int>();
    void Awake() {
        Instance = this;
    }
     void Start() {
        UpdateGameState(GameState.LanConnection);
    }

    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // Modifica las NetworkVariables y realiza otras configuraciones aquí
        GameStarted.Value = false; // Por ejemplo, establece GameStarted en false cuando se inicie el NetworkObject
        UpdateGameState(GameState.LanConnection);
    }

    public void Update()
    {
        Debug.Log(RoundSection);

        if(GameStarted.Value){
            GameManager.Instance.UpdateGameState(GameState.StartGame);
        }

        if (RoundSection)
        {
            TMP_Text timeText = FindTimerText();
            currentRoundTime -= Time.deltaTime;
            timeText.text = (Mathf.Round(currentRoundTime * 10.0f) / 10.0f).ToString();
            if(currentRoundTime <= 10.1)
            {
                timeText.text = Mathf.Round(currentRoundTime ).ToString();
                timeText.color = Color.red;
                timeText.fontSize = 70;

            }
            if (currentRoundTime <= 0.5 )
            {
                GameManager.Instance.UpdateGameState(GameState.Leaderboard);
                RoundSection = false;

            }
        }
        if (LeaderboardSection)
        {
            currentLeaderboardTime -= Time.deltaTime;
            if (currentLeaderboardTime <= 0)
            {
                GameManager.Instance.UpdateGameState(GameState.PurchasePhase);
                LeaderboardSection= false;
            }
        }

        if (PurchasePhase)
        {
            TMP_Text timeText = FindTimerText();
            //Hacer esta parte bien
            timeText.color = Color.white;
            timeText.fontSize = 50;
            //Hacer esta parte bien
            currentPurchaseTime -= Time.deltaTime;
            timeText.text = Mathf.Round(currentPurchaseTime).ToString();
            if (currentPurchaseTime <= 0)
            {
                PurchasePhase= false;
                ResetValues();
                CombatRound();
            }
        }
    }

    public void UpdateGameState(GameState newState){
        State = newState;
        switch(newState){
            case GameState.LanConnection:
                HandleLanConnection();
                break;
            case GameState.StartGame:
                HandleStartGame();
                break;
            case GameState.Round:
                HandleRound();
                break;
            case GameState.Leaderboard:
                HandleLeaderboard();
                break;
            case GameState.PurchasePhase:
                HandlePurchasePhase();
                break;
            case GameState.EditMode:
                break;
            case GameState.WinScreen:
                break;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleLanConnection(){

    }
    
    #region Round Manager Functions

    public void StartGame()
    {
    // Verificar si somos el servidor antes de modificar la variable en la red
    if (IsServer)
    {
        GameStarted.Value = true; // Esto solo se ejecutará en el servidor
        // Llamar a un RPC para informar a los clientes sobre el cambio
        InformClientsAboutGameStartServerRPC();
    }
        Debug.Log("Clickeaste aqui");
    }

    public void CombatRound()
    {
        GameManager.Instance.UpdateGameState(GameState.Round);
        Debug.Log("RoundStart");


    }

    private void HandleStartGame(){
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerController>().spawnPlayerClientRpc();
            player.GetComponent<PlayerController>().startCameraClientRpc();
        }
        currentRoundTime = RoundTime;
        currentLeaderboardTime = leaderboardTime;
        currentPurchaseTime = purchaseTime;
         CombatRound();
    }
    private void HandleRound(){
        RoundSection = true;
    }
    void HandleLeaderboard(){
        LeaderboardSection = true;
    }
    #endregion

    void HandlePurchasePhase(){
        PurchasePhase = true;
    }

    void ResetValues(){
        currentRoundTime = RoundTime;
        currentLeaderboardTime = leaderboardTime;
        currentPurchaseTime = purchaseTime;
    }

    public TMP_Text FindTimerText()
    {
        GameObject textMeshProObject = GameObject.FindWithTag("Timer");
        if (textMeshProObject != null)
        {
            TMP_Text textMeshProComponent = textMeshProObject.GetComponent<TMP_Text>();
            if (textMeshProComponent != null)
            {
                return textMeshProComponent;
            }
            else
            {
                Debug.LogError("TextMeshPro component not found on the object with the 'Timer' tag.");
                return null;
            }
        }
        else
        {
            Debug.LogError("GameObject with the 'Timer' tag not found.");
            return null;
        }
    }

    [ServerRpc]
    private void InformClientsAboutGameStartServerRPC()
    {
    // Este RPC se ejecutará en el servidor y se sincronizará con los clientes
    GameStarted.Value = true;
    }
}


public enum GameState{
    LanConnection,
    StartGame,
    Round,
    Leaderboard,
    PurchasePhase,
    EditMode,
    WinScreen
}