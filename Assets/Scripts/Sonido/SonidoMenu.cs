using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonidoMenu : MonoBehaviour
{
    [SerializeField] AudioSource sound, click;
    [SerializeField] AudioClip soundMenu, soundclick;


    public void SoundButton()//funcion llamada desde el boton a traves del componente Event Trigger
    {
        //Eliges el sonido que sonara
        sound.clip = soundMenu;
        //Se activa y se desactiva para que se genere el sonido
        sound.enabled = false;
        sound.enabled = true;
    }
    public void SoundButtonClick()
    {
        //elegir el sonido que sonara
        sound.clip = soundclick;
        //Lo activo y desactivo para que se genere el sonido
        sound.enabled = false;
        sound.enabled = true;
    }
}
