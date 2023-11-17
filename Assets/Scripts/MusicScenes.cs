using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScenes : MonoBehaviour
{
    private void Awake()
    {
        GameObject musicObj = GameObject.FindGameObjectWithTag("GameMusic");

        if (musicObj != null)
        {

            if (musicObj != this.gameObject)
            {
                Destroy(this.gameObject);
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
