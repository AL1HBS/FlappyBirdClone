using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{

    [SerializeField] private Transform obstacleSpawnPoint;
    [SerializeField] private Transform obstacleExitPoint;

    [SerializeField] private PillarController obstacleTemplate;

    [SerializeField] private float obstacleMoveSpeed;

    [SerializeField] private float obstacleSpawnDistance;

    [SerializeField] private float obstacleMinSize;
    [SerializeField] private float obstacleMaxSize;

    private List<PillarController> obstacleLists;

    private PillarController lastObstacleSpawned;

    // Start is called before the first frame update
    void Start()
    {
        obstacleLists = new List<PillarController>();

        lastObstacleSpawned = SpawnObstacle(
                Random.Range(-1.0f,1.0f),
                Random.Range(obstacleMinSize,obstacleMaxSize)
            );
    }

    // Update is called once per frame
    void Update()
    {
        List<PillarController> obstacleToremove = new List<PillarController>();
        foreach(PillarController pillar in obstacleLists)
        {
            pillar.transform.position += new Vector3(-obstacleMoveSpeed * Time.deltaTime,0,0);
            if(pillar.transform.position.x < obstacleExitPoint.position.x)
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
            if(obstacleSpawnPoint.position.x - lastObstacleSpawned.transform.position.x > obstacleSpawnDistance)
            {
                lastObstacleSpawned = SpawnObstacle(
                    Random.Range(-1.0f,1.0f),
                    Random.Range(obstacleMinSize,obstacleMaxSize)
                );  
            }
        }

    }

    private PillarController SpawnObstacle(float gapPos, float gapSize)
    {
        GameObject newGO = Instantiate(obstacleTemplate.gameObject);
        newGO.transform.SetParent(transform, false);
        newGO.transform.position = obstacleSpawnPoint.position;
        PillarController newPillar = newGO.GetComponent<PillarController>();
        newPillar.gapPos = gapPos;
        newPillar.gapSize = gapSize;

        obstacleLists.Add(newPillar);

        return newPillar;
    }


}
