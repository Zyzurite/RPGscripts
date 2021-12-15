using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.FindGameObjectWithTag("Player") == null)
            SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
