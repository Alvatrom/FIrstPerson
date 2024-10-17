using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPerson : MonoBehaviour
{
    [SerializeField] private float velocidadMovimiento;

    CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        MoverRotar();


    }
    void MoverRotar()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 movimiento = new Vector3(h,0,v).normalized;

        float anguloRotacion = Mathf.Atan2(movimiento.x, movimiento.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        if (movimiento.magnitude > 0)
        {
            //el cuerpo quede orientado hacia donde me muevo(camara)
            transform.eulerAngles = new Vector3(0, anguloRotacion, 0);
            controller.Move(movimiento * velocidadMovimiento * Time.deltaTime);
        }
        
    }
}
