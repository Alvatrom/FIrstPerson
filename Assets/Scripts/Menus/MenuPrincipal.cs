using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [SerializeField] private new AudioSource audio;

    [SerializeField] GameObject menuEspera;


    void Start()
    {
        audio = GetComponent<AudioSource>();
        Time.timeScale = 1.0f;

        if (menuEspera == null)
        {
            menuEspera = GameObject.Find("CanvasPausa");
        }

        // Verificar y desactivar los elementos si se encontraron
        if (menuEspera != null)
        {
            menuEspera.SetActive(false);
        }
        else
        {
            Debug.LogWarning("menuPausa no se encontró en la escena.");
        }

    }

    void Update()
    {

    }

    public void StartGame()
    {
        StartCoroutine(EsperarYCargarEscena());
    }


    
    private IEnumerator EsperarYCargarEscena()
    {
        menuEspera.SetActive(true);
        yield return new WaitForSeconds(7f);// espera 1 segundo
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
