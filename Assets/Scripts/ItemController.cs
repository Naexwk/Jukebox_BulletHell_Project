using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class ItemController : MonoBehaviour
{
    public int ID;
    public int quantity;
    public bool Clicked  = false; 
    public TextMeshProUGUI quantityText;
    private LevelEditorManager editor; 
    // Start is called before the first frame update
    void Start()
    {
        quantityText.text = quantity.ToString();
        editor = GameObject.FindGameObjectWithTag("LevelEditorManager").GetComponent<LevelEditorManager>();
    }

    // Update is called once per frame
    public void ButtonClicked()
    {
        if(quantity > 0 ){
            Clicked = true; 
            quantity--;
            quantityText.text = quantity.ToString(); 
            editor.CurrentButtonPressed = ID;
        }
        
    }
}
