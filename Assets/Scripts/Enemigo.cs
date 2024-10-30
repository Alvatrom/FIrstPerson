using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private FirstPerson player;

    //el enemigo tiene que perseguir al player
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FirstPerson.FindObjectOfType<FirstPerson>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position); 
    }
}
