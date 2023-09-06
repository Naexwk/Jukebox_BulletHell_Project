using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _lanScreen, _timer, _leaderboard, _purchaseScreen, _purchaseItemsUI, _purchaseTrapsUI;
    private bool PurchasePhaseItems, PurchasePhaseTraps;

    void Awake(){
        GameManager.OnGameStateChanged += GameManagerOnOnGameStateChanged;
    }

    private void Update() {
    }

    void OnDestroy() {
        GameManager.OnGameStateChanged += GameManagerOnOnGameStateChanged;
    }

    
    public void OnSelectedItems(){
        PurchasePhaseItems = true;
        GameManagerOnOnGameStateChanged(GameState.PurchasePhase);
        //Codigo de items
  
    }

    public void OnSelectedTraps(){
        PurchasePhaseTraps = true;
        GameManagerOnOnGameStateChanged(GameState.PurchasePhase);
        //Codigo de trampas
    }
    private void GameManagerOnOnGameStateChanged(GameState state){
        Debug.Log(state);
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
