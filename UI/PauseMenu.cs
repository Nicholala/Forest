using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject OptionMenu;
    public GameObject HelpMenu;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }

    public void Option()
    {
        pauseMenu.SetActive(false);
        OptionMenu.SetActive(true);
    }

    public void OptionBack()
    {
        pauseMenu.SetActive(true);
        OptionMenu.SetActive(false);
    }

    public void Help()
    {
        HelpMenu.SetActive(true);
        OptionMenu.SetActive(false);
    }

    public void HelpBack()
    {
        HelpMenu.SetActive(false);
        OptionMenu.SetActive(true);
    }
}
