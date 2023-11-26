using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento del astronauta.
    private int currentPlanetIndex = 0; // indice del planeta actual.
    private int previousPlanetIndex = 0; // indice del planeta anterior
    
    public GameObject[] planets; // Un arreglo para almacenar todos los planetas en la escena.
    public GameObject perdida;
    public GameObject victoria;
    public GameObject fuegoRacha;
    public GameObject corazon;
    public Image[] corazones;
    public Sprite corazonVacio;
    public Sprite corazonLleno;
    public Preguntas ultimoPlaneta;
    public Animator animator;
    public int puntos = 0;
    public TextMeshProUGUI puntosText;
    public int racha = 0;
    public TextMeshProUGUI rachaText;
    public bool quedanVidas = true;

    private bool isMoving = false;
    private float journeyLength;
    private float startTime;
    private Transform destination;
    private bool respuesta;
    public bool respondioIncorrectamente = false; // Para controlar si el jugador respondio incorrectamente.
    private int preguntasIncorrectas = 0;

    public Registro registro;
    public AudioSource gameover;
    public AudioSource win;
    private void Start()
    {
        animator = GetComponent<Animator>();
        fuegoRacha.SetActive(false);
    }

    private void Update()
    {

        if (respuesta && !isMoving)
        {
            // Verifica que todavia hay planetas disponibles para avanzar.
            if (currentPlanetIndex < planets.Length - 1)
            {
                //animator.SetBool("isLeaving", true);
                Debug.Log("Preguntas incorrectas: " + preguntasIncorrectas);
                if (preguntasIncorrectas > 0)
                {
                    racha++;
                    fuegoRacha.SetActive(true);
                    if (racha == 3)
                    {
                        fuegoRacha.SetActive(false);
                        preguntasIncorrectas--;
                        StartCoroutine(mostrarCorazon());
                        corazones[preguntasIncorrectas].sprite = corazonLleno;
                        racha = 0;
                    }
                }
                rachaText.text = Mathf.CeilToInt(racha).ToString();
                // Calcula la longitud del viaje y guarda el tiempo de inicio.
                destination = planets[currentPlanetIndex + 1].transform;
                journeyLength = Vector3.Distance(transform.position, destination.position);
                startTime = Time.time;
                puntos += 1;
                puntosText.text = Mathf.CeilToInt(puntos).ToString();

                // Marca que estamos en movimiento.
                isMoving = true;

                // Actualiza el �ndice del planeta.
                currentPlanetIndex++;
            }
            else
            {
                puntos += 1;
                puntosText.text = Mathf.CeilToInt(puntos).ToString();
                win.Play();
                victoria.SetActive(true);
                ultimoPlaneta.DetenerContador();
                respuesta = false;
            }
        }
        else if (respondioIncorrectamente && !isMoving)
        {

            racha = 0;
            rachaText.text = Mathf.CeilToInt(racha).ToString();
            if (preguntasIncorrectas == 3)
            {
                quedanVidas = false;
                currentPlanetIndex = 0;
                Morir();
            }

            if (currentPlanetIndex > 0) // Verifica que haya un planeta anterior para retroceder.
            {

                // Calcula la longitud del viaje de regreso al planeta anterior.
                destination = planets[currentPlanetIndex - 1].transform;
                journeyLength = Vector3.Distance(transform.position, destination.position);
                startTime = Time.time;

                // Marca que estamos en movimiento.
                isMoving = true;

                // Restablece la variable respondioIncorrectamente.
                //respondioIncorrectamente = false;

                // Retrocede al planeta anterior.
                currentPlanetIndex--;
                puntos -= 1;
                puntosText.text = Mathf.CeilToInt(puntos).ToString();

                //preguntasIncorrectas++;

                corazones[preguntasIncorrectas - 1].sprite = corazonVacio;

                Debug.Log(preguntasIncorrectas);

            }
            else
            {
                for (int i = 0; i <= 2; i++)
                {
                    corazones[i].sprite = corazonVacio;
                }

                Morir();
            }
        }

        // Realiza el movimiento si isMoving es igual a true.
        if (isMoving)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            // Calcula la direcci�n y la magnitud del movimiento.
            Vector3 direction = (destination.position - transform.position).normalized;
            float step = moveSpeed * Time.deltaTime;

            // Mueve al astronauta usando transform.Translate.
            transform.Translate(direction * step);

            // Cuando llegamos al destino, det�n el movimiento y actualiza el �ndice del planeta.
            if (fractionOfJourney >= 1.0f)
            {
                //animator.SetBool("isLeaving", false);
                isMoving = false;
                //currentPlanetIndex++;
            }
        }


    }

    public void PreguntasIncorrectas()
    {
        preguntasIncorrectas++;
    }

    public void Move()
    {
        respuesta = true;
    }

    public void Stop()
    {
        respuesta = false;
    }

    public void SetRespondioIncorrectamente(bool value)
    {
        respondioIncorrectamente = value;
    }

    public void Morir()
    {
        if (!isMoving)
        {
            // Coloca al jugador en la posicion del objeto "Sol".
            Transform solTransform = GameObject.Find("Sol").transform;
            destination = solTransform;

            // Calcula la longitud del viaje y guarda el tiempo de inicio.
            journeyLength = Vector3.Distance(transform.position, destination.position);
            startTime = Time.time;

            moveSpeed = 15f;

            // Marca que estamos en movimiento.
            isMoving = true;
        }

        //perdida.SetActive(true);
    }

    public bool QuedanVidas()
    {
        return quedanVidas;
    }

    public bool respuestaIncorrecta()
    {
        return respondioIncorrectamente;
    }

    public void registrarPuntaje()
    {
        string puntaje = puntos.ToString();
        registro.RegistrarPuntaje(puntaje);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Sol"))
        {
            gameover.Play();
            StartCoroutine(detenerSonido(5));
            perdida.SetActive(true);
        }
    }

    IEnumerator mostrarCorazon()
    {
        corazon.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        corazon.SetActive(false);
    }
    IEnumerator detenerSonido(float tiempo)
    {
        yield return new WaitForSecondsRealtime(tiempo);
        gameover.Stop();
    }
}

