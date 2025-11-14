using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuplaybutton : MonoBehaviour
{
    private MenuManager menuManager;
    public void PlayGame()
    {
        SceneManager.LoadScene("Magic the delivery");
    }

    public void MainMenuOptions()
    {
        menuManager.OptionBtn();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

