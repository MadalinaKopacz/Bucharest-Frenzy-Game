using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    private AudioSource finishSound;

    private void Start()
    {
        finishSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !PlayerScript.isGameOver)
        {
            StartCoroutine(ChangeScene(1f));
        }
    }

    private IEnumerator ChangeScene(float numOfSeconds)
    {
        Time.timeScale = 0;
        finishSound.Play();
        yield return new WaitForSecondsRealtime(numOfSeconds);
        Time.timeScale = 1;

        int playerCoins = GameObject.Find("Player").GetComponent<PlayerScript>().GetGold();
        DataManager.instance.SaveGame(true, SceneManager.GetActiveScene().buildIndex + 1, playerCoins);
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DataManager.instance.LoadGame();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
