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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
