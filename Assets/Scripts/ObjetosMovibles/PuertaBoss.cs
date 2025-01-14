using System.Collections;
using UnityEngine;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PuertaBoss : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform PuertaIz, PuertaDe;

    [SerializeField] TMP_Text textoLlaves;

    [SerializeField] float rotacionPuertaDer, rotacionPuertaIz;
    public bool abierto = false;
    [SerializeField] FirstPerson mainScript;
    [SerializeField] Canvas teclaInteractuar;
    [SerializeField] float distanciaInteraccion;



    // Start is called before the first frame update
    void Start()
    {
        if (teclaInteractuar == null)
        {
            teclaInteractuar = GameObject.Find("CanvasInteraccion")?.GetComponent<Canvas>();
        }
        //Canvas teclaE = GameObject.Find("CanvasEstatua")?.GetComponent<Canvas>();// la interrogacion es para , en el caso que no encuentre el canvas y devuelva un null, no bisque el componente y devuelva un null references
        if (teclaInteractuar != null)
        {
            teclaInteractuar.enabled = false;
        }
        else
        {
            Debug.LogWarning("No se encontró un Canvas llamado 'CanvasInteraccion'.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            //calcular la distancia
            float distancia = Vector3.Distance(transform.position, player.position);
            if (distancia < distanciaInteraccion)
            {
                teclaInteractuar.enabled = true;

            }
            else
            {
                teclaInteractuar.enabled = false;
            }

            if (teclaInteractuar.enabled && Input.GetKeyDown(KeyCode.E) && mainScript.Llaves > 0 && abierto == false)
            {
                PuertaDe.RotateAround(PuertaDe.position, PuertaDe.up, rotacionPuertaDer);//coge la ubicacion local del objeto, .up es en eje y, y el angulo de rotacion deseado
                PuertaIz.RotateAround(PuertaIz.position, PuertaIz.up, rotacionPuertaIz);
                mainScript.Llaves--;
                textoLlaves.SetText("Keys: " + mainScript.llaves + "/" + mainScript.llavesTotales);
                abierto = true;
            }
            else if (!teclaInteractuar.enabled)
            {
                return;
            }
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position + Vector3.up * 2.5f, "Tecla E.gif", true);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, distanciaInteraccion);
    }
#endif
}
