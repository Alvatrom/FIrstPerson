using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject menuPausa, canvasMuerte;


    private int vidas = 3;

    public int Vidas1 { get => vidas; }

    public FirstPerson player;// lo hacemos publico para poder usarlo luego

    //SINGLETON
    public static GameManager instance;



    void Start()
    {
        //INICIALIZACION SINGLETON
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //FIN INICIALIZACION SINGLETON

        // NO DESTRUIR ENTRE ESCENAS
        DontDestroyOnLoad(gameObject);

        BuscarPlayer();

        if (menuPausa == null)
        {
            menuPausa = GameObject.Find("CanvasPausa");
        }

        // Verificar y desactivar los elementos si se encontraron
        if (menuPausa != null)
        {
            menuPausa.SetActive(false);
        }
        else
        {
            Debug.LogWarning("menuPausa no se encontró en la escena.");
        }

        if (canvasMuerte == null)
        {
            canvasMuerte = GameObject.Find("CanvasMuerte");
        }

        if (canvasMuerte != null)
        {
            canvasMuerte.SetActive(false);
        }
        else
        {
            Debug.LogWarning("canvasMuerte no se encontró en la escena.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Asylum")
        {
            BuscarPlayer();
        }
        if (Input.GetKeyDown(KeyCode.P) && SceneManager.GetActiveScene().name == "Asylum")
        {
            if (menuPausa.activeSelf)
            {
                menuPausa.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                menuPausa.SetActive(true);
            }
            menuPausa.SetActive(true);
        }
        if (player == null)
        {
            Debug.LogWarning("El objeto Player no se encontró en la escena.");
        }
        else if (player == null && canvasMuerte != null && SceneManager.GetActiveScene().name != "Asylum")
        {
            canvasMuerte.SetActive(true);
        }
    }

    public void PerderVida()
    {
        vidas -= 1;
        AudioManager.instance.PlaySFX("Daño");
        //player.DesactivarVida(vidas);
    }

    public void RecuperarVida()
    {
        //player.ActivarVida(vidas);
        vidas += 1;
    }

    public void Reanudar()
    {
        Time.timeScale = 1;
        menuPausa.SetActive(false);

    }

    public void ReiniciarPartida()
    {
        menuPausa.SetActive(false);
        canvasMuerte.SetActive(false);
        Time.timeScale = 1;
        vidas = 3;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        BuscarPlayer();
    }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene("Start");
        menuPausa.SetActive(false);
    }
    public void Salir()
    {
        print("cerrando juego...");
        Application.Quit();
    }
    public void BuscarPlayer()
    {
        //para encontrar si o si al player
        if (player == null)
        {
            player = FindObjectOfType<FirstPerson>();

            if (player == null)
            {
                canvasMuerte.SetActive(true);
                Debug.LogWarning("No se encontró ningún objeto de tipo 'Player' en la escena.");
            }
        }
        if (player != null)
        {
            canvasMuerte.SetActive(false);
        }
    }


}