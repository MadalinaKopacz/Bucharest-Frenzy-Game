using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class KillingSlider : MonoBehaviour
{
    [SerializeField] private float movingSpeed = 0.03f;
    [SerializeField] private float maxIdleTime = 7; // 7 seconds
    [SerializeField] private float maxLevelTime = 2 * 60; // 2 mins
    [SerializeField] private GameObject player;
    private Vector3 end;
    private Vector3 start;
    private float startTime;
    private float startIdle;
    private bool isIdle;
    private Animator animator;
    private AudioSource sound;

    private void Start() 
    {
        end = new Vector3(10000, gameObject.transform.position.y, gameObject.transform.position.z);
        startTime = Time.time;
        animator = GameObject.Find("Player").GetComponent<Animator>();
        isIdle = false;
        sound = GetComponent<AudioSource>();
    }


    private bool checkIdle()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle Animation") || 
            animator.GetCurrentAnimatorStateInfo(0).IsName("IdleAiming"))
        {
            return true;
        }
        
        return false;
    }


    void Update() 
    {
        //  If slider is in view nothing can stop it 😈
        Vector3 viewPos = Camera.main.WorldToViewportPoint(gameObject.transform.position);
        if (!(viewPos.x > -0.15 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0))
        {
            float currentTime = Time.time;

            if (!checkIdle())
            {
                // Player moved
                isIdle = false; 
            }
            else if (checkIdle() && !isIdle)
            {
                // Player entered idle state
                isIdle = true;
                startIdle = currentTime;
            }
            
            // In case slider enters camera view, don't stop.
            if (isIdle)
            {
                if (currentTime - startIdle >= maxIdleTime)
                {
                    if (!sound.isPlaying) {
                        sound.Play();
                    }
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x, player.transform.position.y, gameObject.transform.position.z);
                    end = new Vector3(end.x, player.transform.position.y, end.z);
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, end, movingSpeed);
                }
            }


            if (currentTime - startTime >= maxLevelTime)
            {
                if (!sound.isPlaying) {
                    sound.Play();
                }
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, player.transform.position.y, gameObject.transform.position.z);
                end = new Vector3(end.x, player.transform.position.y, end.z);
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, end, movingSpeed);
            }

        }
        else {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, end, movingSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            sound.Stop();
        } 
    }
}
