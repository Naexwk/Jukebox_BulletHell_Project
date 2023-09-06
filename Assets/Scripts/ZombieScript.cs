using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieScript : MonoBehaviour
{
    public float updateTimer = 1f;
    private Transform target;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        //InvokeRepeating("FindPlayer", 0f, updateTimer);
    }

    // Update is called once per frame
    void Update()
    {
        FindPlayer();
        if (target != null){
            agent.SetDestination(target.position);
        }
    }

    void FindPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
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
