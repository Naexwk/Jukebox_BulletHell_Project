using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _lanScreen, _timer, _leaderboard, _purchaseScreen, _purchaseItemsUI, _purchaseTrapsUI;
    private bool PurchasePhaseItems, PurchasePhaseTraps;

    //public GameManager gameManagerInstance;

    void Awake(){
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    private void Update() {
        /*if (gameManagerInstance != null) {
            return;
        }
        Debug.Log("MenuManager currently searching...");
        gameManagerInstance = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        if (gameManagerInstance != null) {
            gameManagerInstance.OnGameStateChanged += GameManagerOnGameStateChanged;
        }*/
    }

    void OnDestroy() {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    
    public void OnSelectedItems(){
        PurchasePhaseItems = true;
        GameManagerOnGameStateChanged(GameState.PurchasePhase);
        //Codigo de items
  
    }

    public void OnSelectedTraps(){
        PurchasePhaseTraps = true;
        GameManagerOnGameStateChanged(GameState.PurchasePhase);
        //Codigo de trampas
    }
    private void GameManagerOnGameStateChanged(GameState state){
        Debug.Log("called with state " + state);
        _lanScreen.SetActive(state == GameState.LanConnection);
        _timer.SetActive(state == GameState.StartGame ||state == GameState.Round || state == GameState.PurchasePhase || state == GameState.PurchasePhase);
        _leaderboard.SetActive(state == GameState.Leaderboard );
        if (PurchasePhaseTraps || PurchasePhaseItems) {
            _purchaseScreen.SetActive(false);
            if(state == GameState.PurchasePhase){
                if(PurchasePhaseItems == true){
                    
                    _purchaseItemsUI.SetActive(true);
                }
                else{
                    _purchaseTrapsUI.SetActive(true);
                }
             }
             else{
                _purchaseItemsUI.SetActive(false);
                _purchaseTrapsUI.SetActive(false);
                PurchasePhaseTraps = false;
                PurchasePhaseItems = false;
            }
        } else {
            _purchaseScreen.SetActive(state == GameState.PurchasePhase);
        }
    }


}
