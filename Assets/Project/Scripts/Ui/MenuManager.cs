using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject pauseMenuScreen;
    public GameObject settingsMenuScreen;
    [SerializeField] public Image crossHair;
    [SerializeField] public Button pauseBtn;

    public Slider musicSlider;
    public Slider soundSlider;
    public Slider voiceSlider;
    

    void Start()
    {
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        soundSlider.onValueChanged.AddListener(OnSoundSliderChanged);
        voiceSlider.onValueChanged.AddListener(OnVoiceSliderChanged);
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

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
        settingsMenuScreen.SetActive(true);
        pauseMenuScreen.SetActive(false);
    }

    public void QuitToMainMenuBtn()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        SceneManager.LoadScene("Title screen");

    }

    public void RestartGameBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #region Setting Buttons & Sliders

    public void BackBtn()
    {
        settingsMenuScreen.SetActive(false);
        pauseMenuScreen.SetActive(true);
    }

    public void ControlsBtn()
    {
        Debug.Log("Controller Controls will appear here");
    }
    
    public void VideoBtn()
    {
        Debug.Log("Video Settings will appear here");
    }
    
    public void AudioBtn()
    {
        Debug.Log("Audio Settings will appear here");
    }
    
    public void GnomeBtn()
    {
        Debug.Log("Voices button clicked");
    }
    
    public void SpecularAudioBtn()
    {
        Debug.Log("Audio Button clicked");
    }

    void OnMusicSliderChanged(float value)
    {
        Debug.Log("Music slider moved! Value: " + value);
    }
    
    void OnSoundSliderChanged(float value)
    {
        Debug.Log("Sound slider moved! Value: " + value);
    }

    void OnVoiceSliderChanged(float value)
    {
        Debug.Log("Voice slider moved! Value: " + value);
    }
    
    #endregion
}