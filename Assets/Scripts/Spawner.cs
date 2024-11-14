using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemigoPrefab;
    [SerializeField] private Transform[] puntosSpawn;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Respawn());
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Respawn()
    {
        while (true)
        {
            RespawnMetodo();
            yield return new WaitForSeconds(2);
        }
    }
    private void RespawnMetodo()
    {
        Instantiate(enemigoPrefab, puntosSpawn[Random.Range(0, puntosSpawn.Length)].position, Quaternion.identity);
    }
}
