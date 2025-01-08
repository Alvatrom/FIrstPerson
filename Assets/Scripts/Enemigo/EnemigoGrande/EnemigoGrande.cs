using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemigoGrande : CreatorEnemy
{
    public Animator animaciones;
    public float daño = 3;


    public Transform[] CheckPoints;
    private int indice, indiceSusto = 0;
    public float distanciaCheckPoints;
    private bool isIdle,asustando = false;

    private Coroutine aumentarSustoCoroutine;
    private EstadoVisto estadoVisto = EstadoVisto.NoVisto;


    public override void Awake()//para poder modificar el del padre,llama lo del padre pero permite modificarlo para este caso 
    {
        base.Awake();
        estado = Estados.patrulla;

    }

    public override void EstadoPatrulla()
    {
        base.EstadoPatrulla();

        if (asustando) return;

        if (isIdle) return;//si esta en idle no continua

        if (animaciones != null)
        {
            animaciones.SetFloat("velocidad", 1);
            animaciones.SetBool("atacando", false);
        }

        if (!agente.pathPending && distancia < distanciaSeguir)
        {
            CambiarEstado(Estados.seguir);
            return;
        }

        agente.SetDestination(CheckPoints[indice].position);


        switch (estadoVisto)
        {
            case EstadoVisto.NoVisto:
                if (!agente.pathPending && distancia < distanciaSeguir)
                {
                    agente.isStopped = true;
                    estadoVisto = EstadoVisto.Visto; // Cambiar el estado

                    if (animaciones != null) animaciones.SetBool("visto", true);
                    EstadoAlerta();
                }
                break;

            case EstadoVisto.Visto:
                Debug.Log("El enemigo ve al objetivo, se dirije hacia el");
                agente.isStopped = false;

                if (animaciones != null)
                {
                    animaciones.SetBool("visto", true);
                    animaciones.SetFloat("velocidad", 1);
                }
                agente.speed = 1.43f;
                break;

            case EstadoVisto.Alerta:
                if (!agente.pathPending && distancia < distanciaAtacar)
                {
                    agente.isStopped = true;
                    if (animaciones != null) animaciones.SetBool("visto", true);
                    EstadoAtacar();
                }
                else if (!agente.pathPending && distancia < distanciaEscapar && distancia > distanciaAtacar && asustando == false)
                {
                    agente.isStopped = false;

                    if (animaciones != null)
                    {
                        animaciones.SetBool("visto", true);
                        animaciones.SetBool("atacando", false);
                        animaciones.SetFloat("velocidad", 1);
                        animaciones.SetBool("corriendo", true);
                    }
                    
                    agente.speed = 3.65f;
                }
                else if (!agente.pathPending && distancia > distanciaEscapar)
                {
                    estadoVisto = EstadoVisto.NoVisto;
                    agente.speed = 1.43f;

                    if (animaciones != null)
                    {
                        animaciones.SetBool("visto", false);
                        animaciones.SetBool("corriendo", false);

                    }
                }
                break;
        }

        if (!agente.pathPending && agente.remainingDistance <= agente.stoppingDistance && distancia > distanciaSeguir)
        {
            if (animaciones != null)
            {
                animaciones.SetBool("visto", false);
            }
            estadoVisto = EstadoVisto.NoVisto; // Reinicia el estado
            StartCoroutine(IdleAtCheckpoint());
        }

    }

    private IEnumerator IdleAtCheckpoint()
    {
        isIdle = true;
        Debug.Log("idlecheckpoints");

        base.EstadoPatrulla();
        if (animaciones != null) animaciones.SetFloat("velocidad", 0); // Detiene la animación de caminar

        yield return new WaitForSeconds(3f); // Pausa de 3 segundos

        indice = (indice + 1) % CheckPoints.Length; // Cambia al siguiente checkpoint
        CambiarEstado(Estados.patrulla); // Vuelve a patrulla
        isIdle = false; // Desmarca el Idle
    }

    public override void EstadoSeguir()
    {
        EnfocarObjetivo();
        base.EstadoSeguir();
        if (asustando) return;

        if (animaciones != null)
        {
            animaciones.SetFloat("velocidad", 1);
            animaciones.SetBool("atacando", false);
        }
        agente.SetDestination(target.position);


        switch (estadoVisto)
        {
            case EstadoVisto.NoVisto:
                if (!agente.pathPending && distancia < distanciaSeguir)
                {
                    agente.isStopped = true;
                    estadoVisto = EstadoVisto.Visto; // Cambiar el estado
                    if (animaciones != null) animaciones.SetBool("visto", true);
                    EstadoAlerta();
                }
                break;

            case EstadoVisto.Visto:
                Debug.Log("El enemigo ya ha visto al objetivo, no realiza esta acción.");
                break;

            case EstadoVisto.Alerta:
                if(!agente.pathPending && distancia < distanciaAtacar)
                {
                    agente.isStopped = true;
                    if (animaciones != null) animaciones.SetBool("visto", true);
                    EstadoAtacar();
                }
                else if(!agente.pathPending && distancia < distanciaEscapar && distancia > distanciaAtacar && asustando == false)
                {
                    agente.isStopped = false;
                    if (animaciones != null)
                    {
                        animaciones.SetBool("visto", true);
                        animaciones.SetBool("atacando", false);
                        animaciones.SetFloat("velocidad", 1);
                        animaciones.SetBool("corriendo", true);
                    } 
                    agente.speed = 3.65f;
                }
                else if (!agente.pathPending && distancia > distanciaEscapar)
                {
                    estadoVisto = EstadoVisto.NoVisto;
                    if (animaciones != null)
                    {
                        animaciones.SetBool("corriendo", false);
                        animaciones.SetBool("visto", false);
                    }
                    agente.speed = 1.43f;
                    EstadoPatrulla();
                }
                    break;
        }

    }

    public override void EstadoAlerta()
    {
        EnfocarObjetivo();
        agente.isStopped = true;

        if (animaciones != null)
        {
            animaciones.SetFloat("velocidad", 0);
            animaciones.SetBool("atacando", false);
            Debug.Log("preparativos susto");
        }
        aumentarSustoCoroutine = StartCoroutine(AumentarSusto("susto", 0.5f, 1f, () =>{ agente.isStopped = false; estadoVisto = EstadoVisto.Alerta; CambiarEstado(Estados.seguir); /*if (animaciones != null) animaciones.SetBool("corriendo", true);*/ }));//se realiza esto de la corutina para poder ser completada

        

    }
    public enum EstadoVisto
    {
        NoVisto,
        Visto,
        Alerta
    }

    private IEnumerator AumentarSusto(string parametro, float incremento, float maximo, System.Action onComplete)//para que complete entero
    {
        asustando = true;
        //estadoVisto = EstadoVisto.Alerta;
        Debug.Log("asustando = true");
        float valorActual = animaciones.GetFloat(parametro);
        EnfocarObjetivo();


        while (valorActual < maximo)
        {
            valorActual += incremento * Time.deltaTime; //Incremento progresivo en función del tiempo
            valorActual = Mathf.Clamp(valorActual, 0, maximo); //Asegurar que no supere el máximo
            animaciones.SetFloat(parametro, valorActual);
            EnfocarObjetivo();

            yield return null; //Esperar al siguiente frame
        }
        onComplete?.Invoke(); //Ejecutar la acción al finalizar
        asustando= false;
        Debug.Log("asustando = false");
        if (animaciones != null)
        {
            animaciones.SetBool("visto", false);
            animaciones.SetFloat("susto", 0);
        }
        EnfocarObjetivo();
    }

    /*private void DetenerAumentarSusto()
    {
        if(aumentarSustoCoroutine== null)
        {
            EnfocarObjetivo();
            StopCoroutine(aumentarSustoCoroutine);
            aumentarSustoCoroutine = null;
            asustando= false;
            Debug.Log("Corutina susto detenida");
        }
    }*/

    public override void EstadoAtacar()
    {
        base.EstadoAtacar();
        if (animaciones != null)
        {
            animaciones.SetBool("atacando", true);
            animaciones.SetFloat("velocidad", 0);
            animaciones.SetBool("corriendo", false);
        } 
        agente.speed = 1.43f;
        Debug.Log("Atacando");

        agente.SetDestination(transform.position);///////////puede que esto este mal
        EnfocarObjetivo();
    }

    private void EnfocarObjetivo()
    {
        //Debug.Log("enfoque");
        //1.calculo vector Unitario que mira hacia el jugador desde nuestra posicion
        Vector3 direccionObjetivo = (target.transform.position - transform.position).normalized;

        //2.Modifico la Y del vector para prevenir que el Enemigo se tumbe.
        direccionObjetivo.y = 0;

        //3.calculo la rotacion para conseguir dicha direccion

        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccionObjetivo);
        transform.rotation = rotacionObjetivo;

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

    /*private void SetAnimationBools(IEnumerable<(string paramName, bool state)> parameters) //tuplas, para cambiar los valores bool de varios strings
    {
        if (animaciones == null) return;

        foreach (var (paramName, state) in parameters)
        {
            animaciones.SetBool(paramName, state);
        }
    }

    private void SetAnimationInts(IEnumerable<(string paramName, int value)> parameters)
    {
        if (animaciones == null) return;

        foreach (var (paramName, value) in parameters)
        {
            animaciones.SetInteger(paramName, value);
        }
    }*/


}
