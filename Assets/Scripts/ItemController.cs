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
    // Start is called before the first frame update
    void Start()
    {
        quantityText.text = quantity.ToString();
    }

    // Update is called once per frame
    public void ButtonClicked()
    {
        Clicked = true; 
        if(quantity > 0 ){
            quantity--;
            quantityText.text = quantity.ToString(); 
        }
        
    }
}
