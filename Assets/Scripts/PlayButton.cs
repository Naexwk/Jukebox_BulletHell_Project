using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    public GameObject gameManagerInstance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManagerInstance != null) {
            return;
        }
        Debug.Log("PlayButton currently searching...");
        gameManagerInstance = GameObject.FindWithTag("GameManager");

        if (gameManagerInstance != null) {
            Button button = gameObject.GetComponent<Button>();
            button.onClick.AddListener(gameManagerInstance.GetComponent<GameManager>().StartGame);
        }
    }
}
