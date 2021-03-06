using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject Enter;
    public bool isEnterActive;
    public int SceneOffset;
    public GameObject Main;
    public bool isMainActive;
    public GameObject AboutUs;
    public bool isAboutUsActive; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isAboutUsActive)
        {
            Application.Quit();
        }
        if (Input.anyKey && isEnterActive)
        {
            isEnterActive = false;
            isMainActive = true;
            Main.SetActive(true);
            Enter.SetActive(false);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1+SceneOffset);
    }

    public void SelectLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void About()
    {
        AboutUs.SetActive(true);
        isAboutUsActive = true;
        Main.SetActive(false);
        isMainActive = false;
    }

    public void AboutBack()
    {
        Main.SetActive(true);
        isMainActive = true;
        AboutUs.SetActive(false);
        isMainActive = false;
    }
}
