using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo : MonoBehaviour
{
    [Header ("Sistema de ataque")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float danhoEnemigo,danhoRecibido;
    [SerializeField] private float vida;
    [SerializeField] private float radioAtaque;
    [SerializeField] private LayerMask queEsDanhable;


    private NavMeshAgent agent;
    Animator animator;


    [SerializeField] private FirstPerson player;
    private bool ventanaAbierta;
    private bool puedoDanhar =true;

    Rigidbody[] huesos;
    int correrAnim;

    public float Vida { get => vida; set => vida = value; }

    //el enemigo tiene que perseguir al player
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = FirstPerson.FindObjectOfType<FirstPerson>();
        huesos = GetComponentsInChildren<Rigidbody>();

        for (int i = 0;i < huesos.Length; i++)
        {
            huesos[i].isKinematic = true;
        }



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
        if (agent.enabled)
        {
           Perseguir();

        }

        if (ventanaAbierta && puedoDanhar)
        {
          DetectarImpacto();
        }
    }
    private void DetectarImpacto()
    {
        //1� referenciar el attackPoint
        //2� crear una variable que represente el radio de ataque
        //3 crear una variable que represente que es da�able,(layer)
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

        if (!agent.pathPending == false && agent.remainingDistance <= agent.stoppingDistance)//si no tiene calcuulos pendientes y se cumple la distancia ...
        {
            //lanzar la animacion de ataque
            agent.isStopped = true;//me paro
            animator.SetBool("attacking", true);//lanzo el ataque
        }
    }

    public void RecibirDanho(float danhoRecibido)
    {
        vida -= danhoRecibido;
        if (vida < 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void FinAtaque()
    {
        agent.isStopped = false;//me paro
        animator.SetBool("attacking", false);//lanzo el ataque
        puedoDanhar = true; 
    }

    public void CambiarEstadoHuesos(bool estado)
    {
        for(int i = 0;i < huesos.Length; i++)
        {
            huesos[i].isKinematic = estado;
        }
    }
    private void AbrirVentanaAtaque()
    {
        ventanaAbierta = true;
        
    }
    private void CerrarVentanaAtaque()
    {
        ventanaAbierta = false;

    }
    public void Morir()
    {
        CambiarEstadoHuesos(false);
        animator.enabled = false;
        agent.enabled = false;
    }
}
