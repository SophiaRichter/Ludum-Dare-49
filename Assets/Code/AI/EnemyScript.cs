using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyScript : MonoBehaviour
{
    private Animator anim;
    public LayerMask target;
    public LayerMask obstacle;
    public float fieldOfViewAngle;
    private float velocity = 2f;
    private Transform enemy;
    public bool canBeEaten = true;
    private long timeSinceFound = 9999;
    private int durationMemory = 4000;

    public enum Orientation
    {
        North,
        South,
        West,
        East
    }
    
    public Orientation orientation = Orientation.North;
    private Rigidbody2D rigi;
    private float angleLimitA = 0;
    private float angleLimitB;

    void Start()
    {
        enemy = transform.Find("EnemySprite");
        anim = enemy.GetComponent<Animator>();
        rigi = GetComponentInParent<Rigidbody2D>();

        angleLimitB = angleLimitA + fieldOfViewAngle;
    }

    void Update()
    {
        Collider2D targetinRadius = Physics2D.OverlapCircle(transform.position, 3f, target);
        Vector2 dirToTarget = new Vector2(0,0);

        if (targetinRadius != null)
        {
            dirToTarget = (targetinRadius.transform.position - transform.position).normalized;
            //Debug.Log(dirToTarget);
            float angle = Vector2.SignedAngle(transform.position, dirToTarget);
            //Debug.Log(angle);

            if (orientation == Orientation.North)
            {
                angleLimitA = 45 - (fieldOfViewAngle / 2);
                angleLimitB = angleLimitA + fieldOfViewAngle;
                if (angle > angleLimitA && angle < angleLimitB)
                {
                    timeSinceFound = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                }
            }
            else if (orientation == Orientation.East)
            {
                angleLimitA = -45 - (fieldOfViewAngle / 2);
                angleLimitB = angleLimitA + fieldOfViewAngle;
                if (angle > angleLimitA && angle < angleLimitB)
                {
                    timeSinceFound = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                } 
            }
            else if (orientation == Orientation.South)
            {
                angleLimitA = -135 - (fieldOfViewAngle / 2);
                angleLimitB = angleLimitA + fieldOfViewAngle;
                if (angle > angleLimitA && angle < angleLimitB)
                {
                    timeSinceFound = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                } 
            }
            else if (orientation == Orientation.West)
            {
                angleLimitA = 135 - (fieldOfViewAngle / 2);
                angleLimitB = angleLimitA + fieldOfViewAngle;
                if (angle > angleLimitA && angle < angleLimitB)
                {
                    timeSinceFound = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                } 
            }

        }

        if (timeSinceFound != 0)
        {
            if (((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - timeSinceFound) < durationMemory)
            {
                canBeEaten = false;
                rigi.velocity = dirToTarget * velocity;
            }
            else
            {
                canBeEaten = true;
                rigi.velocity = new Vector2(0, 0);
                timeSinceFound = 0;
            }
        }

        setOrientation();
        animate();
        
    }

    private void setOrientation()
    {
        if (rigi.velocity.y > 0 && Mathf.Abs(rigi.velocity.y) > Mathf.Abs(rigi.velocity.x)) orientation = Orientation.North;
        if (rigi.velocity.y <= 0 && Mathf.Abs(rigi.velocity.y) > Mathf.Abs(rigi.velocity.x)) orientation = Orientation.South;
        if (rigi.velocity.x >= 0 && Mathf.Abs(rigi.velocity.x) > Mathf.Abs(rigi.velocity.y)) orientation = Orientation.East;
        if (rigi.velocity.x < 0 && Mathf.Abs(rigi.velocity.x) > Mathf.Abs(rigi.velocity.y)) orientation = Orientation.West;

    }

    private void animate()
    {
        //let model face walking direction
        if (orientation == Orientation.East)
        {
            enemy.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        else if (orientation == Orientation.West)
        {
            enemy.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }

        AnimatorClipInfo[] clips = anim.GetCurrentAnimatorClipInfo(0);
        //if (clips.Length > 0 && (clips[0].clip.name.Equals("Enemy_Devour")) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) return;


        if(rigi.velocity.x == 0 && rigi.velocity.y == 0 )
        { 
            if (orientation == Orientation.East || orientation == Orientation.West)  anim.Play(transform.name + "_Idle");
            if (orientation == Orientation.North) anim.Play(transform.name + "_IdleUp"); 
            if (orientation == Orientation.South) anim.Play(transform.name + "_IdleDown"); 
        }
        else
        { 
            if (orientation == Orientation.North) anim.Play(transform.name + "_WalkingUp");
            if (orientation == Orientation.East || orientation == Orientation.West) anim.Play(transform.name + "_Walking");
            else if (orientation == Orientation.South) anim.Play(transform.name + "_WalkingDown");
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerScript player = other.gameObject.GetComponent<PlayerScript>();

        if (other.gameObject.tag == "Player" && canBeEaten && !player.isTransformed)
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.tag == "Player" && !player.isTransformed)
        {
            Destroy(other.gameObject);
            SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
        }
    }
}