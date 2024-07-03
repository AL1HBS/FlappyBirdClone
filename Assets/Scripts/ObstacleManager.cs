using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{

    private Vector3 obstacleSpawnPoint;
    private Vector3 obstacleExitPoint;

    [SerializeField] private PillarController obstacleTemplate;

    [SerializeField] private float obstacleMoveSpeed;

    [SerializeField] private float obstacleSpawnDistance;

    [SerializeField] private float obstacleMinSize;
    [SerializeField] private float obstacleMaxSize;

    private List<PillarController> obstacleLists;

    private PillarController lastObstacleSpawned;

    public bool isRunning;

    // Start is called before the first frame update
    void Start()
    {
        obstacleLists = new List<PillarController>();

        obstacleExitPoint = Camera.main.ViewportToWorldPoint(new Vector3(-.25f,.5f,1));
        obstacleExitPoint.z = transform.position.z;
        obstacleSpawnPoint = Camera.main.ViewportToWorldPoint(new Vector3(1.25f,.5f,1));
        obstacleSpawnPoint.z = transform.position.z;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isRunning)
            return;

        List<PillarController> obstacleToremove = new List<PillarController>();
        foreach(PillarController pillar in obstacleLists)
        {
            pillar.transform.position += new Vector3(-obstacleMoveSpeed * Time.deltaTime,0,0);
            if(pillar.transform.position.x < obstacleExitPoint.x)
            {
                obstacleToremove.Add(pillar);
            }
        }

        foreach(PillarController pillar in obstacleToremove)
        {
            Destroy(pillar.gameObject);
            obstacleLists.Remove(pillar);
        }

        if(lastObstacleSpawned != null)
        {
            if(obstacleSpawnPoint.x - lastObstacleSpawned.transform.position.x > obstacleSpawnDistance)
            {
                lastObstacleSpawned = SpawnObstacle(
                    Random.Range(-1.0f,1.0f),
                    Random.Range(obstacleMinSize,obstacleMaxSize)
                );  
            }
        }
        else 
        {
            lastObstacleSpawned = SpawnObstacle(
                Random.Range(-1.0f,1.0f),
                Random.Range(obstacleMinSize,obstacleMaxSize)
            );
        }

    }

    private PillarController SpawnObstacle(float gapPos, float gapSize)
    {
        GameObject newGO = Instantiate(obstacleTemplate.gameObject);
        newGO.transform.SetParent(transform, false);
        newGO.transform.position = obstacleSpawnPoint;
        
        PillarController newPillar = newGO.GetComponent<PillarController>();
        newPillar.gapPos = gapPos;
        newPillar.gapSize = gapSize;

        obstacleLists.Add(newPillar);

        return newPillar;
    }

    private void ClearAllObstacle()
    {
        foreach(PillarController obstacle in obstacleLists)
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
