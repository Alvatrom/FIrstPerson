using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [SerializeField] private new AudioSource audio;


    void Start()
    {
        audio = GetComponent<AudioSource>();
        Time.timeScale = 1.0f;

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
        yield return new WaitForSeconds(1f);// espera 1 segundo
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
