using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class MenuManager : NetworkBehaviour
{
    [SerializeField] private GameObject _lanScreen, _timer, _leaderboard, _purchaseScreen, _purchaseItemsUI, _purchaseTrapsUI;
    private bool PurchasePhaseItems, PurchasePhaseTraps;  
    private bool purchased = false;

    private GameObject[] players;
    public GameObject myPlayer;
    private GameObject[] cameraTargets;
    public GameObject myCameraTarget;
    public GameObject UIHelper;
    public bool loaded = false;


    // Suscribirse al cambio de estado del GameManager
    void Awake(){
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    // Encuentra al jugador al que le corresponda este MenuManager
    void Start()
    {

        UIHelper = GameObject.FindWithTag("UIHelper");

        _lanScreen = UIHelper.GetComponent<UIHelper>().LanScreen;
        _timer = UIHelper.GetComponent<UIHelper>().GameTimer;
        _leaderboard = UIHelper.GetComponent<UIHelper>().Leaderboard;
        _purchaseScreen = UIHelper.GetComponent<UIHelper>().PurchaseUI;
        _purchaseItemsUI = UIHelper.GetComponent<UIHelper>().PurchaseItems;
        _purchaseTrapsUI = UIHelper.GetComponent<UIHelper>().PurchaseTraps;

        loaded = true;

        loadButtonActions();

        //Button button = gameObject.GetComponent<Button>();
        //button.onClick.AddListener(gameManagerInstance.GetComponent<GameManager>().StartGame);

        if (IsOwner) {
            
            players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players) {
                if (player.GetComponent<NetworkObject>().OwnerClientId == GetComponent<NetworkObject>().OwnerClientId){
                    myPlayer = player;
                }
            }

            cameraTargets = GameObject.FindGameObjectsWithTag("CameraTarget");
            foreach (GameObject cameraTarget in cameraTargets) {
                if (cameraTarget.GetComponent<NetworkObject>().OwnerClientId == GetComponent<NetworkObject>().OwnerClientId){
                    myCameraTarget = cameraTarget;
                }
            }
            //StartCoroutine(searchForCameraTarget());
        }
    }

    void loadButtonActions(){
        Button button;
        button = _purchaseScreen.transform.GetChild(0).GetComponent<Button>();
        button.onClick.AddListener(OnSelectedItems);

        button = _purchaseScreen.transform.GetChild(1).GetComponent<Button>();
        button.onClick.AddListener(OnSelectedTraps);

        button = _purchaseItemsUI.transform.GetChild(0).GetComponent<Button>();
        button.onClick.AddListener(() => selectObject(1));

        button = _purchaseItemsUI.transform.GetChild(1).GetComponent<Button>();
        button.onClick.AddListener(() => selectObject(2));

        button = _purchaseItemsUI.transform.GetChild(2).GetComponent<Button>();
        button.onClick.AddListener(() => selectObject(3));
    }
/*
    IEnumerator searchForCameraTarget(){
        Debug.Log("Searching for Camera Targets...");
        yield return new WaitForSeconds(0.05f);
        cameraTargets = GameObject.FindGameObjectsWithTag("CameraTarget");
        Debug.Log(cameraTargets.Length);
        if (cameraTargets.Length == 0) {
            StartCoroutine(searchForCameraTarget());
        } else {
            foreach (GameObject cameraTarget in cameraTargets) {
                if (cameraTarget.GetComponent<NetworkObject>().OwnerClientId == GetComponent<NetworkObject>().OwnerClientId){
                    myCameraTarget = cameraTarget;
                }
            }
            if (myCameraTarget == null) {
                StartCoroutine(searchForCameraTarget());
            }
        }
    }*/

    void OnDestroy() {
        GameManager.OnGameStateChanged += GameManagerOnGameStateChanged;
    }

    // Funciones de objetos
    public void OnSelectedItems(){
        PurchasePhaseItems = true;
        GameManagerOnGameStateChanged(GameState.PurchasePhase);
    }

    // Añade el objeto al jugador segun su ID
    public void selectObject(int _objectID){
        if (IsOwner) {
            myPlayer.GetComponent<ItemManager>().addItem(_objectID);
            purchased = true;
            GameManagerOnGameStateChanged(GameState.PurchasePhase);
        }
    }

    public void OnSelectedTraps(){
        PurchasePhaseTraps = true;
        GameManagerOnGameStateChanged(GameState.PurchasePhase);
        //Codigo de trampas
    }
    private void GameManagerOnGameStateChanged(GameState state){
        if (!loaded || !IsOwner) {
            return;
        }
        Debug.Log("Called in state " + state);
        _lanScreen.SetActive(state == GameState.LanConnection);
        _timer.SetActive(state == GameState.StartGame ||state == GameState.Round || state == GameState.PurchasePhase || state == GameState.PurchasePhase);
        _leaderboard.SetActive(state == GameState.Leaderboard );

        if(state != GameState.Round && state != GameState.StartGame) {
            if (myPlayer != null) {
                myPlayer.GetComponent<PlayerController>().Despawn();
            }
            if (myCameraTarget != null) {
                myCameraTarget.GetComponent<CameraTarget>().lockOnPlayer = false;
            }
        } else {
            if (myPlayer != null) {
                myPlayer.GetComponent<PlayerController>().Respawn();
            }
            if (myCameraTarget != null) {
                myCameraTarget.GetComponent<CameraTarget>().lockOnPlayer = true;
            }
        }

        if (PurchasePhaseTraps || PurchasePhaseItems) {
            _purchaseScreen.SetActive(false);
            if(state == GameState.PurchasePhase){
                if (purchased) {
                    _purchaseItemsUI.SetActive(false);
                    _purchaseTrapsUI.SetActive(false);
                } else {
                    if(PurchasePhaseItems == true){
                        _purchaseItemsUI.SetActive(true);
                    }
                    else{
                        _purchaseTrapsUI.SetActive(true);
                    }
                }
             }
             else{
                _purchaseItemsUI.SetActive(false);
                _purchaseTrapsUI.SetActive(false);
                PurchasePhaseTraps = false;
                PurchasePhaseItems = false;
                purchased = false;
            }
        } else {
            _purchaseScreen.SetActive(state == GameState.PurchasePhase);
        }
    }


}
