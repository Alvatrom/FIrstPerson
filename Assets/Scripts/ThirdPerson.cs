using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPerson : MonoBehaviour
{
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float smoothTime;

    CharacterController controller;
    private Animator animaciones;

    private float velocidadRotacion;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animaciones = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        MoverYRotar();
    }

    private void MoverYRotar()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 input = new Vector2(h, v).normalized;


        //si el jugador ha tocado teclas...
        if (input.sqrMagnitude > 0)// de esta manera es mas optimo
        {
            animaciones.SetBool("walking", true);
            //calculo el angulo al que tengo que rotarme en funcion de los inputs y camara
            float angulo = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

            float anguloSuave = Mathf.SmoothDampAngle(transform.eulerAngles.y, angulo,ref velocidadRotacion, smoothTime);


            //el cuerpo quede orientado hacia donde me muevo(camara)
            transform.eulerAngles = new Vector3(0, anguloSuave, 0);
            //Mi movimiento tambien ha quedado rotado en base al angulo calculado
            Vector3 movimiento = Quaternion.Euler(0, angulo, 0) * Vector3.forward;

            controller.Move(movimiento * velocidadMovimiento * Time.deltaTime);
        }
        else
        {
            animaciones.SetBool("walking", false);

        }
    }
}
