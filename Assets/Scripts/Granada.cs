using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granada : MonoBehaviour
{
    [SerializeField] private float fuerzaImpulso;

    [Header("Explosiom")]
    [SerializeField] private float radioExplosion;
    [SerializeField] private float fuerzaExplosion;
    [SerializeField] private GameObject explosion;
    [SerializeField] private LayerMask queEsExplotable;

    private Collider[] buffer = new Collider[100];
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();//te puedes ahorrar esta linea porque en la siguiente ya lo estas pidiendo
        rb.AddForce(transform.forward * fuerzaImpulso, ForceMode.Impulse);
        Destroy(gameObject, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(1, 1, 1) * Random.Range(0, 500) * Time.deltaTime;
    }

    private void OnDestroy()//se ejecuta automaticamente cuando va a morir
    {
        Instantiate(explosion, transform.position,Quaternion.identity);

        int numeroDetectados = Physics.OverlapSphereNonAlloc(transform.position, radioExplosion, buffer, queEsExplotable);
        //si el nº de detecciones es superior a 0....
        if(numeroDetectados > 0)
        {
            //recorrer todos los collider detectados....
            for (int i = 0; i < numeroDetectados; i++)
            {
                //por cada collider detectado(huesos), voy a coger el script de cada uno
                if(buffer[i].TryGetComponent(out ParteDeEnemigo scriptHueso))
                {
                    scriptHueso.Explotar(fuerzaExplosion, transform.position,radioExplosion,3.5f);
                }
            }
        }
    }
}
