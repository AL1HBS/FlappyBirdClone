using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    private bool isGameStarted = false;

    [SerializeField] private ObstacleManager obstacleManager;
    [SerializeField] private BackgroudRepeater floorLevel;
    [SerializeField] private BackgroudRepeater backGroundLevel;

    [SerializeField] private Bird birdScript;

    [SerializeField] private NumberSprites scoreUI;

    [SerializeField] AudioClip scoreSFX;
    [SerializeField] AudioClip gameOverSFX;

    [SerializeField] AudioSource audioSrc;

    private int totalScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        StopGame();

        birdScript.OnBirdDead += GameOver;
        obstacleManager.onBirdPassed += OnBirdPassObstacle;

        scoreUI.value = 0;
    }

    private void GameOver()
    {
        StopGame();
        
        audioSrc.clip = gameOverSFX;
        audioSrc.Play();
        
        isGameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!isGameStarted){
                obstacleManager.Reset();
                StartGame();
            }

            birdScript.FlyUp();
            
        }
        
    }


    void StartGame()
    {
        obstacleManager.isRunning = true;
        backGroundLevel.enabled = true;
        floorLevel.enabled = true;
        birdScript.isEnabled = true;
        birdScript.Reset();

        totalScore = 0; 
        scoreUI.value = (short)totalScore;

        isGameStarted = true;
        
    }

    void StopGame()
    {
        obstacleManager.isRunning = false;
        backGroundLevel.enabled = false;
        floorLevel.enabled = false;
        birdScript.isEnabled = false;
    }

    void OnBirdPassObstacle(int score)
    {
        if(isGameStarted){
            totalScore += score;
            audioSrc.clip = scoreSFX;
            audioSrc.Play();
            scoreUI.value = (short)totalScore;
        }
    }
}
