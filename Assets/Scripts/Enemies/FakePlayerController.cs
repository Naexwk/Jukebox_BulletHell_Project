using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FakePlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform waypoint;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = waypoint.position; //used to be agent destination
    }
}
