using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveWaypoint : MonoBehaviour
{
    public float speed = 5f;
    private bool change = true;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update(){
        if(change){
            waypointRight();
        }
        else {
            waypointLeft();
        }
    }
    void waypointRight(){
        transform.Translate(speed*Time.deltaTime,0,0);
    }
    void waypointLeft(){
        transform.Translate(-speed*Time.deltaTime,0,0);
    }
     void OnTriggerEnter2D(Collider2D otherCollider)
    {
        Debug.Log("HI");
        change = !change;
    }
}
