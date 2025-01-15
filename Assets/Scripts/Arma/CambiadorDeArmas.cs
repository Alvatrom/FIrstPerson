using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CambiadorDeArmas : MonoBehaviour
{
    [SerializeField] private GameObject[] armas;
    private int indiceArmaActual;



    void Update()
    {
        CambiarArmaConTeclado();
        CambiarArmaConRaton();
    }

    private void CambiarArmaConRaton()
    {

        float scrollWheel = Input.GetAxis("Mouse ScrollWheel") * 10 * Time.deltaTime;

        if (scrollWheel > 0 && indiceArmaActual < armas.Length - 1)
        {

            CambiarArma(indiceArmaActual + 1);
        }
        else if (scrollWheel < 0 && indiceArmaActual > 0)
        {
            CambiarArma(indiceArmaActual - 1);
        }
        else if (scrollWheel > 0 && indiceArmaActual == armas.Length - 1)
        {
            CambiarArma(0);
        }
        else if (scrollWheel < 0 && indiceArmaActual == 0)
        {
            CambiarArma(armas.Length - 1);
        }
    }
    private void CambiarArmaConTeclado()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            CambiarArma(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            CambiarArma(1);

        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            CambiarArma(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            CambiarArma(3);
        }
    }

    private void CambiarArma(int indiceNuevaArma)
    {

        armas[indiceArmaActual].SetActive(false);
        if (indiceNuevaArma < 0)
        {
            indiceNuevaArma = armas.Length - 1;
        }
        if (indiceNuevaArma > armas.Length)
        {
            indiceNuevaArma = 0;
        }


        armas[indiceNuevaArma].SetActive(true);
        indiceArmaActual = indiceNuevaArma;


    }
}