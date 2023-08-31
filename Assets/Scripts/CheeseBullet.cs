using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseBullet : MonoBehaviour
{
    public int cheeseDamage;
    private GameObject[] players;
    public bool isFake;
    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            Physics2D.IgnoreCollision(player.transform.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        StartCoroutine(selfDestruct());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

/*
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(collision.transform.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }*/

    private IEnumerator selfDestruct(){
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }

    
}
