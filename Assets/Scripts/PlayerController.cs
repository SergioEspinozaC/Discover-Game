using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento del astronauta.
    private int currentPlanetIndex = 0; // Índice del planeta actual.

    public GameObject[] planets; // Un arreglo para almacenar todos los planetas en la escena.
    private bool isMoving = false;
    private float journeyLength;
    private float startTime;
    private Transform destination;
    private Preguntas pregunta;
    private bool respuesta;

    private void Start()
    {
        //planets = GameObject.FindGameObjectsWithTag("Planet");
    }

    public void Move()
    {
        respuesta = true;
    }

    public void Stop()
    {
        respuesta = false;
    }

    private void Update()
    {
        // Verifica si se presionó la tecla "T" y si no estamos ya en movimiento.
        if (respuesta && !isMoving)
        {
            // Verifica que todavía hay planetas disponibles para avanzar.
            if (currentPlanetIndex < planets.Length - 1)
            {
                // Calcula la longitud del viaje y guarda el tiempo de inicio.
                destination = planets[currentPlanetIndex + 1].transform;
                journeyLength = Vector3.Distance(transform.position, destination.position);
                startTime = Time.time;

                // Marca que estamos en movimiento.
                isMoving = true;
            }
        }

        // Realiza el movimiento si isMoving es igual a true.
        if (isMoving)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            // Calcula la dirección y la magnitud del movimiento.
            Vector3 direction = (destination.position - transform.position).normalized;
            float step = moveSpeed * Time.deltaTime;

            // Mueve al astronauta usando transform.Translate.
            transform.Translate(direction * step);

            // Cuando llegamos al destino, detén el movimiento y actualiza el índice del planeta.
            if (fractionOfJourney >= 1.0f)
            {
                isMoving = false;
                currentPlanetIndex++;
            }
        }
    }
}

