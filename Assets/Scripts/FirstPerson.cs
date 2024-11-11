using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPerson : MonoBehaviour
{
    [SerializeField] private float vidas;


    [Header("Movimiento")]
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float factorGravedad;
    [SerializeField] private float alturaSalto;



    [Header("Detector Suelo")]
    [SerializeField] private float radioDeteccion;
    [SerializeField] private Transform pies;
    [SerializeField] private LayerMask queEsSuelo;

    CharacterController controller;

    private Vector3 movimientoVertical;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        //bloquea el raton en el centro de la pantalla y lo oculta
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    // Update is called once per frame
    void Update()
    {
        MoverYRotar();

        AplicarGravedad();

        if(DetectorSuelo() == true)
        {
            //Cada vez que aterricemos reiniciamos el calculo de gravedad
            movimientoVertical.y = 0;
            Saltar();

        }
    }

    private void Saltar()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Aplico "formula" de salto
            movimientoVertical.y = Mathf.Sqrt(-2 * factorGravedad * alturaSalto);
        }
    }

    private void MoverYRotar()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(h, v).normalized;

        //calculo el angulo al que tengo que rotarme en funcion de los inputs y camara


        //Roto el cuerpo de forma constante con la rotacion "y" de la camara
        transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

        //si el jugador ha tocado teclas...
        if (input.magnitude > 0)
        {
             float angulo = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            //Mi movimiento tambien ha quedado rotado en base al angulo calculado
            Vector3 movimiento = Quaternion.Euler(0, angulo, 0) * Vector3.forward;
            controller.Move(movimiento * velocidadMovimiento * Time.deltaTime);
        }
    }
    private void AplicarGravedad()
    {
        //Mi velocidadVertical va en aumento a cierto factor por segundo.
        movimientoVertical.y += factorGravedad * Time.deltaTime;
        controller.Move(movimientoVertical* Time.deltaTime);
        // se multiplica dos veces porque la gravedad es m/s^2, en unity ponemos la gravedad en negativo porque aqui el calculo esta en positivo
    }

    private bool DetectorSuelo()
    {
        //tirar una esfera de deteccion en los pies con cierto radio
        bool resultado= Physics.CheckSphere(pies.position, radioDeteccion, queEsSuelo);
        return resultado;
    }

    public void RecibirDanho(float x)
    {
        vidas -= x;
    }
    //Sinonimos de OnCollisionEnter PERO para un C.C.
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("ParteEnemigo"))
        {
            Vector3 vectorPush = hit.gameObject.transform.position - transform.position;
            hit.gameObject.GetComponent<Rigidbody>().AddForce(vectorPush.normalized * 150, ForceMode.Impulse);
        }
    }

    // metodo que se ejecuta automaticamente para dibujar cualquier figura
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pies.position, radioDeteccion);
    }
}
