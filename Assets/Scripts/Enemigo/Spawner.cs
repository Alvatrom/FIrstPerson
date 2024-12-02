using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemigoPrefab;
    [SerializeField] private Transform[] puntosSpawn;
    [SerializeField] private int numeroNiveles;
    [SerializeField] private int rondasPorNivel;
    [SerializeField] private int spawnsPorRonda;
    [SerializeField] private float esperaEntreSpawns;
    [SerializeField] private float esperaEntreRondas;
    [SerializeField] private float esperaEntreNiveles;
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
        for (int i = 0; i < numeroNiveles; i++)
        {
            for (int j = 0; j < rondasPorNivel; j++)
            {
                for (int k = 0; k < spawnsPorRonda; k++)
                {
                    int indiceAleatorio = Random.Range(0, puntosSpawn.Length);
                    RespawnMetodo();

                    //podriamos poner un contador de zombis vivos?
                    yield return new WaitForSeconds(esperaEntreSpawns);
                }

                //actualizar texto de ronda
                yield return new WaitForSeconds(esperaEntreRondas);
            }

            //actualizar texto de nuevo nivel
            yield return new WaitForSeconds(esperaEntreNiveles);
        }
    }
    private void RespawnMetodo()
    {
        Instantiate(enemigoPrefab, puntosSpawn[Random.Range(0, puntosSpawn.Length)].position, Quaternion.identity);
    }
}
