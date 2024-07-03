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

    // Start is called before the first frame update
    void Start()
    {
        StopGame();

        birdScript.OnBirdDead += GameOver;
    }

    private void GameOver()
    {
        StopGame();
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

        isGameStarted = true;
    }

    void StopGame()
    {
        obstacleManager.isRunning = false;
        backGroundLevel.enabled = false;
        floorLevel.enabled = false;
        birdScript.isEnabled = false;
    }
}
