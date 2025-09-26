using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject pauseMenuScreen;
    [SerializeField] private Image crossHair;

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenuScreen.SetActive(true);
        crossHair.enabled = false;
    }

    public void ResumeGameBtn()
    {
        Time.timeScale = 1;
        pauseMenuScreen.SetActive(false);
        crossHair.enabled = true;
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
}