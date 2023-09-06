using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBulletScript : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D col) {
        
        if (col.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
        }

        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerController>().GetHit();
            Destroy(this.gameObject);
        }
    }
}
