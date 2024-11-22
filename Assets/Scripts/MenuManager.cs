using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void ReloadGame()
    {
        // load the current scene again
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        // this currently points back to the main menu
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
