using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstPerson : MonoBehaviour
{
    public static FirstPerson singleton;

    [SerializeField] private float vidas;

    [SerializeField] TMP_Text textoLlaves, textoVida;
    public int llaves = 0;
    public int llavesTotales= 3;

    public bool isSprinting;

    public float sprintingSpeedMultiplier = 2f;

    private float sprintSpeed = 1;

    public float staminaUseAmount = 5;

    private StaminaBar staminaSlider;



    [Header("Movimiento")]
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float factorGravedad;
    [SerializeField] private float alturaSalto;



    [Header("Detector Suelo")]
    [SerializeField] private float radioDeteccion;
    [SerializeField] private Transform pies;
    [SerializeField] private LayerMask queEsSuelo;

    CharacterController controller;
    //Rigidbody rb;

    //sirve tanto para la gravedad como para los saltos
    private Vector3 movimientoVertical;

    public int Llaves { get => llaves; set => llaves = value; }
    public float Vidas { get => vidas; set => vidas = value; }

    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;// lo del trono, si esta vacio pues me siento yo
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        llavesTotales = 3;
        staminaSlider = FindObjectOfType<StaminaBar>();


        controller = GetComponent<CharacterController>();
        //rb = GetComponent<Rigidbody>();

        if (SceneManager.GetActiveScene().name == "Asylum")
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (SceneManager.GetActiveScene().name == "Start")
        {
            Cursor.lockState = CursorLockMode.None;
        }

        //bloquea el raton en el centro de la pantalla y lo oculta
        //Cursor.lockState = CursorLockMode.Locked;
        
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
            RunCheck();

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


        //Roto el cuerpo de forma constante con la rotacion "y" de la camara
        transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);

        //si el jugador ha tocado teclas...
        if (input.magnitude > 0)
        {
            //calcula el angulo al que tengo que rotarme en funcion de los inputs y camara.
             float angulo = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;

            //float anguloSuave = Mathf.SmoothDampAngle(transform.eulerAngles.y, angulo,ref velocidadRotacion)

            //Mi movimiento tambien ha quedado rotado en base al angulo calculado
            Vector3 movimiento = Quaternion.Euler(0, angulo, 0) * Vector3.forward;

            controller.Move(movimiento * velocidadMovimiento * Time.deltaTime * sprintSpeed);
        }
    }
    public void RunCheck()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            isSprinting = !isSprinting;

            if (isSprinting)
            {
                staminaSlider.UseStamina(staminaUseAmount);
            }
            else
            {
                staminaSlider.UseStamina(0);
            }
        }
        if (isSprinting)
        {
            sprintSpeed = sprintingSpeedMultiplier;
        }
        else
        {
            sprintSpeed = 1;
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


    public void RecibirDanho(float danhoEnemigo)
    {
        vidas -= danhoEnemigo;
        textoVida.SetText("Life: " + vidas);
        if(vidas<=0)
        {
            Destroy(gameObject);
        }

    }

    public void ReiniciarJugador()
    {
        textoLlaves.SetText("Keys: " + llaves + "/" + llavesTotales);

        textoVida.SetText("Life: " + vidas);
        vidas = 100;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Llave"))
        {
            llaves += 1;
            Destroy(other.gameObject);
            //AudioManager.instance.PlaySFX("CapsulaEnergia");
            textoLlaves.SetText("Keys: " + llaves + "/" + llavesTotales);
            /*if (objetos == 5)
            {
                ActivarPortal();
            }*/
        }
        /*if (other.CompareTag("Cura") && vidasRestantes < 3)
        {
            Destroy(other.gameObject);
            AudioManager.instance.PlaySFX("Vida");
            GameManager.instance.RecuperarVida();

        }
        else if (other.CompareTag("Cura") && vidasRestantes == 3)
        {
            vidasLlenas.SetActive(true);
            yield return new WaitForSeconds(1f);
            vidasLlenas.SetActive(false);
        }*/

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
