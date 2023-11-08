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

    private bool isInside = false; // Variable para controlar si el jugador esta dentro del objeto planeta
    private PlayerController playerController;
    private bool detenerContador = false;
    private bool respuestaCorrecta;
    private bool respuestaIncorrecta;

    public bool continuar = false;

    public Animator animator;

    private List<Pregunta> preguntasUsadas = new List<Pregunta>();
    private int preguntaActualIndex = 0;

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
            if (!tiempoAgotado)
            {
                if (!detenerContador)
                {
                    tiempoRestante -= Time.deltaTime; // Reducir el tiempo restante
                    tiempoText.text = Mathf.CeilToInt(tiempoRestante).ToString(); // Actualizar el texto del tiempo

                    if (tiempoRestante <= 5)
                    {
                        tiempoText.color = Color.red;

                        if (tiempoRestante <= 0 && !continuar)
                        {
                            tiempoAgotado = true;
                            tiempoText.text = "0";
                            panelRespuesta.SetActive(true);
                        }
                    }
                }
            }
            StartCoroutine(retrasoPregunta());
            //animator.SetBool("isLeaving", true);
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
        if (preguntasUsadas.Count == bancoPreguntas[nivelPregunta].preguntas.Length)
        {
            preguntasUsadas.Clear();
        }

        int preguntaRandom;
        do
        {
            preguntaRandom = Random.Range(0, bancoPreguntas[nivelPregunta].preguntas.Length);
        } while (preguntasUsadas.Contains(bancoPreguntas[nivelPregunta].preguntas[preguntaRandom]));

        preguntasUsadas.Add(bancoPreguntas[nivelPregunta].preguntas[preguntaRandom]);
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

            if (playerController.QuedanVidas())
            {
                setPregunta();

                if (playerController.respuestaIncorrecta())
                {
                    animator.SetBool("spaceship", false);
                    animator.SetBool("arriving", true);
                    StartCoroutine(AnimacionLlegada());
                }
                else
                {
                    animator.SetBool("spaceship2", false);
                    animator.SetBool("newPlanet", true);
                    StartCoroutine(AnimacionLlegada2());
                }
                
                isInside = true;
            }
            
            //animator.SetTrigger("idleTrigger");
            Debug.Log("Jugador entrando al planeta");
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            //panelRespuesta.SetActive(false);

            if (respuestaIncorrecta)
            {
                animator.SetBool("alien", false);
                animator.SetBool("spaceship", true);
            }

            if (respuestaCorrecta)
            {
                animator.SetBool("humanSpaceship", false);
                animator.SetBool("spaceship2", true);
            }
            
            isInside = false;
            continuar = false;
            foreach (Button btnRespuesta in btnRespuesta)
            {
                Image image = btnRespuesta.GetComponent<Image>();
                image.sprite = spriteNormal;
            }
            Debug.Log("Jugador saliendo del planeta");
            playerController.Stop();
            respuestaCorrecta = false;
            respuestaIncorrecta = false;
        }
    }

    public void evaluarPregunta(int respuestaJugador)
    {
        
        if (respuestaJugador == preguntaActual.respuestaCorrecta)
        {
            Debug.Log("respuesta correcta");
            respuestaCorrecta = true;
            CambiarColorBoton(btnRespuesta[respuestaJugador], spriteCorrecto);
            StartCoroutine(TiempoAnimacionAvance());
            StartCoroutine(retrasoMovimientoCorrecto());
        }
        else
        {
            Debug.Log("respuesta mala");
            respuestaIncorrecta = true;
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
        continuar = true;
        playerController.PreguntasIncorrectas();
        panelRespuesta.SetActive(false);
        StartCoroutine(TiempoAnimacionAlien());
        StartCoroutine(volver());
    }

    public void DetenerContador()
    {
        detenerContador = true;
    }

    IEnumerator volver()
    {
        yield return new WaitForSecondsRealtime(2f);
        playerController.SetRespondioIncorrectamente(true);
    }

    IEnumerator retrasoMovimientoCorrecto()
    {
        yield return new WaitForSecondsRealtime(1.8f);
        tiempoRestante = 20f;
        playerController.Move();
    }

    IEnumerator TiempoAnimacionAlien()
    {
        yield return new WaitForSecondsRealtime(1f);
        isInside = false;
        animator.SetBool("idle", false);
        animator.SetBool("alien", true);
    }

    IEnumerator TiempoAnimacionAvance()
    {
        yield return new WaitForSecondsRealtime(1f);
        isInside = false;
        animator.SetBool("idle", false);
        animator.SetBool("humanSpaceship", true);
    }

    IEnumerator AnimacionLlegada()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        animator.SetBool("arriving", false);
        animator.SetBool("idle", true);
    }

    IEnumerator AnimacionLlegada2()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        animator.SetBool("newPlanet", false);
        animator.SetBool("idle", true);
    }

    IEnumerator retrasoMovimientoIncorrecto()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        //panelPregunta.SetActive(false);
        isInside = false;
        if (!continuar)
        {
            panelRespuesta.SetActive(true);
        }
    }

    IEnumerator retrasoPregunta()
    {
        yield return new WaitForSecondsRealtime(3f);
        if (isInside)
        {
            panelPregunta.SetActive(true);
        }
        
    }
}