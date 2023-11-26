using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    public AudioSource src;
    public AudioClip sfx1, sfx2, sfx3, sfx4, sfx5, sfx6, sfx7;

    void Start()
    {
        // Comprueba si debes mostrar los cr�ditos o pasar a la escena "Inicio".
        if (SceneManager.GetActiveScene().name == "CreditosIniciales")
        {
            StartCoroutine(ShowCreditsAndChangeScene());
        }

    }

    public void Button1(){
        src.clip = sfx1;
        src.Play();
    }

    public void CambiarEscenaClick(string sceneName)
    {
        Debug.Log("Cambiando Escena: " + sceneName);
        StartCoroutine(retrasoEscena(sceneName));
    }

    public void SalirJuego()
    {
        Debug.Log("Salir");
        Application.Quit();
    }

    public void Continuar() {
        Preguntas preguntas = FindObjectOfType<Preguntas>();
        preguntas.Continuar();
    }

    //Corrutina
    IEnumerator retrasoEscena(string sceneName)
    {
        yield return new WaitForSecondsRealtime(0.3f);
        SceneManager.LoadScene(sceneName);
    }


    IEnumerator ShowCreditsAndChangeScene()
    {
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("Inicio");
    }
}
