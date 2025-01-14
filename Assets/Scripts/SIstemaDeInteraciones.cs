using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class SistemaDeInteraciones : MonoBehaviour
{
    private Camera cam;
    private Transform interactuableActual;
    [SerializeField] private float distanciaInterracion;
    //[SerializeField] Canvas teclaE;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        DeteccionInteractuable();
    }

    private void DeteccionInteractuable()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, distanciaInterracion))
        {
            if (hit.transform.TryGetComponent(out AmmoBox scriptAmmobox))
            {
                interactuableActual = scriptAmmobox.transform;
                interactuableActual.GetComponent<Outline>().enabled = true;
                //teclaE.enabled = true;
                if (Input.GetKey(KeyCode.E))
                {
                    scriptAmmobox.AbrirCaja();
                }
            }
        }
        else if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit1, distanciaInterracion))
        {
            if (hit1.transform.TryGetComponent(out Cartel scriptCartel))
            {
                interactuableActual = scriptCartel.transform;
                interactuableActual.GetComponent<Outline>().enabled = true;
                //teclaE.enabled = true;
                /*if (Input.GetKey(KeyCode.E))
                {
                    scriptCartel.AbrirCaja();
                }*/
            }
        }
        else if (interactuableActual != null)
        {
            interactuableActual.GetComponent<Outline>().enabled = false;
            interactuableActual = null;
        }
    }

    
}
