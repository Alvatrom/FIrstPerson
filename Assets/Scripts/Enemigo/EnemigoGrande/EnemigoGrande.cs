using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemigoGrande : CreatorEnemy
{
    private NavMeshAgent agente;
    public Animator animaciones;
    public Transform[] CheckPoints;
    private int indice;
    public float distanciaCheckPoints;
    private float distanciaCheckPoints2;
    public float daño = 3;
    private bool isIdle;

    public override void Awake()//para poder modificar el del padre,llama lo del padre pero permite modificarlo para este caso 
    {
        base.Awake();
        agente = GetComponent<NavMeshAgent>();//es un metodo costoso pero solo se hace una vez en el awake
        distanciaCheckPoints2 = distanciaCheckPoints * distanciaCheckPoints;
    }

    public override void EstadoPatrulla()
    {
        base.EstadoPatrulla();

        if (isIdle) return;//si esta en idle no continua

        if (animaciones != null) animaciones.SetFloat("velocidad", 1);//para que no pueda haber errores por falta de animacion
        if (animaciones != null) animaciones.SetBool("atacando", false);
        agente.SetDestination(CheckPoints[indice].position);
        if ((CheckPoints[indice].position - transform.position).sqrMagnitude < distanciaCheckPoints2)//preguntar esto
        {
            StartCoroutine(IdleAtCheckpoint());
            /*indice = (indice + 1)% CheckPoints.Length;// al llegar a 3( el punto lenght) vuelve a valer 0 porque hace el modulo de 3
            Debug.Log(indice);*/
        }
    }

    private IEnumerator IdleAtCheckpoint()
    {
        isIdle = true;

        base.EstadoPatrulla();
        if (animaciones != null) animaciones.SetFloat("velocidad", 0); // Detiene la animación de caminar

        yield return new WaitForSeconds(3f); // Pausa de 3 segundos

        indice = (indice + 1) % CheckPoints.Length; // Cambia al siguiente checkpoint
        CambiarEstado(Estados.patrulla); // Vuelve a patrulla
        isIdle = false; // Desmarca el Idle
    }

    public override void EstadoSeguir()
    {
        base.EstadoSeguir();
        if (animaciones != null) animaciones.SetFloat("velocidad", 1);
        if (animaciones != null) animaciones.SetBool("atacando", false);
        agente.SetDestination(target.position);
        Vector3 targetPosition = target.position;
        targetPosition.y -= 1.0f;//preguntar si esta bien esto, modifica la posicion del target durante la animacion de atacar del enemigo
        transform.LookAt(targetPosition, Vector3.up);

    }
    public override void EstadoAtacar()
    {
        //Quaternion rot = new Quaternion(45,0,0,0);
        base.EstadoAtacar();

        if (animaciones != null) animaciones.SetFloat("velocidad", 0);
        if (animaciones != null) animaciones.SetBool("atacando", true);

        agente.SetDestination(transform.position);
        Vector3 targetPosition = target.position;
        targetPosition.y -= 1.0f;//preguntar si esta bien esto, modifica la posicion del target durante la animacion de atacar del enemigo
        transform.LookAt(targetPosition, Vector3.up);
    }
    public override void EstadoMuerto()
    {
        base.EstadoMuerto();
        if (animaciones != null) animaciones.SetBool("vivo", false);
        agente.enabled = false;
    }
    [ContextMenu("Matar")]//para poder cambiar a este estado en el editor de unity

    public void Matar()
    {
        CambiarEstado(Estados.muerto);
    }
    public void Atacar()
    {
        //Personaje.singleton.vida.CausarDaño(daño);
    }

}
