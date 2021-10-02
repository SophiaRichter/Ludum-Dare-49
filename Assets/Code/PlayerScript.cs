using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    bool isIdle = true;
    bool isWalking = false;
    bool isWalkingDown = false;
    bool isWalkingUp = false;
    
    float velocity = 2f;

    private Transform blob;

    private Rigidbody2D rigidbody;
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        blob = transform.Find("Blob");
        anim = blob.GetComponent<Animator>(); 

    }

    // Update is called once per frame
    void Update()
    {
        float xDir = Input.GetAxisRaw("Horizontal");
        float yDir = Input.GetAxisRaw("Vertical");
        setDirections(xDir, yDir);

        move(velocity, xDir, yDir);
        animate();

    }

    private void setDirections(float xDir, float yDir)
    {
        if (xDir == 0 && yDir == 0)
        {
            isIdle = true;
            isWalking = false;
            isWalkingDown = false;
            isWalkingUp = false;
            return;
        }
        if (xDir == 0 && yDir < 0)
        {
            isIdle = false;
            isWalkingDown = true;
            isWalking = false;
            isWalkingUp = false;
            return;
        }
        if (xDir == 0 && yDir > 0)
        {
            isIdle = false;
            isWalkingUp = true;
            isWalking = false;
            isWalkingDown = false;
            return;
        }
        if (xDir != 0)
        {
            isIdle = false;
            isWalking = true;
            isWalkingDown = false;
            isWalkingUp = false;
        }
    }

    private void animate()
    {
        //AnimatorClipInfo[] clips = anim.GetCurrentAnimatorClipInfo(0);

        
        //anim.Play("Blob_Idle");
        
        if (isIdle)
        {
            anim.Play("Blob_Idle");
            return;
        }

        if (isWalking)
        {
            anim.Play("Blob_Walking");
            return;
        }

        if (isWalkingDown)
        {
            anim.Play("Blob_WalkingDown");
            return;
        }

        if (isWalkingUp)
        {
            anim.Play("Blob_WalkingUp");
            return;
        }
    }

    private void move(float vel, float xDir, float yDir)
    {
        rigidbody.velocity = new Vector2(vel * xDir, vel * yDir);       

        //let model face walking direction
        if (xDir > 0)
        {
            blob.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        else if (xDir < 0 )
        {
            blob.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }

        if (yDir < 0)
        {
            //blob.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        else if (yDir > 0)
        {
            //blob.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Collider")
        {
        }
    }

    void OnCollisionEnter(Collision other)
    {

    }
}
