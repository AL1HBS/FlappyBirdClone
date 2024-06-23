using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;

public class BackgroudRepeater : MonoBehaviour
{

    [SerializeField] private Sprite spriteTest;

    [Tooltip("In Pixel per second")]
    [SerializeField] private int moveSpeed = 300;

    
    private List<Image> platforms;
    
    private RectTransform rectTransform;


    // Reference to the last platform created
    private Image lastPlatform;
    

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if(rectTransform == null)
        {
            Debug.LogError("This script need recttransform component");
            return;
        }

        platforms = new List<Image>();

    }

    // Start is called before the first frame update
    void Start()
    {
        lastPlatform = InsertNewPlatform(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(platforms == null || rectTransform == null)
            return;


        if(platforms.Count == 0)
            return;

        MovePlatformByPixel(-moveSpeed);

        // Check if first platform is out of rectangle
        float rightEdgeOfFirstPlatform = GetRightEdgeFromRect(platforms[0].rectTransform);

        if(rightEdgeOfFirstPlatform < 0)
        {
            Destroy(platforms[0].gameObject);
            platforms.Remove(platforms[0]);
        }

        // Fill the rest of rectangle with new platforms
        float screenWidth = rectTransform.rect.width;
        if(lastPlatform != null){
            float nextPlatformPosition = GetRightEdgeFromRect(lastPlatform.rectTransform);
        
            if(nextPlatformPosition < screenWidth)
            {
                lastPlatform = InsertNewPlatform(nextPlatformPosition);
            }
        }

    }

    private void MovePlatformByPixel(float pixel)
    {
        platforms[0].rectTransform.anchoredPosition += new Vector2(pixel, 0) * Time.deltaTime;

        // Place the rest of element following the first element
        if(platforms.Count > 1){
            float nextPlatformPosition = GetRightEdgeFromRect(platforms[0].rectTransform);
            for(int i=1; i<platforms.Count; i++)
            {
                platforms[i].rectTransform.anchoredPosition = new Vector2(nextPlatformPosition, 0);
                nextPlatformPosition = GetRightEdgeFromRect(platforms[i].rectTransform);
            }    
        }
    }

    private float GetRightEdgeFromRect(RectTransform rectTransform)
    {
        return rectTransform.anchoredPosition.x + rectTransform.rect.width;
    }


    // Insert a new platform on a position
    private Image InsertNewPlatform(float position)
    {
        Image createdImage = CreateNewPlatform(spriteTest, platforms.Count.ToString());
        createdImage.rectTransform.anchoredPosition = new Vector2(position, 0);
        platforms.Add(createdImage);

        return createdImage;
    }

    // Create new platform / image using a Sprite
    private Image CreateNewPlatform(Sprite sprite, string objectName  = "NewPlatform")
    {
        GameObject newGO = new GameObject(objectName);
        newGO.transform.SetParent(transform,false);

        Image imgCp = newGO.AddComponent<Image>();
        imgCp.sprite = sprite;
        imgCp.SetNativeSize();

        imgCp.rectTransform.pivot = new Vector2(0.0f, 0.0f);
        imgCp.rectTransform.anchorMax = new Vector2(0.0f, 0.0f);
        imgCp.rectTransform.anchorMin = new Vector2(0.0f, 0.0f);
        
        return imgCp;

    }
}
