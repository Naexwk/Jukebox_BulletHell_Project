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
    private GameObject tempObject;
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
            Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            Clicked = true; 
            tempObject = Instantiate(editor.ItemPrefabs[ID], new Vector3(worldPosition.x, worldPosition.y,0), Quaternion.identity);
            tempObject.GetComponent<Renderer>().material.color = Color.red;
            quantity--;
            quantityText.text = quantity.ToString(); 
            editor.CurrentButtonPressed = ID;
        }
        
    }
    void Update(){
        if(tempObject != null){
            Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            
            tempObject.transform.position = worldPosition; 
        }
    }
}
