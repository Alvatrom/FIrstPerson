using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
//para que solo funcione en el unity editor
public class CreatorEnemy : MonoBehaviour
{
    //el objetivo que seguira
    [SerializeField] Transform player;
    [SerializeField] float velocidad = 5f;
    public Estados estado;
    public float distanciaSeguir, distanciaAtacar, distanciaEscapar;

    //para que se autoseleccione
    public bool autoSeleccionarTarget = true;
    public Transform target;
    public float distancia, distanciaGizmos;

    public bool vivo = true;

    public virtual void Awake()
    {
        StartCoroutine(CalcularDistancia());
    }

    private void Start()
    {
        if (autoSeleccionarTarget)
        {
            //para que encuentre el objeto con el tag player // tendian que estar en el awake
            //target = GameObject.FindGameObjectWithTag("Player").transform; // tendian que estar en el awake
            target = FirstPerson.singleton.transform;
            //para calcular contunuamente la distancia al jugador, consume // tendian que estar en el awake
            //player = GameObject.FindGameObjectWithTag("Player").transform; // tendian que estar en el awake

        }
    }

    void Update()
    {
        // Verifica si hay un objetivo asignado
        if (player != null)
        {
            //vectro dirireccion hacia el jugaddor
            Vector3 direccion = (player.position - transform.position).normalized;
            transform.position += direccion * velocidad * Time.deltaTime;

            // Opcional: Gira el enemigo para que mire hacia el objetivo
            transform.LookAt(player);
        }
    }
    private void LateUpdate()
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
            case Estados.muerto:
                vivo = false;
                EstadoMuerto();
                break;
        }
        estado = e;
    }


    public virtual void EstadoPatrulla()
    {
        if (distancia < distanciaSeguir)
        {
            CambiarEstado(Estados.seguir);
        }
    }
    public virtual void EstadoSeguir()
    {
        if (distancia < distanciaAtacar)
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
        if (distancia > distanciaAtacar + 0.4f)
        {
            CambiarEstado(Estados.seguir);
        }
    }
    public virtual void EstadoMuerto()
    {

    }

    IEnumerator CalcularDistancia()// esto quiere decir que es una corutina(puede pausarse y reanudarse despues de un tiempo)
    {
        while (vivo)//para optimizar, si esta muerto nos ahorramos el calculo
        {
            yield return new WaitForSeconds(0.2f);//con esta pausa no se cuelga el unity
            if (target != null)// necesario para que no ocurran errores
            {
                distancia = Vector3.Distance(transform.position, target.position);
            }
        }
    }
    public enum Estados
    {
        patrulla = 0,
        seguir = 1,
        atacar = 2,
        muerto = 3,
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
