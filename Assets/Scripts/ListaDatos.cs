using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class ListaDatos : MonoBehaviour
{
    public string phpURL = "http://localhost/connection/getData.php";
    public TextMeshProUGUI[] textMeshArray;
    void Start()
    {
        StartCoroutine(GetDataFromPHP());
    }

    IEnumerator GetDataFromPHP()
    {
        UnityWebRequest www = UnityWebRequest.Get(phpURL);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string[] data = www.downloadHandler.text.Split(','); // Separa los datos por comas
            for (int i = 0; i < textMeshArray.Length && i < data.Length; i++)
            {
                textMeshArray[i].text = data[i].Trim(); // Trim para eliminar posibles espacios en blanco
            }
        }
        else
        {
            Debug.LogError("Error al obtener datos: " + www.error);
        }
    }
}
