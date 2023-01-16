using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private PlayerScript playerStats;

    public void Start()
    {
        playerStats = player.GetComponent<PlayerScript>();
    }

    public void resetGame()
    {

        Time.timeScale = 1f;
        if(player.CompareTag("Player"))
            resetStats();
        SceneManager.LoadScene(0);

    }

    private void resetStats()
    {
        playerStats.SetHp(100);
    }
}
