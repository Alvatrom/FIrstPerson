using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuertaLlave : MonoBehaviour
{
    [SerializeField] float rotacionAngulo = -130f;
    //[SerializeField] bool lectura = false;
    [SerializeField] private Cartel mainScript;

    // Start is called before the first frame update
    void Start()
    {
        //mainScript.Leido = lectura;
    }

    // Update is called once per frame
    void Update()
    {
        AbrirPuerta();
    }
    public void AbrirPuerta()
    {
        if (mainScript.Leido)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotacionAngulo, transform.eulerAngles.z);
        }
    }
}
