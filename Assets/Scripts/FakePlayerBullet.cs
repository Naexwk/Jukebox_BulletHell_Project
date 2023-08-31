using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayerBullet : MonoBehaviour
{
    public float bulletSpeed = 30f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {

            Destroy(this.gameObject);
        }
    }

    
}
