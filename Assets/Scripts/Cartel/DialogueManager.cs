using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public bool mostrando = false;
    public string[] textos;
    public int indiceTextos = 0;

    public string fraseActual = "";
    public int indiceFrase = 0;

    public GameObject canvasDialogue;
    public TMP_Text textDialogue;

    //SINGLETON
    public static DialogueManager instance;
    void Start()
    {
        //INICIALIZACION SINGLETON
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //FIN INICIALIZACION SINGLETON

        // NO DESTRUIR ENTRE ESCENAS
        DontDestroyOnLoad(gameObject);

        canvasDialogue.SetActive(false);
    }
    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E) && mostrando)
        {
            if (indiceFrase < fraseActual.Length)
            {
                textDialogue.text = fraseActual;
                indiceFrase = fraseActual.Length;
                CancelInvoke();
            }
            else if (indiceTextos < textos.Length)//longitud del array, quedan frases por mostrar
            {
                //textDialogue.text = textos[indiceTextos];
                fraseActual = textos[indiceTextos];
                indiceFrase = 0;
                textDialogue.text = "";
                InvokeRepeating("LetraALetra", 0, 0.2f);
                indiceTextos++;
            }
            else //No quedan frases por mostrar 
            {
                OcultarTexto();
            }
        }
    }
    void LetraALetra()
    {
        if (indiceFrase < fraseActual.Length)
        {
            textDialogue.text += fraseActual[indiceFrase];
            indiceFrase++;
        }
        else
        {
            CancelInvoke();
        }
    }
    public void MostrarTexto(string texto)
    {
        canvasDialogue.SetActive(true);
        textDialogue.text = texto;
        print("DM -> " + texto);
    }
    public void MostrarTexto(string[] _textos)
    {
        if (!mostrando)
        {
            mostrando = true;
            textos = _textos;
            indiceTextos = 0;// para que cada vez que abra un cartel inicie en el primer mensaje
            canvasDialogue.SetActive(true);
            //textDialogue.text = textos[indiceTextos];

        }
    }
    public void OcultarTexto()
    {
        canvasDialogue.SetActive(false);
        mostrando = false;
    }

    public void ResetDialogueManager()
    {
        mostrando = false;
        textos = null;
        indiceTextos = 0;
        fraseActual = "";
        indiceFrase = 0;
        if (canvasDialogue != null)
        {
            canvasDialogue.SetActive(false);
        }
    }

}