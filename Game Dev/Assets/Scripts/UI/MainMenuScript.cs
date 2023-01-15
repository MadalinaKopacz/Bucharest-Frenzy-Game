using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void quitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
