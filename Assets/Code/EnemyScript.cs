using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private Animator anim;
    public LayerMask target;
    public LayerMask obstacle;
    public float fieldOfViewAngle;
    private float velocity = 0.5f;
    private Transform enemy;
    private Rigidbody2D rigi;
    private int posMultiplier;
    private int negMultiplier;
    // Start is called before the first frame update
    void Start()
    {
        enemy = transform.Find("EnSprite");
        anim = enemy.GetComponent<Animator>();
        rigi = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D targetinRadius = Physics2D.OverlapCircle(transform.position, 3f, target);

        if (targetinRadius != null)
        {
            Vector2 dirToTarget = (targetinRadius.transform.position - transform.position).normalized;
            //Debug.Log(dirToTarget);
            float angle = Vector2.SignedAngle(transform.position, dirToTarget);


            if (angle > 0)
            {
                posMultiplier = 2;
                int beginofCone = 45 * posMultiplier;
                float endofCone = beginofCone + fieldOfViewAngle;

                if (angle > beginofCone && angle < endofCone)
                {
                    //Debug.Log(angle);
                    //rigi.velocity = dirToTarget * velocity;
                }
                else rigi.velocity = new Vector2(0, 0);

            }
            else
            {
                negMultiplier = 2;
                int beginofCone = -45 * negMultiplier;
                float endofCone = beginofCone - fieldOfViewAngle;

                if (angle < beginofCone && angle > endofCone)
                {
                    //Debug.Log(angle);
                    //rigi.velocity = dirToTarget * velocity;
                }
                else rigi.velocity = new Vector2(0, 0);
            }


            animate(dirToTarget.x, dirToTarget.y);
        }
    }

    private void animate(float xDir, float yDir)
    {
        //let model face walking direction
        if (xDir > 0)
        {
            enemy.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        else if (xDir < 0)
        {
            enemy.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }

        AnimatorClipInfo[] clips = anim.GetCurrentAnimatorClipInfo(0);
        if (clips.Length > 0 && (clips[0].clip.name.Equals("Enemy_Devour")) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) return;
       
        if (xDir == 0 && yDir == 0) anim.Play("Enemy_Idle");

        if (xDir == 0 && yDir > 0) anim.Play("Enemy_WalkingUp");
        if (xDir == 0 && yDir < 0) anim.Play("Enemy_WalkingDown");


    }

    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            anim.Play("Enemy_Devour");
        }
    }
}