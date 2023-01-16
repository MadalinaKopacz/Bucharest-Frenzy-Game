using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IDataManager
{
    // UI
    [SerializeField] private GameObject healthBar;
    private HealthBarScript healthScript;

    [SerializeField] private GameObject currency;
    private CurrencyScript currencyScript;

    [SerializeField] private GameObject gameOver;

    // Player stats
    [SerializeField] private int hp = 100;
    [SerializeField] private int gold = 10;
    [SerializeField] public int damagePerHit;

    // Powerup stats helper
    [SerializeField] public int restoreDamagePerHit;
    [SerializeField] public float restoreSpeed;
    [SerializeField] public float restoreJump;
    [SerializeField] public float timeLeft;
    [SerializeField] public bool inPowerup;


    // Sound
    private AudioSource soundPlayer1;
    private AudioSource soundPlayer2;
    public AudioClip shootSound;
    public AudioClip easter;
    public AudioClip hurtSound;
    public AudioClip downgradeSound;
    public AudioClip upgradeSound;

    // Helpers
    private bool isHit;
    private bool godMode = false;
    private float timeSinceLastHit;
    public bool Inverted { get; set; }
    public static bool isGameOver = false;
    private bool isTakingDamage = false;
    private int enemyDamage = 0;
    
    public void LoadData(GameData data)
    {
        Start();
        healthScript.Start();
        transform.position = data.playerData.position;
        this.hp = data.playerData.hp;
        this.gold = data.playerData.gold;
        this.damagePerHit = data.playerData.damagePerHit;
        
        // PlayerMovement
        gameObject.GetComponent<PlayerMovement>().setSpeed(data.playerData.speed);
        gameObject.GetComponent<PlayerMovement>().setJumpSize(data.playerData.jumpSize);

        // Player powerup
        this.inPowerup =  data.playerData.inPowerup;
        this.restoreDamagePerHit = data.playerData.restoreDamagePerHit;
        this.restoreSpeed = data.playerData.restoreSpeed;
        this.restoreJump = data.playerData.restoreJump;
        this.timeLeft = data.playerData.timeLeft;

        healthScript.Start();
        healthScript.setHealth();
        currencyScript.setCurrency(gold);

        if (inPowerup)
        {
            // Player was using a powerup when game was saved, make sure
            // to restore the values
            StartCoroutine(RestoreValuesPowerup(timeLeft, restoreDamagePerHit, 
                restoreSpeed, restoreJump));
        }
    }

    public void SaveData(ref GameData data)
    {
        data.playerData.position = gameObject.transform.position;
        data.playerData.hp = this.hp;
        data.playerData.gold = this.gold;
        data.playerData.damagePerHit = this.damagePerHit;

        // Player movement
        data.playerData.speed = gameObject.GetComponent<PlayerMovement>().getSpeed();
        data.playerData.jumpSize = gameObject.GetComponent<PlayerMovement>().getJumpSize();
        
        // Player powerup
        data.playerData.inPowerup = this.inPowerup;
        data.playerData.restoreDamagePerHit = this.restoreDamagePerHit;
        data.playerData.restoreSpeed = this.restoreSpeed;
        data.playerData.restoreJump = this.restoreJump;
        data.playerData.timeLeft = this.timeLeft;
    }

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
        if (Time.time == 0)
        {
            PauseSound();
        } 
        if (Time.time == 1)
        {
            UnpauseSound();
        }

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

        if (Input.GetKeyDown(KeyCode.K))
        {
            godMode = !godMode;
            SetHp(100);
            Debug.Log("god");
        }
    }

    public void CheckGameOver()
    {
        if (hp <= 0)
        {
            isGameOver = true;
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

    public void PauseSound()
    {
        if (soundPlayer1.isPlaying)
        {
            soundPlayer1.Pause();
        } 

        if (soundPlayer2.isPlaying) {
            soundPlayer2.Pause();
        } 

        if (GameObject.Find("BackgroundSoundPlayer") != null )
        {
            AudioSource bck = GameObject.Find("BackgroundSoundPlayer").GetComponent<AudioSource>();
            if (bck.isPlaying) 
            {
                bck.Pause();
            }
        }
    }

    public void UnpauseSound()
    {
        if (!soundPlayer1.isPlaying)
        {
            soundPlayer1.Play();
        } 

        if (!soundPlayer2.isPlaying) 
        {
            soundPlayer2.Play();
        } 

        if (GameObject.Find("BackgroundSoundPlayer")  != null )
        {
            AudioSource bck = GameObject.Find("BackgroundSoundPlayer").GetComponent<AudioSource>();
            if (!bck.isPlaying) 
            {
                bck.Play();
            }
        }
    }

    private void takeDamage(int damage)
    {
        if(!godMode)
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
            if (!isHit)
            {
                if (!godMode)
                    hp -= enemyDamage;
                isHit = true;
                playSound(hurtSound, 0.02f);
                healthScript.setHealth();
            }
            CheckGameOver();
        }
        int damageBird = 15;
        if (collision.gameObject.CompareTag("caca"))
        {
            Destroy(collision.gameObject);  
            if(!godMode)
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
            if (!isHit)
            {
                if (!godMode)
                    hp -= enemyDamage;
                isHit = true;
                healthScript.setHealth();
            }
            CheckGameOver();
        }

        if (collision.gameObject.CompareTag("GruzMother"))
        {
            isTakingDamage = true;
            enemyDamage = 5;
            if (!isHit)
            {
                if (!godMode)
                    hp -= enemyDamage;
                isHit = true;
                healthScript.setHealth();
            }
            CheckGameOver();
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
            yield return new WaitForSeconds(7);
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

        inPowerup = true;
        int _restoreDamagePerHit = PowerupDamage(powerup, isDownGrade);
        float _restoreSpeed = PowerupSpeed(powerup, isDownGrade);
        float _restoreJump = PowerupJump(powerup, isDownGrade);

        float duration = powerup.GetComponent<Powerup>().getPowerupDuration();

        if (restoreDamagePerHit == 0 && duration > 0)
        {
            restoreDamagePerHit = _restoreDamagePerHit;
            restoreSpeed = _restoreSpeed;
            restoreJump = _restoreJump;
        }

        timeLeft = duration;

        // When powerup duration expires restore values back
        if (duration > 0)
        {
            StartCoroutine(RestoreValuesPowerup(duration, _restoreDamagePerHit, 
                _restoreSpeed, _restoreJump));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        var obstaclesList = new List<string> { "Rat", "Dog" , "GruzMother"};

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

        restoreDamagePerHit = 0;
        restoreSpeed = 0;
        restoreJump = 0;
        timeLeft = 0;
        inPowerup = false;
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
        if(godMode == true)
        {
            hp = 100;
            return;
        }

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
