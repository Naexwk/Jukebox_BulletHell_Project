using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TriggerEditModeController : MonoBehaviour
{
    private Renderer tempRend;
    private Color currentColor;
    public bool placeable = true;  

    // Start is called before the first frame update
    void Start()
    {
        tempRend = GetComponent<Renderer>();
        currentColor = tempRend.material.color; 
        // Access the Collider2D component of the GameObject this script is attached to
        Collider2D collider = GetComponent<Collider2D>();
        // Set the Collider2D to be a trigger
        if (collider != null) // Make sure the Collider2D component exists
        {
            collider.isTrigger = true;
        }
    }

    // Called when another object enters the trigger collider attached to this object
    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("THE TYPE IS" + other.GetType() + "Triggered by: " + other.gameObject.name);
        if (other is BoxCollider2D )
        {
            placeable = false; 
            //Debug.Log("Triggered by: " + other.gameObject.name);
            float newRed = 255f;
            tempRend.material.color = new Color(newRed, currentColor.g, currentColor.b, currentColor.a);
        } else if(other is CircleCollider2D){
            placeable = false; 
            float newRed = 255f;
            tempRend.material.color = new Color(currentColor.r, currentColor.g, newRed, currentColor.a);
        }
    }

    // Called when another object exits the trigger collider attached to this object
    void OnTriggerExit2D(Collider2D other)
    {
        placeable = true; 
        //Debug.Log("No longer triggered by: " + other.gameObject.name);
        tempRend.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, currentColor.a);
    }
}

