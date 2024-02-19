using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public void Home()
    {
        //SceneManager.LoadSceneAsync("Main Menu");
        SceneController.instance.LoadScene("Main Menu");
        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneController.instance.RestartLevel();
        Time.timeScale = 1;
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }

    public void NextLevelButton()
    {
        //go to next level
        SceneController.instance.NextLevel();
    }
}
