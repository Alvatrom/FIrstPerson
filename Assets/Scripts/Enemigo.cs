using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour
{
    [Header ("Sistema de ataque")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float danhoEnemigo;
    [SerializeField] private float radioAtaque;
    [SerializeField] private LayerMask queEsDanhable;


    private NavMeshAgent agent;
    Animator animator;


    [SerializeField] private FirstPerson player;
    private bool ventanaAbierta;
    private bool puedoDanhar =true;
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
        Perseguir();
        if (ventanaAbierta && puedoDanhar)
        {
          DetectarImpacto();
        }
    }
    private void DetectarImpacto()
    {
        //1º referenciar el attackPoint
        //2º crear una variable que represente el radio de ataque
        //3 crear una variable que represente que es dañable,(layer)
        Collider[] collDetectados = Physics.OverlapSphere(attackPoint.position,radioAtaque, queEsDanhable);
        
        //si hemos detectado algo dentro de nuestro radar
        if (collDetectados.Length > 0)
        {
            for (int i = 0; i < collDetectados.Length; i++)
            {
                collDetectados[i].GetComponent<FirstPerson>().RecibirDanho(danhoEnemigo);

            }
            puedoDanhar = false;
        }
    }

    private void Perseguir()
    {
        agent.SetDestination(player.transform.position);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            //lanzar la animacion de ataque
            agent.isStopped = true;//me paro
            animator.SetBool("attacking", true);//lanzo el ataque
        }
    }

  
    private void FinAtaque()
    {
        agent.isStopped = false;//me paro
        animator.SetBool("attacking", false);//lanzo el ataque
        puedoDanhar = true; 
    }
    private void AbrirVentanaAtaque()
    {
        ventanaAbierta = true;
        
    }
    private void CerrarVentanaAtaque()
    {
        ventanaAbierta = false;
        Debug.Break();

    }
}
