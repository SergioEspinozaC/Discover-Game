using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EmpezarJuego : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(empezarJuego());
    }

    IEnumerator empezarJuego()
    {
        yield return new WaitForSecondsRealtime(21f);
        SceneManager.LoadScene("Juego");
    }
}
