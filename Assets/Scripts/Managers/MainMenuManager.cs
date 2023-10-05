using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject joinGamePanel;
    bool joinGamePanelState = false;

    public void changeStateJoinGamePanel(){
        joinGamePanelState = !joinGamePanelState;
        joinGamePanel.SetActive(joinGamePanelState);
    }
}
