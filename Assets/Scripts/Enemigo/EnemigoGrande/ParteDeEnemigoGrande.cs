using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParteEnemigoGrande : MonoBehaviour
{
    [SerializeField] private EnemigoGrande mainScript;
    [SerializeField] private float multiplicadorDanho;

    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void RecibirDanho(float danhoRecibido)
    {
        mainScript.Vida -= danhoRecibido * multiplicadorDanho;
        if (mainScript.Vida <= 0)
        {
            mainScript.Matar();
        }
    }
    public void Explotar(float fuerzaExplosion, Vector3 puntoImpacto, float radioExplosion, float upModifier)
    {
        //desactivar todo (animaciones, navmeshAgent, huesos: dynamic)
        mainScript.Matar();
        rb.AddExplosionForce(fuerzaExplosion, puntoImpacto, radioExplosion, upModifier, ForceMode.Impulse);

    }
}
