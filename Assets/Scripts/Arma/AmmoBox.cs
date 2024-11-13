using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void AbrirCaja()
    {
        anim.SetTrigger("abrir");
    }
    public void CerrarCaja()
    {
        anim.SetTrigger("cerrar");
    }
}
