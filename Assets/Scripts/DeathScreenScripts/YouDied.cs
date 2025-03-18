using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouDied : MonoBehaviour
{
    public void Respawn() {
        SceneManager.LoadScene("firstarea");
    }
    public void TitleScreen() {
        SceneManager.LoadScene("Title Screen");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
