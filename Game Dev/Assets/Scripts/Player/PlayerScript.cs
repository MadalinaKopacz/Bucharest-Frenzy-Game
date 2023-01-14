using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private GameObject healthBar;
    private HealthBarScript healthScript;

    [SerializeField] private GameObject currency;
    private CurrencyScript currencyScript;

    [SerializeField] private GameObject gameOver;

    [SerializeField] private int hp = 100;
    [SerializeField] private int gold = 10;
    [SerializeField] public int damagePerHit;

    private bool isHit;
    private float timeSinceLastHit;
    public bool Inverted { get; set; }

    private AudioSource soundPlayer1;
    private AudioSource soundPlayer2;
    public AudioClip shootSound;
    public AudioClip easter;
    public AudioClip hurtSound;
    public AudioClip downgradeSound;
    public AudioClip upgradeSound;

    private bool isTakingDamage = false;
    private int enemyDamage = 0;
    private void Start()
    {
        healthScript = healthBar.GetComponent<HealthBarScript>();
        isHit = false;
        timeSinceLastHit = Time.time;
        currencyScript = currency.GetComponent<CurrencyScript>();
        damagePerHit = 10;
        Inverted = false;
        soundPlayer1 = GetComponents<AudioSource>()[0];
        soundPlayer2 = GetComponents<AudioSource>()[1];
    }

    void Update()
    {
        if (Time.time >= timeSinceLastHit + 0.8f)
        {
            isHit = false;
            timeSinceLastHit = Time.time;
        }
        if (!isHit && isTakingDamage)
        {
            takeDamage(enemyDamage);
        }
        currencyScript.setCurrency(gold);
    }

    public void CheckGameOver()
    {
        if (hp <= 0)
        {
            gameOver.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
            gameOver.SetActive(false);
        }
    }

    public void playSound(AudioClip clip, float volume=0.02f)
    {
        if (!soundPlayer1.isPlaying)
        {
            soundPlayer1.volume = volume;
            soundPlayer1.clip = clip;
            soundPlayer1.Play();
        } else if (!soundPlayer2.isPlaying) {
            soundPlayer2.volume = volume;
            soundPlayer2.clip = clip;
            soundPlayer2.Play();
        }
    }

    private void takeDamage(int damage)
    {
        hp -= damage;
        isHit = true;
        playSound(hurtSound, 0.02f);
        healthScript.setHealth();
        CheckGameOver();
    }

    private IEnumerator OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rat"))
        {
            isTakingDamage = true;
            enemyDamage = 20;
        }
        int damageBird = 15;
        if (collision.gameObject.CompareTag("caca"))
        {
            Destroy(collision.gameObject);

            hp -= damageBird;
            isHit = true;
            playSound(hurtSound, 0.02f);
            healthScript.setHealth();
            
            CheckGameOver();
        }

        if (collision.gameObject.CompareTag("Dog"))
        {
            isTakingDamage = true;
            enemyDamage = 25;
        }

        if (collision.gameObject.CompareTag("Coin"))
        {
            playSound(collision.gameObject.GetComponent<AudioSource>().clip, 0.02f);
            Destroy(collision.gameObject);
            gold++;
            currencyScript.setCurrency(gold);
        }

        if (collision.gameObject.CompareTag("Mushroom"))
        {
            Destroy(collision.gameObject);
            Inverted = true;
            yield return new WaitForSeconds(5);
            Inverted = false;

        }

        if (collision.gameObject.CompareTag("Powerup"))
        {
            // Activate powerup and destroy object
            UsePowerup(collision.gameObject);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Downgrade"))
        {
            // Activate powerup and destroy object
            UsePowerup(collision.gameObject, true);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("KillingSlider"))
        {
            hp = 0;
            CheckGameOver();
        }
    }

    public void UsePowerup(GameObject powerup, bool isDownGrade = false)
    {
        if (!isDownGrade) {
            playSound(upgradeSound);
        } else {
            playSound(downgradeSound);
        }
        // Hp is added without being removed later
        PowerupHp(powerup, isDownGrade);
        int restoreDamagePerHit = PowerupDamage(powerup, isDownGrade);
        float restoreSpeed = PowerupSpeed(powerup, isDownGrade);
        float restoreJump = PowerupJump(powerup, isDownGrade);

        float duration = powerup.GetComponent<Powerup>().getPowerupDuration();

        // When powerup duration expires restore values back
        if (duration > 0)
        {
            StartCoroutine(RestoreValuesPowerup(duration, restoreDamagePerHit, 
                restoreSpeed, restoreJump));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        var obstaclesList = new List<string> { "Rat", "Dog" };

        if (obstaclesList.Contains(collision.gameObject.tag))
        {
            isTakingDamage = false;
        }
    }

    IEnumerator RestoreValuesPowerup(float delayTime, int oldDamagePerHit, 
        float oldSpeed, float oldJumpSize)
    {
        //Wait for the specified delay time before continuing.
        yield return new WaitForSeconds(delayTime);
        
        // Restore values
        damagePerHit = oldDamagePerHit;
        GetComponent<PlayerMovement>().setSpeed(oldSpeed);
        GetComponent<PlayerMovement>().setJumpSize(oldJumpSize);
    }

    private void PowerupHp(GameObject powerup, bool isDownGrade = false)
    {
        // Get hp bonus and add it without overflowing healthbar
        int addHp = powerup.GetComponent<Powerup>().getHpAddition();
        if (addHp > 0)
        {
            if (!isDownGrade)
                SetHp(Mathf.Min(100, addHp + hp));
            else
                SetHp(Mathf.Max(0, hp - addHp));
        }
        healthScript.setHealth();
    }

    private int PowerupDamage(GameObject powerup, bool isDownGrade = false)
    {
        // Get damage bonus from powerup and add it
        // return the old value 
        int addDamage = powerup.GetComponent<Powerup>().getDamageAddition();
        if (addDamage > 0)
        {
            int beforePowerup = damagePerHit;
            if (!isDownGrade)
                damagePerHit += addDamage;
            else 
                damagePerHit -= addDamage;

            return beforePowerup;
        }
        return damagePerHit;
    }

    private float PowerupSpeed(GameObject powerup, bool isDownGrade = false)
    {
        // Get speed bonus from powerup and add it
        // return the old value 
        float addSpeed = powerup.GetComponent<Powerup>().getSpeedAddition();
        float beforePowerup = GetComponent<PlayerMovement>().getSpeed();
        if (addSpeed > 0)
        {
            if (!isDownGrade)
                GetComponent<PlayerMovement>().setSpeed(beforePowerup + addSpeed);
            else
                GetComponent<PlayerMovement>().setSpeed(beforePowerup - addSpeed);
        }
        return beforePowerup;
    }

    private float PowerupJump(GameObject powerup, bool isDownGrade = false)
    {
        // Get jump size bonus from powerup and add it
        // return the old value 
        float addJumpSize = powerup.GetComponent<Powerup>().getJumpAddition();
        float beforePowerup = GetComponent<PlayerMovement>().getJumpSize();
        if (addJumpSize > 0)
        {
            if(!isDownGrade)
                GetComponent<PlayerMovement>().setJumpSize(beforePowerup + addJumpSize);
            else
                GetComponent<PlayerMovement>().setJumpSize(beforePowerup - addJumpSize);
        }
        return beforePowerup;
    }

    public int GetHp()
    {
        return hp;
    }

    public void SetHp(int newHp)
    {
        hp = newHp;
    }

    public int GetGold()
    {
        return gold;
    }

    public void SetGold(int newGold)
    {
        gold = newGold;
    }

    public int getDamagePerHit()
    {
        return damagePerHit;
    }

    public void setDamagePerHit(int newDamagePerHit)
    {
        damagePerHit = newDamagePerHit;
    }
}
