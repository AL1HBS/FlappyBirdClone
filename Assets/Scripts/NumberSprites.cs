using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberSprites : MonoBehaviour
{
    private short _value;
    public short value {
        get { return _value; }
        set { 
            _value = value; 

            numberOfDigit = GetNumberOfDigit(value);
            GenerateSprites(numberOfDigit);

            int[] digitValues = GetEachDigitValue(_value);
            for (int i = 0; i<numberOfDigit; i++)
            {
                generatedSpritesDigit[i].sprite = numberSprites[digitValues[i]];
            }

        }
    }

    private short numberOfDigit;

    private float totalWidth = 0;

    [SerializeField] private Sprite[] numberSprites;

    [SerializeField] private SpriteRenderer spriteTemplate;

    private List<SpriteRenderer> generatedSpritesDigit;

    // Start is called before the first frame update
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Alpha1)) value = 1;
        if(Input.GetKeyUp(KeyCode.Alpha2)) value = 123;
        if(Input.GetKeyUp(KeyCode.Alpha3)) value = 13456;
        
    }

    private short GetNumberOfDigit(short value)
    {
        if(value <= 0) return 1;
        short n = 0;
        while (value != 0) {
            ++n;
            value /= 10;
        }
        return n;
    }

    private int[] GetEachDigitValue(short value)
    {
        int _numberOfDigit = GetNumberOfDigit(value);
        int[] digitValues = new int[_numberOfDigit];

        short score = value;
        int index = _numberOfDigit;
        while(score > 0)
        {
            index--;
            digitValues[index] = score % 10;
            score /= 10;
            
        }


        return digitValues;
    }

    private void GenerateSprites(int numOfDigits)
    {
        if(generatedSpritesDigit == null)
            generatedSpritesDigit = new List<SpriteRenderer>();

        foreach(SpriteRenderer sprite in generatedSpritesDigit) {
            Destroy(sprite.gameObject);
        }
        
        generatedSpritesDigit.Clear();

        totalWidth = 0;

        for(int i=0; i<numOfDigits; ++i)
        {
            GameObject newDigit_go = Instantiate(spriteTemplate.gameObject, Vector3.zero, Quaternion.identity, this.transform);
            newDigit_go.SetActive(true);
            SpriteRenderer sr = newDigit_go.GetComponent<SpriteRenderer>();
            generatedSpritesDigit.Add(sr);

            float width = sr.bounds.size.x;
            totalWidth += width;
            
        }

        //Debug.Log(totalWidth);

        RearrangeSprites();
        
    }

    private void RearrangeSprites()
    {
        float startPos = -totalWidth / 2.0f;
        foreach(SpriteRenderer sprite in generatedSpritesDigit) {
            float width = sprite.bounds.size.x;
            sprite.gameObject.transform.localPosition = new Vector3(startPos + width/2.0f,0,0);
            startPos += sprite.bounds.size.x;
        }

    }

    
}
