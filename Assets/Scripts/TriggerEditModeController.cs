using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TriggerEditModeController : MonoBehaviour
{
    private bool isOverlapping = false; // To keep track of overlapping state
    private Renderer tempRend;
    private Color currentColor; 

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("HELLO I AM HERE");
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
    void OnTriggerEnter2D(Collider2D other)
    {
        // Set overlapping state to true and log the event
        
            Debug.Log("Triggered by: " + other.gameObject.name);
            float newRed = 255f;
            tempRend.material.color = new Color(newRed, currentColor.g, currentColor.b, currentColor.a);
        
        
    }

    // Called when another object exits the trigger collider attached to this object
    void OnTriggerExit2D(Collider2D other)
    {
        
            Debug.Log("No longer triggered by: " + other.gameObject.name);
            tempRend.material.color = new Color(currentColor.r, currentColor.g, currentColor.b, currentColor.a);
        
        // Set overlapping state to false and log the event
    }
}

