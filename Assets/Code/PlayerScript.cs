using Assets.Code;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public int timer = 100;
    public Sprite loveLetter;
    public Sprite toolBox;

    bool isIdle = true;
    bool isWalking = false;
    bool isWalkingDown = false;
    bool isWalkingUp = false;
    int lastAnimation = 0;

    public ArrayList items = new ArrayList();

    private float velocity = 4f;
    private Transform itemAquiredLetter;
    private Transform itemAquiredToolbox;
    private Transform blob;
    private Rigidbody2D rigidbody;
    private Animator anim;
    private ArrayList specialAnimation = new ArrayList { "Blob_ItemAquired", "Blob_Devour", "Blob_Vent" };

    void Start()
    {
        if (PlayerPrefs.HasKey("Items"))
        { 
            //recover all items
        }

        Cursor.visible = false;
        rigidbody = GetComponent<Rigidbody2D>();
        blob = transform.Find("Blob");
        itemAquiredLetter = transform.Find("ItemAquiredLetter");
        itemAquiredToolbox = transform.Find("ItemAquiredToolbox");
        anim = blob.GetComponent<Animator>();

    }

    void Update()
    {
        float xDir = Input.GetAxisRaw("Horizontal") * timer / 100;
        float yDir = Input.GetAxisRaw("Vertical") * timer / 100;

        setDirections(xDir, yDir);
        move(velocity, xDir, yDir);
        animate();
        interact();

    }

    private void interact()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetinRadius = Physics2D.OverlapCircle(transform.position, 1f);

            if (targetinRadius.tag == "Vent")
            {
                anim.Play("Blob_Vent");
                transform.position = targetinRadius.transform.position;
                StartCoroutine(waitForAnimation(0.9f));
                StartCoroutine(loadNewLevel("Level 1"));
            }
        }
        if (Input.GetKeyDown(KeyCode.AltGr) || Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (Cursor.visible)
            {
                Cursor.visible = false;
            }else Cursor.visible = true;
        }
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
        
        if (clips.Length > 0 && specialAnimation.Contains(clips[0].clip.name.ToString()) && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return;
        }

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
        rigidbody.velocity = new Vector2(vel * xDir, vel * yDir);

        //let model face walking direction
        if (xDir > 0)
        {
            blob.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        else if (xDir < 0)
        {
            blob.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" && other.gameObject.GetComponent<EnemyScript>().canBeEaten)
        {
            //SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);

            anim.Play("Blob_Devour");
            StartCoroutine(waitForAnimation(1.8f));
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Item")
        {
            anim.Play("Blob_ItemAquired");

            if (other.gameObject.name.Equals("LoveLetter"))
            {
                items.Add(new Item("LoveLetter", "Some description", loveLetter));
                itemAquiredLetter.gameObject.SetActive(true); //for victory pose
            }
            else if (other.gameObject.name.Equals("Toolbox"))
            {
                items.Add(new Item("Toolbox", "A handy toolbox", toolBox));
                itemAquiredToolbox.gameObject.SetActive(true); //for victory pose
            }
            StartCoroutine(waitForAnimation(0.9f));
            Destroy(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

    }

    IEnumerator waitForAnimation(float time)
    {
        timer = 1;
        yield return new WaitForSeconds(time);
        resetEquipment();
        timer = 100;
    }

    void resetEquipment()
    {
        itemAquiredLetter.gameObject.SetActive(false);
        itemAquiredToolbox.gameObject.SetActive(false);
    }


    IEnumerator loadNewLevel(String levelName)
    {

        timer = 1;
        yield return new WaitForSeconds(0.8f);
        timer = 100;
        GameObject.Find("Player").GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(false);
        //save items
        string itemsToSave = "";
        foreach (Item i in items) itemsToSave += (i.name + ",");
        PlayerPrefs.SetString("Items",itemsToSave);
        PlayerPrefs.Save();
        GameObject.Find("UI").BroadcastMessage("fadeOut");
        yield return new WaitForSeconds(2.2f);
        SceneManager.LoadScene(levelName);
    }

    public void resumeGame()
    {
        timer = 100;
    }

    public void pauseGame()
    {
        timer = 1;
    }

}
