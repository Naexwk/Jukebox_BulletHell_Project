using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerBullet : MonoBehaviour
{
    public int bulletDamage = 0;
    public float bulletSpeed = 30f;
    void Start ()
    {
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {

            Destroy(this.gameObject);
        }
    }
}
