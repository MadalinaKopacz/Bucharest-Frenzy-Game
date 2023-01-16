using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    [SerializeField] private Slider slider;
    
    [SerializeField] private GameObject healthTextObject;
    private Text healthText;

    [SerializeField] private GameObject player;
    private PlayerScript playerStats;


    public void Start()
    {
        healthText = healthTextObject.GetComponent<Text>();
        playerStats = player.GetComponent<PlayerScript>();
        setMaxHealth();
    }

    public void setMaxHealth()
    {
        slider.maxValue = 100;
        slider.value = playerStats.GetHp();
    }

    public void setHealth()
    {
        healthText.text = playerStats.GetHp() + "/100"; 
        slider.value = playerStats.GetHp();
    }
}
