using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaManual : MonoBehaviour
{

    [SerializeField] private ArmaSO misDatos;
    [SerializeField] private ParticleSystem system;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam= Camera.main;//"Main Caamera"debe de estar etiquetada
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            system.Play();// ejecutar sistema de particulas
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, misDatos.distanciaAtaque))
            {
                if (hitInfo.transform.CompareTag("Enemigo"))
                {
                    Debug.Log("Disparo recibido "+ hitInfo.transform.name);
                    hitInfo.transform.GetComponent<Enemigo>().RecibirDanho(misDatos.distanciaAtaque);
                }
                //Debug.Log(hitInfo.transform.name);//muestro el nombre de a quien he impactado

            }
        }

    }
}
