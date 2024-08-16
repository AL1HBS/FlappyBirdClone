using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{

    private Vector3 obstacleSpawnPoint;
    private Vector3 obstacleExitPoint;

    [SerializeField] private Pillar[] obstacleTemplate;

    [SerializeField] private float obstacleMoveSpeed;

    [SerializeField] private float obstacleSpawnDistance;

    [SerializeField] private float obstacleMinSize;
    [SerializeField] private float obstacleMaxSize;

    private List<Pillar> obstacleLists;

    private Pillar lastObstacleSpawned;

    private bool _isRunning = false;
    
    public bool isRunning {  // ENCAPSULATION
        get { return _isRunning; }
        set { 
            if(obstacleLists != null) {
                foreach(Pillar pillar in obstacleLists)
                {
                    pillar.enabled = value;
                }
            }
            _isRunning = value; 
        }
    }

    public Action<int> onBirdPassed;

    // Start is called before the first frame update
    void Start()
    {
        obstacleLists = new List<Pillar>();

        obstacleExitPoint = Camera.main.ViewportToWorldPoint(new Vector3(-.25f,.5f,1));
        obstacleExitPoint.z = transform.position.z;
        obstacleSpawnPoint = Camera.main.ViewportToWorldPoint(new Vector3(1.25f,.5f,1));
        obstacleSpawnPoint.z = transform.position.z;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!_isRunning)
            return;

        List<Pillar> obstacleToremove = new List<Pillar>();
        foreach(Pillar pillar in obstacleLists)
        {
            pillar.transform.position += new Vector3(-obstacleMoveSpeed * Time.deltaTime,0,0);
            if(pillar.transform.position.x < obstacleExitPoint.x)
            {
                obstacleToremove.Add(pillar);
            }
        }

        foreach(Pillar pillar in obstacleToremove)
        {
            Destroy(pillar.gameObject);
            obstacleLists.Remove(pillar);
        }

        if(lastObstacleSpawned != null)
        {
            if(obstacleSpawnPoint.x - lastObstacleSpawned.transform.position.x > obstacleSpawnDistance)
            {
                lastObstacleSpawned = SpawnObstacle(
                    UnityEngine.Random.Range(-1.0f,1.0f),
                    UnityEngine.Random.Range(obstacleMinSize,obstacleMaxSize)
                );  
            }
        }
        else 
        {
            lastObstacleSpawned = SpawnObstacle(
                UnityEngine.Random.Range(-1.0f,1.0f),
                UnityEngine.Random.Range(obstacleMinSize,obstacleMaxSize)
            );
        }

    }

    private Pillar SpawnObstacle(float gapPos, float gapSize)
    {
        int obstacleIndex = UnityEngine.Random.Range(0, obstacleTemplate.Length);

        GameObject newGO = Instantiate(obstacleTemplate[obstacleIndex].gameObject);
        newGO.transform.SetParent(transform, false);
        newGO.transform.position = obstacleSpawnPoint;
        
        Pillar newPillar = newGO.GetComponent<Pillar>();
        newPillar.gapPos = gapPos;
        newPillar.gapSize = gapSize;
        newPillar.onBirdPassed = onBirdPassed;

        obstacleLists.Add(newPillar);

        return newPillar;
    }

    private void ClearAllObstacle()
    {
        foreach(Pillar obstacle in obstacleLists)
        {
            Destroy(obstacle.gameObject);
        }

        obstacleLists.Clear();
    }

    public void Reset()
    {
        ClearAllObstacle();
    }


}
