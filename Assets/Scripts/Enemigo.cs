using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private FirstPerson player;
    Animator animator;
    int correrAnim;
    //el enemigo tiene que perseguir al player
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FirstPerson.FindObjectOfType<FirstPerson>();
        animator = GetComponent<Animator>();

        correrAnim = Random.Range(0,2);
        if (correrAnim == 0)
        {
            animator.SetBool("RunNormal", true);
        }
        else
        {
            animator.SetBool("RunHerido", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position); 
    }
}
