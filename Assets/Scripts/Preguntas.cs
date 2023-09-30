using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class Preguntas : MonoBehaviour
{

    public GameObject panelPregunta;
    

    public bool respuestaValida;

    public Dificultad[] bancoPreguntas;
    public TextMeshProUGUI enunciado;
    public TextMeshProUGUI[] respuestas;
    public int nivelPregunta;
    public Pregunta preguntaActual;

    public Button[] btnRespuesta;

    private bool isInside = false; // Variable para controlar si el jugador está dentro del objeto planeta
    private PlayerController playerController;
    

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
        cargarBancoPreguntas();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isInside)
        {
            StartCoroutine(retrasoPregunta());
            //Debug.Log("Jugador dentro del planeta");
        }
        else
        {
            panelPregunta.SetActive(false);
            //Debug.Log("Jugador fuera del planeta");
        }
    }

    public void setPregunta()
    {
        int preguntaRandom = Random.Range(0, bancoPreguntas[nivelPregunta].preguntas.Length);
        preguntaActual = bancoPreguntas[nivelPregunta].preguntas[preguntaRandom];
        enunciado.text = preguntaActual.enunciado;

        for (int i = 0; i < respuestas.Length; i++)
        {
            respuestas[i].text = preguntaActual.respuestas[i].texto;
        }
    }

    public void cargarBancoPreguntas()
    {
        try
        {
            bancoPreguntas =
                JsonConvert
                    .DeserializeObject<Dificultad[]>(File
                        .ReadAllText(Application.streamingAssetsPath +
                        "/QuestionBank.json"));
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
            enunciado.text = ex.Message;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            isInside = true;
            setPregunta();
            Debug.Log("Jugador entrando al planeta");
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            isInside = false;
            Debug.Log("Jugador saliendo del planeta");
            playerController.Stop();
        }
    }

    public void evaluarPregunta(int respuestaJugador)
    {
        if (respuestaJugador == preguntaActual.respuestaCorrecta)
        {
            //respuestaValida = true;
            Debug.Log("respuesta correcta");
            playerController.Move();
 

            /*if (nivelPregunta == bancoPreguntas.Length)
            {
                SceneManager.LoadScene("Creditos");
            }*/

        }
        else
        {
            Debug.Log("respuesta mala");
            //devolverse una casilla
            playerController.SetRespondioIncorrectamente(true);
        }
    }

    IEnumerator retrasoPregunta()
    {
        yield return new WaitForSecondsRealtime(1f);
        if (isInside)
        {
            panelPregunta.SetActive(true);
        }
        
    }

}
