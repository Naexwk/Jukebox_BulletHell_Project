using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseBullet : MonoBehaviour
{
    public int bulletDamage = 3;
    private GameObject[] players;

    // Variable que determina si la bala es server-side o client-side
    public bool isFake;

    // Ignorar colisiones con jugadores
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            Physics2D.IgnoreCollision(player.transform.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        StartCoroutine(selfDestruct());
    }

    // Destruir el proyectil después de 5 segundos
    private IEnumerator selfDestruct(){
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }

    // DEV: Debería atravesar enemigos
    
}
