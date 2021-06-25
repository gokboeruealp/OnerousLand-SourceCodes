using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void newGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void exitGame()
    {
        Application.Quit();
    }
    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
