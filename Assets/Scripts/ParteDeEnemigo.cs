using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParteDeEnemigo : MonoBehaviour
{
    [SerializeField] private Enemigo mainScript;
    [SerializeField] private float multiplicadorDanho;
    public void RecibirDanho(float danhoRecibido)
    {
        mainScript.Vida -= danhoRecibido * multiplicadorDanho;
        if (mainScript.Vida <= 0)
        {
            mainScript.Morir();
        }
    }
}
