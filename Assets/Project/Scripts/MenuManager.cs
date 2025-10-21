using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject pauseMenuScreen;
    [SerializeField] private Image crossHair;
    [SerializeField] private Button pauseBtn;
    

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenuScreen.SetActive(true);
        crossHair.enabled = false;
        pauseBtn.gameObject.SetActive(false);
    }

    public void ResumeGameBtn()
    {
        Time.timeScale = 1;
        pauseMenuScreen.SetActive(false);
        crossHair.enabled = true;
        pauseBtn.gameObject.SetActive(true); 
    }
    
    public void OptionBtn()
    {
        Debug.Log("Option Menu Coming SOON!-");
    }

    public void QuitBtn()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void RestartGameBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}