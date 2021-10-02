using Assets.Code;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public int time = 100;

    bool isIdle = true;
    bool isWalking = false;
    bool isWalkingDown = false;
    bool isWalkingUp = false;
    int lastAnimation = 0;

    public ArrayList items = new ArrayList();
    
    
    private float velocity = 2f;
    private Transform itemAquiredLetter;
    private Transform itemAquiredToolbox;
    private Transform blob;
    private Rigidbody2D rigidbody;
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        blob = transform.Find("Blob");
        itemAquiredLetter = transform.Find("ItemAquiredLetter");
        itemAquiredToolbox = transform.Find("ItemAquiredToolbox");
        anim = blob.GetComponent<Animator>(); 

    }

    // Update is called once per frame
    void Update()
    {
        float xDir = Input.GetAxisRaw("Horizontal") * time /100;
        float yDir = Input.GetAxisRaw("Vertical") * time /100;
        
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
        //give priority over lesser animations
        AnimatorClipInfo[] clips = anim.GetCurrentAnimatorClipInfo(0);
        if (clips.Length > 0 && clips[0].clip.name.Equals("Blob_ItemAquired") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) return;
        resetEquipment();

        if (isIdle)
        {
            if (lastAnimation == 0) anim.Play("Blob_Idle");
            if (lastAnimation == 1) anim.Play("Blob_IdleDown");
            if (lastAnimation == 2) anim.Play("Blob_IdleUp");
            return;
        }

        if (isWalking)
        {
            anim.Play("Blob_Walking");
            lastAnimation = 0;
            return;
        }

        if (isWalkingDown)
        {
            anim.Play("Blob_WalkingDown");
            lastAnimation = 1;
            return;
        }

        if (isWalkingUp)
        {
            anim.Play("Blob_WalkingUp");
            lastAnimation = 2;
            return;
        }
    }




    private void move(float vel, float xDir, float yDir)
    {
        rigidbody.velocity = new Vector2(vel * xDir,vel * yDir);       

        //let model face walking direction
        if (xDir > 0)
        {
            blob.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        else if (xDir < 0 )
        {
            blob.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Item")
        {
            anim.Play("Blob_ItemAquired");

            if (other.gameObject.name.Equals("LoveLetter"))
            {
                items.Add(new Item("LoveLetter", "Some description"));
                itemAquiredLetter.gameObject.SetActive(true);
            }
            else if (other.gameObject.name.Equals("Toolbox"))
            {
                items.Add(new Item("Toolbox", "A handy toolbox"));
                itemAquiredToolbox.gameObject.SetActive(true);
            }
            StartCoroutine(resetEquipment());
            Destroy(other.gameObject);
            
        }
    }

    IEnumerator resetEquipment()
    {
        yield return new WaitForSeconds(0.9f);
        itemAquiredLetter.gameObject.SetActive(false);
        itemAquiredToolbox.gameObject.SetActive(false);
    }
}
