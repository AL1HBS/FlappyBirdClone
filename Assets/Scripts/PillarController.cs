using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class PillarController : MonoBehaviour
{

    [SerializeField] private SpriteRenderer topPillar;
    [SerializeField] private SpriteRenderer bottomPillar;

    public float gapSize;
    private float prev_gapSize;
    public float gapPos;
    private float prev_gapPos;
    
    private Rect gapRect;

    [SerializeField] private BoxCollider2D scoreTrigger;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(prev_gapSize != gapSize || prev_gapPos != gapPos)
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

            prev_gapSize = gapSize;
            prev_gapPos = gapPos;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        Debug.Log("Score!!!");
    }
}
