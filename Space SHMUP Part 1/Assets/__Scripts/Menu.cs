using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    
        public void OnPlayButton ()
    {
        SceneManager.LoadScene("__Scene_0");
    }
    // Called when we click the "Quit" button.
    public void OnQuitButton ()
    {
        Application.Quit();
    }
    //Restart Game
    public void OnRestartButton ()
    {
        SceneManager.LoadScene("__Scene_0");
    }

}

