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

    [Header("Sistema de ataque")]
    [SerializeField] private float danhoEnemigo, danhoRecibido;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float radioAtaque = 1;
    [SerializeField] private LayerMask queEsDanhable;


    [SerializeField] private FirstPerson player;
    private bool ventanaAbierta;
    private bool puedoDanhar = true;

    [SerializeField] private float vida;
    [SerializeField] private bool muerto = false;



    public Transform[] CheckPoints;
    private int indice, indiceSusto = 0;
    public float distanciaCheckPoints;
    private bool isIdle,asustando = false,destruyendo = false;

    private Coroutine aumentarSustoCoroutine;
    private EstadoVisto estadoVisto = EstadoVisto.NoVisto;

    Rigidbody[] huesos;

    public float Vida { get => vida; set => vida = value; }


    public override void Awake()//para poder modificar el del padre,llama lo del padre pero permite modificarlo para este caso 
    {
        base.Awake();

        huesos = GetComponentsInChildren<Rigidbody>();
        player = FirstPerson.FindObjectOfType<FirstPerson>();

        CambiarEstadoHuesos(true);
        estado = Estados.patrulla;

    }
    public void FixedUpdate()
    {
        if (ventanaAbierta && puedoDanhar)
        {
            DetectarImpacto();
        }
    }

    public void CambiarEstadoHuesos(bool estado)
    {
        for (int i = 0; i < huesos.Length; i++)
        {
            huesos[i].isKinematic = estado;
        }
    }
    public void RecibirDanho(float danhoRecibido)
    {
        vida -= danhoRecibido;
        if (vida <= 0)
        {
            CambiarEstadoHuesos(false);
            Matar();
            //CambiarEstado(Estados.muerto);
        }
    }

    public override void EstadoPatrulla()
    {
        base.EstadoPatrulla();

        if (asustando) return;
        if(muerto) return;

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
        if(muerto) return;

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

        CambiarEstadoHuesos(false);

        agente.enabled = false;
        agente.isStopped = true;
        muerto = true;

        Destroy(gameObject,10);
    }
    [ContextMenu("Matar")]//para poder cambiar a este estado en el editor de unity


    public void Matar()
    {
        CambiarEstado(Estados.muerto);
        //CambiarEstadoHuesos(false);
    }
    public void Atacar()
    {
        //Personaje.singleton.vida.CausarDaño(daño);
    }

    private void FinAtaque()
    {
        agente.isStopped = false;//me paro
        animaciones.SetBool("atacando", false);
        puedoDanhar = true;
    }

    private void DetectarImpacto()
    {
        //1º referenciar el attackPoint
        //2º crear una variable que represente el radio de ataque
        //3 crear una variable que represente que es dañable,(layer)


        Collider[] collDetectados = Physics.OverlapSphere(attackPoint.position, radioAtaque, queEsDanhable);

        //si hemos detectado algo dentro de nuestro radar
        if (collDetectados.Length > 0)
        {
            //pasoo collider por collider aplicando daño
            for (int i = 0; i < collDetectados.Length; i++)
            {
                collDetectados[i].GetComponent<FirstPerson>().RecibirDanho(danhoEnemigo);

            }
            puedoDanhar = false;
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


}
