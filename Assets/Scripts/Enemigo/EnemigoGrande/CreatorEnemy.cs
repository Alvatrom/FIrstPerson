using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif
//para que solo funcione en el unity editor
public class CreatorEnemy : MonoBehaviour
{
    protected NavMeshAgent agente;


    //[SerializeField] float velocidad = 5f;
    public Estados estado;
    public float distanciaSeguir, distanciaAtacar, distanciaEscapar;

    //para que se autoseleccione
    public bool autoSeleccionarTarget = true;
    public Transform target;
    public float distancia, distanciaGizmos;

    public bool vivo = true;

    public virtual void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
        estado = Estados.patrulla;
        StartCoroutine(CalcularDistancia());
    }

    private void Start()
    {
        if (autoSeleccionarTarget)
        {
            target = FirstPerson.singleton.transform;
        }
    }

    protected virtual void Update()
    {
        CheckEstado();
    }

    private void CheckEstado()
    {
        switch (estado)
        {
            case Estados.patrulla:
                EstadoPatrulla();
                break;
            case Estados.seguir:
                transform.LookAt(target, Vector3.up);//para que no haga el giro tan lento mientras se mueve
                EstadoSeguir();
                break;
            case Estados.atacar:
                EstadoAtacar();
                break;
            case Estados.alerta:
                transform.LookAt(target, Vector3.up);
                EstadoAlerta();
                break;
            case Estados.muerto:
                EstadoMuerto();
                break;
        }
    }
    public void CambiarEstado(Estados e)
    {
        if (estado == e)//en el caso que ya estemos en ese estado no volver a cambiarlo
            return;
        switch (e)
        {
            case Estados.patrulla:
                EstadoPatrulla();
                break;
            case Estados.seguir:
                EstadoSeguir();
                break;
            case Estados.atacar:
                EstadoAtacar();
                break;
            case Estados.alerta:
                EstadoAlerta();
                break;
            case Estados.muerto:
                vivo = false;
                EstadoMuerto();
                break;
        }
        estado = e;
    }


    public virtual void EstadoPatrulla()
    {
        if (!agente.pathPending && distancia < distanciaSeguir)
        {
            CambiarEstado(Estados.seguir);
        }
    }
    public virtual void EstadoSeguir()
    {
        if (!agente.pathPending && distancia < distanciaAtacar)
        {
            CambiarEstado(Estados.atacar);
        }
        else if (distancia > distanciaEscapar)
        {
            CambiarEstado(Estados.patrulla);
        }
    }
    public virtual void EstadoAtacar()
    {
        if (!agente.pathPending && distancia > distanciaAtacar + 0.4f)
        {
            CambiarEstado(Estados.seguir);
        }
    }
    public virtual void EstadoAlerta()
    {
        
    }
    public virtual void EstadoMuerto()
    {

    }

    IEnumerator CalcularDistancia() // Corutina para calcular la distancia al jugador
    {
        while (vivo) // Solo calcular mientras el enemigo esté vivo
        {
            if (target != null && target.gameObject.activeInHierarchy) // Verifica si el target es válido y está activo
            {
                distancia = Vector3.Distance(transform.position, target.position); // Calcula la distancia
                // Verifica si el target tiene el componente FirstPerson
                if (agente.destination == target.position)
                {
                    agente.stoppingDistance = 1.5f; // Distancia menor si el destination del navmesh es el player
                }
                else
                {
                    agente.stoppingDistance = 0f; // Distancia por defecto
                }
            }
            else
            {
                distancia = Mathf.Infinity; // Si no hay target, establece la distancia como infinita
            }

            yield return new WaitForSeconds(0.1f); // Calcula la distancia cada 0.1 segundos (ajústalo si es necesario)
        }
    }
    public enum Estados
    {
        patrulla = 0,
        seguir = 1,
        atacar = 2,
        alerta = 3,
        muerto = 4,
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, distanciaAtacar);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, distanciaSeguir);
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, Vector3.up, distanciaEscapar);
    }
#endif
}
