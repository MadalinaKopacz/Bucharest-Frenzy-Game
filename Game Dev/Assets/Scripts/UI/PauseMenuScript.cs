using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{

    private static bool isPaused = false;

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject shopMenuUI;
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                resumeGame();
            }
            else
            {
                pauseGame();
            }
        }
    }

    public void resumeGame()
    {
        pauseMenuUI.SetActive(false);
        shopMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void pauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void loadMainMenuScene()
    {
        resumeGame();
        SceneManager.LoadScene("Assets/Scenes/MainMenuScene.unity");
    }

    public void loadShopMenu()
    {
        pauseMenuUI.SetActive(false);
        shopMenuUI.SetActive(true);
    }

    public void quitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
