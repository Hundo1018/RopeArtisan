using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public RopeSwing ropeSwing;
    public GameObject GGUI;
    public TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        ropeSwing.GameOver += OnGameOver;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGameOver(int score)
    {
        scoreText.text = "GG" + Environment.NewLine + "Score: " + score;
        GGUI.SetActive(true);
    }

    public void RestartGame()
    {
        GGUI.SetActive(false);
        SceneManager.LoadScene(0);
    }
}
