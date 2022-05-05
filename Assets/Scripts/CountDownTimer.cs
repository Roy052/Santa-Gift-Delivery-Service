using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountDownTimer : MonoBehaviour
{
    int hour = 11, minute = 50;
    float second = 0f;

    MainSceneManager msm;
    TutorialSceneManager tsm;

    public Text clock;
    public Sprite christmas;
    private SpriteRenderer spriteRenderer;

    int activeScene = 0;
    // Start is called before the first frame update
    void Start()
    {
        clock.text = "PM" + hour + ":" + minute + ":" + (int)second;
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            activeScene = 1;
            tsm = GameObject.Find("TutorialSceneManager").GetComponent<TutorialSceneManager>();
        }
        else if(SceneManager.GetActiveScene().buildIndex == 2)
        {
            activeScene = 2;
            msm = GameObject.Find("MainSceneManager").GetComponent<MainSceneManager>();
        }
        
        
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(activeScene == 2)
        {
            if (hour < 12)
            {
                second += 1 * Time.deltaTime;
            }

            if (second >= 60.0)
            {
                minute += 1;
                second = 0;
            }

            if (minute >= 60)
            {
                hour += 1;
                minute = 0;
                spriteRenderer.sprite = christmas;
                StartCoroutine(msm.GameEnd());
            }

            if (hour == 12)
            {
                clock.text = "PM " + hour + ":0" + minute + ":0" + (int)second;
            }
            else if (second < 10)
            {
                clock.text = "PM " + hour + ":" + minute + ":0" + (int)second;
            }
            else clock.text = "PM " + hour + ":" + minute + ":" + (int)second;
        }
        
    }
}
