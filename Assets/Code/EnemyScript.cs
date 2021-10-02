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
    private Rigidbody2D rigi;
    // Start is called before the first frame update
    void Start()
    {
        Transform enemy = transform.Find("EnSprite");
        anim = enemy.GetComponent<Animator>();
        rigi = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, 3f, target);

        foreach (var trg in targetsInViewRadius)
        {
            Vector2 dirToTarget = (trg.transform.position - transform.position).normalized;
            float angle = Vector2.Angle(transform.position, dirToTarget);
            if (angle * 2 < fieldOfViewAngle)
            {
                rigi.velocity = dirToTarget * velocity;
            }else rigi.velocity = new Vector2(0, 0);

        }

        //anim.Play("Enemy_Idle");
    }
}
