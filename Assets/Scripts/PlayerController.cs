using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento del astronauta.
    private int currentPlanetIndex = 0; // �ndice del planeta actual.
    private int previousPlanetIndex = 0; // �ndice del planeta anterior

    public GameObject[] planets; // Un arreglo para almacenar todos los planetas en la escena.
    public GameObject perdida;
    public GameObject victoria;
    public Image[] corazones;
    public Sprite corazonVacio;
    public Preguntas ultimoPlaneta;
    public Animator animator;
    public float puntos = 0;
    public TextMeshProUGUI puntosText;
    public bool quedanVidas = true;

    private bool isMoving = false;
    private float journeyLength;
    private float startTime;
    private Transform destination;
    private bool respuesta;
    public bool respondioIncorrectamente = false; // Para controlar si el jugador respondi� incorrectamente.
    private int preguntasIncorrectas = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {

        if (respuesta && !isMoving)
        {
            // Verifica que todav�a hay planetas disponibles para avanzar.
            if (currentPlanetIndex < planets.Length - 1)
            {
                //animator.SetBool("isLeaving", true);

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
                victoria.SetActive(true);
                ultimoPlaneta.DetenerContador();
            }
        }
        else if (respondioIncorrectamente && !isMoving)
        {

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
                respondioIncorrectamente = false;

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
            // Coloca al jugador en la posici�n del objeto "Sol".
            Transform solTransform = GameObject.Find("Sol").transform;
            destination = solTransform;

            // Calcula la longitud del viaje y guarda el tiempo de inicio.
            journeyLength = Vector3.Distance(transform.position, destination.position);
            startTime = Time.time;

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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Sol"))
        {
            perdida.SetActive(true);
        }
    }

}

