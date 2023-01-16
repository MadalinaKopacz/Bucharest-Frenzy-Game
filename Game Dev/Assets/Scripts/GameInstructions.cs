using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInstructions : MonoBehaviour
{
    [SerializeField] private GameObject i1;
    [SerializeField] private GameObject i2;
    [SerializeField] private GameObject i3;
    [SerializeField] private GameObject i4;
    [SerializeField] private GameObject i5;
    [SerializeField] private GameObject i6;
    private static bool isPaused = true;
    public static bool finishTutorial = false;

    void Update()
    {   
        if (!finishTutorial)
        {
            if (isPaused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
        
    }
    public void i1i2()
    {
        i1.SetActive(false);
        i2.SetActive(true);
    }

    public void i2i3()
    {
        i2.SetActive(false);
        i3.SetActive(true);
    }

    public void i3i4()
    {
        i3.SetActive(false);
        i4.SetActive(true);
    }

    public void i4i5()
    {
        i4.SetActive(false);
        i5.SetActive(true);
    }

    public void i5i6()
    {
        i5.SetActive(false);
        i6.SetActive(true);
    }

    public void startGame()
    {
        i6.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
        finishTutorial = true;
    }
}
