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
    public GameObject panelRespuesta;

    public bool respuestaValida;

    public float tiempoRestante = 20f;
    public TextMeshProUGUI tiempoText;
    private bool tiempoAgotado = false;

    public Dificultad[] bancoPreguntas;
    public TextMeshProUGUI enunciado;
    public TextMeshProUGUI[] respuestas;
    public TextMeshProUGUI solucion;
    public int nivelPregunta;
    public Pregunta preguntaActual;

    public Button[] btnRespuesta;
    public Sprite spriteCorrecto;
    public Sprite spriteIncorrecto;
    public Sprite spriteNormal;

    private bool isInside = false; // Variable para controlar si el jugador est� dentro del objeto planeta
    private PlayerController playerController;
    private bool detenerContador = false;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
        cargarBancoPreguntas();
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInside)
        {
            if (!tiempoAgotado)
            {
                if (!detenerContador)
                {
                    tiempoRestante -= Time.deltaTime; // Reducir el tiempo restante
                    tiempoText.text = Mathf.CeilToInt(tiempoRestante).ToString(); // Actualizar el texto del tiempo

                    if (tiempoRestante <= 5)
                    {
                        tiempoText.color = Color.red;

                        if (tiempoRestante <= 0)
                        {
                            tiempoAgotado = true;
                            tiempoText.text = "0";
                            panelRespuesta.SetActive(true);
                            Debug.Log("SetActive(true)");
                        }
                    }
                }
            }
            StartCoroutine(retrasoPregunta());
            animator.SetBool("isLeaving", true);
            //Debug.Log("Jugador dentro del planeta");
        }
        else
        {
            panelPregunta.SetActive(false);
            tiempoRestante = 20f; // Reiniciar el tiempo cuando el jugador sale del planeta
            tiempoAgotado = false;
            tiempoText.text = "20"; // Actualizar el texto del tiempo
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

        solucion.text = preguntaActual.solucion;
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
            panelRespuesta.SetActive(false);
            animator.SetBool("spaceship", true);
            isInside = false;
            foreach (Button btnRespuesta in btnRespuesta)
            {
                Image image = btnRespuesta.GetComponent<Image>();
                image.sprite = spriteNormal;
            }
            Debug.Log("Jugador saliendo del planeta");
            playerController.Stop();
        }
    }

    public void evaluarPregunta(int respuestaJugador)
    {
        
        if (respuestaJugador == preguntaActual.respuestaCorrecta)
        {
            Debug.Log("respuesta correcta");
            CambiarColorBoton(btnRespuesta[respuestaJugador], spriteCorrecto);
            StartCoroutine(TiempoAnimacion());
            StartCoroutine(retrasoMovimientoCorrecto());
        }
        else
        {
            Debug.Log("respuesta mala");
            CambiarColorBoton(btnRespuesta[respuestaJugador], spriteIncorrecto);
            StartCoroutine(retrasoMovimientoIncorrecto());
        }
    }

    private void CambiarColorBoton(Button boton, Sprite nuevoSprite)
    {
        // Obtener el componente Image del botón.
        Image image = boton.GetComponent<Image>();
        if (image != null)
        {
            // Cambiar el sprite del botón.
            image.sprite = nuevoSprite;
        }
    }

    public void Continuar() 
    {
        Debug.Log("Le dio click a continuar");
        playerController.SetRespondioIncorrectamente(true);
    }

    public void DetenerContador()
    {
        detenerContador = true;
    }

    IEnumerator retrasoMovimientoCorrecto()
    {
        yield return new WaitForSecondsRealtime(2f);
        tiempoRestante = 20f;
        playerController.Move();
    }

    IEnumerator TiempoAnimacion()
    {
        yield return new WaitForSecondsRealtime(1f);
        isInside = false;
        animator.SetBool("isLeaving", true);

    }

    IEnumerator retrasoMovimientoIncorrecto()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        panelRespuesta.SetActive(true);
    }

    IEnumerator retrasoPregunta()
    {
        yield return new WaitForSecondsRealtime(1f);
        if (isInside)
        {
            animator.SetBool("spaceship", false);
            panelPregunta.SetActive(true);
        }
        
    }
}