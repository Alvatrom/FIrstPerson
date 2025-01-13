using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class SistemaDeInteraciones : MonoBehaviour
{
    private Camera cam;
    private Transform interactuableActual;
    [SerializeField] private float distanciaInterracion;
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
                if (Input.GetKey(KeyCode.E))
                {
                    scriptAmmobox.AbrirCaja();
                }
            }
        }
        else if (interactuableActual != null)
        {
            interactuableActual.GetComponent<Outline>().enabled = false;
            interactuableActual = null;
        }
    }

    
}
