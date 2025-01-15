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
        if (Input.GetMouseButtonDown(0))//el 0 es boton iz, 1 boton dere, 2 boton central
        {
            system.Play();// ejecutar sistema de particulas
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, misDatos.distanciaAtaque))
            {
                if (hitInfo.transform.CompareTag("ParteEnemigo"))
                {
                    Debug.Log("Disparo recibido "+ hitInfo.transform.name);
                    if (hitInfo.transform.GetComponent<ParteDeEnemigo>())
                    {
                        hitInfo.transform.GetComponent<ParteDeEnemigo>().RecibirDanho(misDatos.danhoAtaque);
                    }
                    //hitInfo.transform.GetComponent<Enemigo>().RecibirDanho(misDatos.danhoAtaque);
                    if (hitInfo.transform.GetComponent<EnemigoGrande>())
                    {
                        hitInfo.transform.GetComponent<ParteEnemigoGrande>().RecibirDanho(misDatos.danhoAtaque);
                    }
                }
                /*if (hitInfo.transform.CompareTag("ParteEnemigo"))
                {
                    Debug.Log("Disparo recibido " + hitInfo.transform.name);
                    hitInfo.transform.GetComponent<EnemigoGrande>().RecibirDanho(misDatos.danhoAtaque);
                }*/
                //Debug.Log(hitInfo.transform.name);//muestro el nombre de a quien he impactado

            }
        }

    }
}
