using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semaforo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EjemploSemaforo());
    }
    
    void Update()
    {
        
    }
    IEnumerator EjemploSemaforo()
    {
        while (true)
        {
            Debug.Log("Verde");//para que te ponga una espera
            yield return new WaitForSeconds(2);
            Debug.Log("Amarillo");
            yield return new WaitForSeconds(4);
            Debug.Log("Rojo");
        }
    }
}
