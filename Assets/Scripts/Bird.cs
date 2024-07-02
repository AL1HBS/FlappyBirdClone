using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{

    [SerializeField] Animator birdAnimator;
    [SerializeField] Rigidbody2D birdRigidBody;

    [SerializeField] RectTransform rectTransform;

    private float targetRotation = 0;

    [SerializeField] private float jumpelocity = 200;

    [SerializeField] private float jumpAngle = 25;
    [SerializeField] private float fallAngle = -20;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            FlyUp();
        }

        targetRotation = birdRigidBody.velocity.y > 0 ? jumpAngle : fallAngle;
        rectTransform.localEulerAngles = new Vector3(0,0,Mathf.LerpAngle(rectTransform.localEulerAngles.z,targetRotation,0.1f));
    }

    private void FlyUp()
    {
        if (birdAnimator != null)
        {
            birdAnimator.Play("FlyUp");
        }

        if (birdRigidBody != null)
        {
            birdRigidBody.velocity = new Vector2(0,jumpelocity);
        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.collider.tag == "Pillar")
            Time.timeScale = 0;

            
        Debug.Log($"OnCollisionEnter2D {col.collider.name}");
    }
}
