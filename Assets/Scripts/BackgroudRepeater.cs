using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.EditorTools;
using UnityEngine;

public class BackgroudRepeater : MonoBehaviour
{

    [SerializeField] private Sprite spriteTest;

    [Tooltip("In Pixel per second")]

    [Range(0.0f, 5.0f)]
    [SerializeField] private float moveSpeed = 1;

    [SerializeField] private bool scaleToFullScreen = false;
    [SerializeField] private bool generateCollider = false;

    [SerializeField] private List<SpriteRenderer> generatedSprites;

    // Reference to the last platform created
    private SpriteRenderer lastSpriteGenerated;

    // Start is called before the first frame update
    void Start()
    {
        if(generatedSprites.Count == 0)
            lastSpriteGenerated = InsertNewPlatform();
        else {
            lastSpriteGenerated = generatedSprites[generatedSprites.Count -1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(generatedSprites == null)
            return;

        if(generatedSprites.Count == 0)
            return;

        MovePlatformByPixel(-moveSpeed);

        //Check if first platform is out of rectangle
        Vector3 rightEdgeOfFirstPlatform = GetSpriteBottomRightPoint(generatedSprites[0]);
        Vector3 bottomLeftPos = Camera.main.ViewportToWorldPoint(new Vector3(0,0,1));

        if(rightEdgeOfFirstPlatform.x < bottomLeftPos.x)
        {
            Destroy(generatedSprites[0].gameObject);
            generatedSprites.Remove(generatedSprites[0]);
        }

        // Fill the rest of rectangle with new platforms
        Vector3 bottomRightScreen = Camera.main.ViewportToWorldPoint(new Vector3(1,0,1));
        if(lastSpriteGenerated != null){
            
            Vector3 lastPlatformRightEdge = GetSpriteBottomRightPoint(lastSpriteGenerated);
        
            if(lastPlatformRightEdge.x < bottomRightScreen.x)
            {
                Vector3 nextPlatformPosition = lastPlatformRightEdge;
                
                lastSpriteGenerated = InsertNewPlatform();
                nextPlatformPosition.x += lastSpriteGenerated.bounds.size.x / 2.0f;
                lastSpriteGenerated.transform.position = new Vector3(nextPlatformPosition.x,0,nextPlatformPosition.z);;
            }
        }

    }

    private void MovePlatformByPixel(float pixel)
    {
        generatedSprites[0].transform.position += new Vector3(pixel, 0) * Time.deltaTime;

        // Place the rest of element following the first element
        if(generatedSprites.Count > 1){
            Vector3 nextPlatformPosition = GetSpriteBottomRightPoint(generatedSprites[0]);
            for(int i=1; i<generatedSprites.Count; i++)
            {   
                Vector2 spriteSize = generatedSprites[i].bounds.size;
                generatedSprites[i].transform.position = 
                    new Vector3(
                        nextPlatformPosition.x + spriteSize.x / 2.0f,
                        transform.position.y,
                        nextPlatformPosition.z);       

                nextPlatformPosition = GetSpriteBottomRightPoint(generatedSprites[i]);
            }    
        }
    }

    private Vector3 GetSpriteBottomRightPoint(SpriteRenderer spriteRenderer)
    {
        Vector2 spriteSize = spriteRenderer.bounds.size;
        return spriteRenderer.transform.position + new Vector3(spriteSize.x / 2.0f, -spriteSize.y / 2.0f, 0);
    }


    // Insert a new sprite to stack
    private SpriteRenderer InsertNewPlatform()
    {
        GameObject newGO = new GameObject(generatedSprites.Count.ToString());
        newGO.transform.SetParent(transform,false);

        SpriteRenderer sr = newGO.AddComponent<SpriteRenderer>();
        sr.sprite = spriteTest;
    
        if(scaleToFullScreen)
            ScaleSpriteToFullScreen(sr);

        if(generateCollider)
            newGO.AddComponent<BoxCollider2D>();

        generatedSprites.Add(sr);

        return sr;
    }

    private void ScaleSpriteToFullScreen(SpriteRenderer sr)
    {
        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;
        
        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        sr.transform.localScale = 
            new Vector3(worldScreenHeight / height, worldScreenHeight / height, 1);
    }
}
