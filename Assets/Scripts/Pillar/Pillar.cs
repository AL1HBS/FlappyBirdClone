using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{

    [SerializeField] private SpriteRenderer topPillar;
    [SerializeField] private SpriteRenderer bottomPillar;

    public float gapSize;
    private float prev_gapSize;
    public float gapPos;
    private float prev_gapPos;
    
    private Rect gapRect;

    [SerializeField] private BoxCollider2D scoreTrigger;

    public Action<int> onBirdPassed;

    void Start()
    {
        Init();
    }

    protected virtual void Init()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ObstacleUpdateBehacior();

        if(prev_gapSize != gapSize || prev_gapPos != gapPos)
        {
            UpdateGap(); // ABSTRACTION

            prev_gapSize = gapSize;
            prev_gapPos = gapPos;
        }
    }

    protected virtual void ObstacleUpdateBehacior()
    {

    }

    // ABSTRACTION
    private void UpdateGap()
    {
        gapRect = new Rect(
            new Vector2(0,gapPos), 
            new Vector2(topPillar.bounds.size.x,gapSize));

        scoreTrigger.size = gapRect.size;
        scoreTrigger.offset = gapRect.position;

        float topOffset = 1 * gapSize / 2.0f;
        float bottomOffset = -1 * gapSize / 2.0f;
        topPillar.transform.localPosition = new Vector2(0,gapPos + topOffset);
        bottomPillar.transform.localPosition = new Vector2(0,gapPos + bottomOffset);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        onBirdPassed?.Invoke(1);
    }

}
