using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Bird : MonoBehaviour
{

    [SerializeField] Animator birdAnimator;
    [SerializeField] Rigidbody2D birdRigidBody;

    private Collider2D boxCollider;

    private float targetRotation = 0;

    [SerializeField] private float jumpelocity = 200;

    [SerializeField] private float jumpAngle = 25;
    [SerializeField] private float fallAngle = -20;

    [SerializeField] AudioSource wingsSFX;

    private bool _isEnabled  = false;

    public bool isEnabled { 
        get { return _isEnabled; } 
        set {            
            if( _isEnabled != value ) {
                if(value)
                {
                    birdRigidBody.bodyType = RigidbodyType2D.Dynamic;
                }
                else {
                    birdRigidBody.velocity = Vector2.zero;
                    birdRigidBody.bodyType = RigidbodyType2D.Static;
                }

                _isEnabled = value; 
            }            
        }
    }

    public Action OnBirdDead;
    public Action OnDeadAnimationFinished;

    private void Awake()
    {
        boxCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        
        Vector3 horizontalPos = Camera.main.ViewportToWorldPoint(new Vector3(.25f,.5f,1));
        transform.position = new Vector3(horizontalPos.x, transform.position.y, transform.position.z);

        targetRotation = birdRigidBody.velocity.y > 0 ? jumpAngle : fallAngle;
        transform.localEulerAngles = new Vector3(0,0,Mathf.LerpAngle(transform.localEulerAngles.z,targetRotation,0.2f));
        
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

        wingsSFX?.Play();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.collider.tag == "Pillar")
        {
            OnBirdDead?.Invoke();
            isEnabled = false;
            StartCoroutine(AnimateGameOver());
        }

        
            
        //Debug.Log($"OnCollisionEnter2D {col.collider.name}");
    }

    public void Reset()
    {
        // boxCollider.enabled = true;
        // birdRigidBody.simulated = false;
    }

    private IEnumerator AnimateGameOver()
    {
        birdAnimator.speed = 0;
        yield return new WaitForSeconds(1);
        birdAnimator.speed = 1;

        boxCollider.enabled = false;

        birdRigidBody.bodyType = RigidbodyType2D.Dynamic;
        FlyUp();

        yield return new WaitUntil( () => transform.position.y < -3);
        Debug.Log("GameOver");
        OnDeadAnimationFinished?.Invoke();
    }
}
