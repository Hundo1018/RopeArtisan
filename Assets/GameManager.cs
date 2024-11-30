using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public RopeSwing ropeSwing;
    public GameObject GGUI;
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
        GGUI.SetActive(true);
    }
}
