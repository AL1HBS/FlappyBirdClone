using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Bird : MonoBehaviour
{

    [SerializeField] Animator birdAnimator;
    [SerializeField] Rigidbody2D birdRigidBody;

    private float targetRotation = 0;

    [SerializeField] private float jumpelocity = 200;

    [SerializeField] private float jumpAngle = 25;
    [SerializeField] private float fallAngle = -20;

    public bool isEnabled = false;

    public Action OnBirdDead;

    // Update is called once per frame
    void Update()
    {

        
        Vector3 horizontalPos = Camera.main.ViewportToWorldPoint(new Vector3(.25f,.5f,1));
        transform.position = new Vector3(horizontalPos.x, transform.position.y, transform.position.z);

        birdRigidBody.isKinematic = !isEnabled;

        if(!isEnabled){
            birdRigidBody.velocity = Vector2.zero;
            return;
        }

        targetRotation = birdRigidBody.velocity.y > 0 ? jumpAngle : fallAngle;
        transform.localEulerAngles = new Vector3(0,0,Mathf.LerpAngle(transform.localEulerAngles.z,targetRotation,0.1f));
        
    }

    public void FlyUp()
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
        {
            OnBirdDead?.Invoke();
        }
            
        //Debug.Log($"OnCollisionEnter2D {col.collider.name}");
    }
}
