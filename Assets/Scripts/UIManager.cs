using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static public UIManager instance;

    public GameObject player;
    public Text timeText;
    public Text gameOver;
    public Button retry;
    public bool isGameOver;


    private float time;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        timeText.text = "Time:     ";
        ResetState();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isGameOver)
        {
            time += Time.fixedDeltaTime;
            timeText.text = "Time:" + StringSpacePadding(((int)time).ToString(), 5);
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                Retry();
            }
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOver.gameObject.SetActive(true);
        retry.gameObject.SetActive(true);
    }

    public void Retry()
    {
        player.GetComponent<FirstPersonController>().Retry();
        ResetState();
    }

    private void ResetState()
    {
        isGameOver = false;
        gameOver.gameObject.SetActive(false);
        retry.gameObject.SetActive(false);
        time = 0;
    }

    private string StringSpacePadding(string str, int len)
    {
        if (str.Length >= len) return str;
        for(int i=0; i<len - str.Length; ++i)
        {
            str = " " + str;
        }
        return str;
    }
}
