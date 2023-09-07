using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieScript : MonoBehaviour
{
    public float updateTimer = 1f;
    private Transform target;
    private NavMeshAgent agent;

    public int health;

    // Valores iniciales
    void Start()
    {
        health = 5;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Moverse a su target
    void Update()
    {
        FindPlayer();
        if (target != null){
            agent.SetDestination(target.position);
        }
    }

    // Al entrar en contacto con una bala, recibir daño
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "PlayerBullet")
        {
            health -= col.gameObject.GetComponent<PlayerBullet>().bulletDamage;
            if (health <= 0) {
                Destroy(this.gameObject);
            }
        }
    }

    // Al entrar en contacto con un jugador, dañarlo
    // Al entrar en contacto con una bala, recibir daño
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerController>().GetHit();
        }

        // Funciona exclusivamente con la bala de queso porque es la única con Collider, no trigger
        if (col.gameObject.tag == "PlayerBullet")
        {
            health -= col.gameObject.GetComponent<CheeseBullet>().bulletDamage;
            if (health <= 0) {
                Destroy(this.gameObject);
            }
        }
    }
    
    // Buscar al jugador más cercano
    void FindPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        // Si no hay jugadores, no moverse
        if (players.Length == 0) {
            target = transform;
        } else {
            float closestDistance = Mathf.Infinity;
            foreach(GameObject p in players){
                float distance = Vector2.Distance(transform.position, p.transform.position);
                if (distance < closestDistance){
                    closestDistance = distance;
                    target = p.transform;
                }
            }
        }
        
    }
}
