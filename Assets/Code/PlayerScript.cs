using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public float jumpHeight = 5f;
    float velocity = 0.15f;
    bool onGround = true;
    bool doubleJump = true;
    private float levelStart;

    private Transform blob;

    CharacterController charController;

    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
        levelStart = Time.time;
        blob = transform.Find("blob");
    }

    // Update is called once per frame
    void Update()
    {
        float xDir = Input.GetAxisRaw("Horizontal");
        float yDir = Input.GetAxisRaw("Vertical");
        move(velocity, xDir, yDir);

    }

    private void move(float vel, float xDir, float yDir)
    {
        charController.Move(new Vector2(vel * xDir, vel * yDir));
        
        //let model face walking direction
        if (xDir > 0)
        {
            //transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        else if (xDir < 0 )
        {
            //transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        
        if (other.gameObject.tag == "Collider")
        {
            onGround = true;
        }
    }
}
