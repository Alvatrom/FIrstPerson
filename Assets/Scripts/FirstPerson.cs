using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPerson : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float factorGravedad;



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
        
    }

    // Update is called once per frame
    void Update()
    {
        MoverYRotar();
        AplicarGravedad();
        DetectorSuelo();
    }

    private void MoverYRotar()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 input = new Vector2(h, v).normalized;

        //calculo el angulo al que tengo que rotarme en funcion de los inputs y camara
        float angulo = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

        //el cuerpo quede orientado hacia donde me muevo(camara)
        transform.eulerAngles = new Vector3(0, angulo, 0);

        //si el jugador ha tocado teclas...
        if (input.magnitude > 0)
        {
            //Mi movimiento tambien ha quedado rotado en base al angulo calculado
            Vector3 movimiento = Quaternion.Euler(0, angulo, 0) * Vector3.forward;

            controller.Move(input * velocidadMovimiento * Time.deltaTime);
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
}
