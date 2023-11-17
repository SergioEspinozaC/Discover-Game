using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Registro : MonoBehaviour
{
    public InputField nombre;
    public InputField edad;
    public InputField email;

    public Button registrar;

    public void LlamadoRegistro()
    {
        StartCoroutine(Register());
    }

    public void RegistrarPuntaje(string puntaje)
    {
        StartCoroutine(RegisterPuntaje(puntaje));
    }

    IEnumerator Register()
    {
        //string url = "http://localhost/connection/conexionDB.php";

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("nombre_completo", nombre.text));
        formData.Add(new MultipartFormDataSection("edad", edad.text));
        formData.Add(new MultipartFormDataSection("email", email.text));
        
        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/connection/conexionDB.php", formData))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("User created successfully");
                SceneManager.LoadScene("Juego");
            }
            else
            {
                Debug.LogError("User creation failed. Error #" + www.result + ", Response Code: " + www.responseCode + ", Response: " + www.downloadHandler.text);

                // Additional error handling
                if (www.isNetworkError)
                {
                    Debug.LogError("Network error: " + www.error);
                }
                else if (www.isHttpError)
                {
                    Debug.LogError("HTTP error: " + www.error);
                }
                else
                {
                    Debug.LogError("Unknown error");
                }
            }
        }
    }

    IEnumerator RegisterPuntaje(string puntaje)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("puntaje", puntaje));

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/connection/registroPuntuacion.php", formData))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Puntaje registrado exitosamente");
            }
            else
            {
                Debug.LogError("Error al registrar el puntaje. Error #" + www.result + ", Response Code: " + www.responseCode + ", Response: " + www.downloadHandler.text);

                // Manejar errores adicionales si es necesario
            }
            www.Dispose();
        }

    }

    public void VerifyInputs()
    {
        registrar.interactable = (edad.text.Length <= 2 && nombre.text.Length >= 4 && email.text.Length >= 8);
    }
}
