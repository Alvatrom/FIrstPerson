using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanzaGranadas : MonoBehaviour
{
    [SerializeField] private GameObject granadaPrefab;
    [SerializeField] private Transform spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //instanciar una copia del prefab  granada en la boca del caañon
            Instantiate(granadaPrefab, spawnPoint.position, transform.rotation);
        } 
    }
}
