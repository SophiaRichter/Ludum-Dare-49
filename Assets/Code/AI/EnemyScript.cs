using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public int startingDirection; //0-360 
    private Animator anim;
    public LayerMask target;
    public LayerMask obstacle;
    public float fieldOfViewAngle;
    private float velocity = 0.5f;
    private Transform herbert;
    private Rigidbody2D rigi;
    private int posMultiplier;
    private int negMultiplier;
    private Vector2 rotation;
    private int currentDirection;

    void Start()
    {
        
        herbert = transform.Find("HeSprite");
        anim = herbert.GetComponent<Animator>();
        rigi = GetComponentInParent<Rigidbody2D>();

        rigi.velocity = rotation * velocity;
    }

    void Update()
    {
        Collider2D targetinRadius = Physics2D.OverlapCircle(transform.position, 3f, target);
        
        if (targetinRadius != null)
        {
            Vector2 dirToTarget = (targetinRadius.transform.position - transform.position).normalized;
            //Debug.Log(dirToTarget + " " + rotation);
            float angle = Vector2.SignedAngle(transform.position, dirToTarget);



            //animate(rotation);
            //remeber last rotation
            //if (rigi.velocity.x != 0 && rigi.velocity.y != 0) rotation = rigi.velocity;
        }
    }

    private void animate(Vector2 rotation)
    {
        //let model face walking direction
        if (rotation.x > 0 || rigi.velocity.x > 0)
        {
            herbert.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        else if (rotation.x < 0 || rigi.velocity.x < 0)
        {
            herbert.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }

        AnimatorClipInfo[] clips = anim.GetCurrentAnimatorClipInfo(0);
        //if (clips.Length > 0 && (clips[0].clip.name.Equals("Enemy_Devour")) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) return;
       
        if (rigi.velocity.x == 0 && rigi.velocity.y == 0) anim.Play("Enemy_Idle");
        if (Mathf.Abs(rigi.velocity.x) < 1 && rigi.velocity.y < 0) anim.Play("Enemy_WalkingUp");
        if (Mathf.Abs(rigi.velocity.x) < 1 && rigi.velocity.y > 0) anim.Play("Enemy_Walking");
        else if (rigi.velocity.y < -0.1f) anim.Play("Enemy_WalkingDown");

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}