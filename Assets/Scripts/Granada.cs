using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granada : MonoBehaviour
{
    [SerializeField] private float fuerzaImpulso;
    [SerializeField] private GameObject explosion;
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
    }
}
