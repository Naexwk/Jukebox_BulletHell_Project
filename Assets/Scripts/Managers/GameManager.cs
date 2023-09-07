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
    //public GameState State;
    public static NetworkVariable<GameState> State = new NetworkVariable<GameState>(default, NetworkVariableReadPermission.Everyone);

    public static event Action<GameState> OnGameStateChanged;
    public NetworkVariable<bool> GameStarted = new NetworkVariable<bool>();
    public bool startHandled = false;
    #endregion

    #region Round Manager Variables   
    private GameObject[] players;
    private GameObject[] cameraTargets;

    public NetworkVariable<bool> RoundSection = new NetworkVariable<bool>();
    public NetworkVariable<bool> LeaderboardSection = new NetworkVariable<bool>();
    public NetworkVariable<bool> PurchasePhase = new NetworkVariable<bool>();

    //private bool RoundSection, LeaderboardSection, PurchasePhase;
    public float RoundTime, leaderboardTime, purchaseTime;

    // Variables de tiempo
    public NetworkVariable<float> currentRoundTime = new NetworkVariable<float>();
    public NetworkVariable<float> currentLeaderboardTime = new NetworkVariable<float>();
    public NetworkVariable<float> currentPurchaseTime = new NetworkVariable<float>();

    
    private TMP_Text timeText;
    #endregion

    void Awake() {
        Instance = this;
        //State = this.State;
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

        if(GameStarted.Value){
            if(!startHandled){
                GameManager.Instance.UpdateGameState(GameState.StartGame);
                startHandled = true;
            }
        }

        if (RoundSection.Value)
        {
            //Debug.Log("RoundSection.Value!");
            TMP_Text timeText = FindTimerText();
            if (IsOwner) {
                currentRoundTime.Value -= Time.deltaTime;
            }
            timeText.text = (Mathf.Round(currentRoundTime.Value * 10.0f) / 10.0f).ToString();
            if(currentRoundTime.Value <= 10.1)
            {
                timeText.text = Mathf.Round(currentRoundTime.Value ).ToString();
                timeText.color = Color.red;
                timeText.fontSize = 70;

            }
            if (currentRoundTime.Value <= 0.5 )
            {
                GameManager.Instance.UpdateGameState(GameState.Leaderboard);
                if (IsOwner) {
                    RoundSection.Value = false;
                }

            }
        }
        if (LeaderboardSection.Value)
        {
            //Debug.Log("Leaderboard!");
            if (IsOwner) {
                currentLeaderboardTime.Value -= Time.deltaTime;
            }
            
            if (currentLeaderboardTime.Value <= 0)
            {
                GameManager.Instance.UpdateGameState(GameState.PurchasePhase);
                if (IsOwner) {
                    LeaderboardSection.Value= false;
                }
                
            }
        }

        if (PurchasePhase.Value)
        {
            TMP_Text timeText = FindTimerText();
            //Hacer esta parte bien
            timeText.color = Color.white;
            timeText.fontSize = 50;
            //Hacer esta parte bien
            if (IsOwner) {
                currentPurchaseTime.Value -= Time.deltaTime;
            }
            timeText.text = Mathf.Round(currentPurchaseTime.Value).ToString();
            //Debug.Log(currentPurchaseTime.Value);
            if (currentPurchaseTime.Value <= 0)
            {
                if (IsOwner) {
                    PurchasePhase.Value = false;
                }
                ResetValues();
                // Llamar al RPC
                CombatRound();
            }
        }
    }

    public void UpdateGameState(GameState newState){
        if (IsOwner) {
            State.Value = newState;
        }
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
        GameStarted.Value = true;
    }

    public void CombatRound()
    {
        GameManager.Instance.UpdateGameState(GameState.Round);
    }

    private void HandleStartGame(){

        if (IsOwner) {
            players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in players)
            {
                //player.GetComponent<PlayerController>().spawnPlayerClientRpc();
                player.GetComponent<PlayerController>().startCameraClientRpc();
            }
            currentRoundTime.Value = RoundTime;
            currentLeaderboardTime.Value = leaderboardTime;
            currentPurchaseTime.Value = purchaseTime;
        }
        CombatRound();
    }
    private void HandleRound(){
        if (IsOwner) {
            RoundSection.Value = true;
        }
        
    }
    void HandleLeaderboard(){
        if (IsOwner) {
            LeaderboardSection.Value = true;
        }
    }
    #endregion

    void HandlePurchasePhase(){
        if (IsOwner) {
            PurchasePhase.Value = true;
        }
    }

    void ResetValues(){
        if (IsOwner) {
            currentRoundTime.Value = RoundTime;
            currentLeaderboardTime.Value = leaderboardTime;
            currentPurchaseTime.Value = purchaseTime;
        }
        
        
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