//This script is placed on a  button, the button contains the quantityof an item that can be placed
// each time the button is pressed a 'fake' item is created and then destroyed when it is placed again
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 
using static TriggerEditModeController;

public class ItemController : MonoBehaviour
{
    public int ID;
    public int quantity;
    public bool Clicked  = false; 
    public TextMeshProUGUI quantityText;
    private LevelEditorManager editor; 
    public GameObject tempObject;
    private Renderer tempRend;
    // Start is called before the first frame update
    void Start()
    {
        quantityText.text = quantity.ToString();
        editor = GameObject.FindGameObjectWithTag("LevelEditorManager").GetComponent<LevelEditorManager>();
    }

    // Update is called once per frame
    public void ButtonClicked() //if the button is clicked
    {
        if(quantity > 0 ){
            //Get Spawn Position
            Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

            Clicked = true; 
            //Instantiates Temp Object (ghost)
            tempObject = Instantiate(editor.ItemPrefabs[ID], new Vector3(worldPosition.x, worldPosition.y,0), Quaternion.identity);
            //add Trigger script
            Collider collider = tempObject.GetComponent<Collider>();
            if(collider != null){
                collider.isTrigger = true; 
            }
            TriggerEditModeController triggerEditModeController = tempObject.AddComponent<TriggerEditModeController>();
            //Make the Object Transparent
            tempRend = tempObject.GetComponent<Renderer>();
            Color currentColor = tempRend.material.color; 
            float newAlpha = 0.5f;
            tempRend.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);

            //Change the button
            quantity--;
            quantityText.text = quantity.ToString(); 
            editor.CurrentButtonPressed = ID;
        }
        
    }
    void Update(){ //follows the mouse and destroys the object when ready
        if(tempObject != null){
            Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            
            tempObject.transform.position = worldPosition; 
            if(Input.GetMouseButtonDown(0) && tempObject.GetComponent<TriggerEditModeController>().placeable){
                Destroy(tempObject);
            }
        }
    }
}