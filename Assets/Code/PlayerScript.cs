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

    private bool isIdle = true;
    private bool isWalking = false;
    private bool isWalkingDown = false;
    private bool isWalkingUp = false;
    private int lastAnimation = 0;
    public bool isTransformed = false;
    private bool isManToBlob = false;
    private long timeOfTransformation = 0;

    public ArrayList items = new ArrayList();

    private int durationTransformation = 10000;
    private float velocity = 4f;
    private Transform itemAquiredLetter;
    private Transform itemAquiredToolbox;
    private Transform blob;
    private Transform rob;
    private Transform manToBlob;
    private Rigidbody2D rigidbody;
    private Animator anim;
    private Animator animManToBlob;
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
        rob = transform.Find("RobSprite");
        manToBlob = transform.Find("ManToBlob");
        
        itemAquiredLetter = transform.Find("ItemAquiredLetter");
        itemAquiredToolbox = transform.Find("ItemAquiredToolbox");

        anim = blob.GetComponent<Animator>();
        animManToBlob = manToBlob.GetComponent<Animator>();
    }

    void Update()
    {
        float xDir = Input.GetAxisRaw("Horizontal") * timer / 100;
        float yDir = Input.GetAxisRaw("Vertical") * timer / 100;

        setDirections(xDir, yDir);
        move(velocity, xDir, yDir);
        
        if (isTransformed && !isManToBlob) animateHuman();
        if (!isTransformed) animateBlob();
        if (isTransformed && isManToBlob) animateManToBlob();
        interact();

    }

    private void interact()
    {
        if (Input.GetButtonDown("Eat"))
        {
            Collider2D targetinRadius = Physics2D.OverlapCircle(transform.position, 1f);

            if (targetinRadius.tag == "Vent" && !isTransformed)
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
        if (Input.GetButtonDown("Transform"))
        {
            if (isTransformed)
            {
                transformToBlob();
            }
            else
            {
                transformToHuman();
            }
        }

        if (isTransformed && ((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - timeOfTransformation) > durationTransformation)
        {
            transformToBlob();
        }

        if (Input.GetButtonDown("Interact"))
        { 
            //if ()
        
        }
    }

    private void transformToManToBlob()
    {
        isManToBlob = true;
        transform.Find("Blob").gameObject.SetActive(false);
        transform.Find("RobSprite").gameObject.SetActive(false);
        transform.Find("ManToBlob").gameObject.SetActive(true);
        animManToBlob = manToBlob.GetComponent<Animator>();
    }

    private void transformToBlobToMan()
    {
        isManToBlob = false;
        transform.Find("Blob").gameObject.SetActive(false);
        transform.Find("RobSprite").gameObject.SetActive(true);
        transform.Find("ManToBlob").gameObject.SetActive(false);
    }

    private void transformToBlob()
    {
        timeOfTransformation = 0;
        isTransformed = false;
        isManToBlob = false;
        gameObject.layer = 8; //target
        transform.Find("Blob").gameObject.SetActive(true);
        transform.Find("RobSprite").gameObject.SetActive(false);
        transform.Find("ManToBlob").gameObject.SetActive(false);
        anim = blob.GetComponent<Animator>();
    }

    private void transformToHuman()
    {
        isTransformed = true;
        gameObject.layer = 0;
        transform.Find("Blob").gameObject.SetActive(false);
        transform.Find("RobSprite").gameObject.SetActive(true);
        transform.Find("ManToBlob").gameObject.SetActive(false);
        anim = rob.GetComponent<Animator>();
        timeOfTransformation = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        StopAllCoroutines();
        StartCoroutine(glitchManToBlob());
        

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



    private void animateManToBlob()
    {
        if (isIdle)
        {
            if (lastAnimation == 0) animManToBlob.Play("man_Idle");
            if (lastAnimation == 1) animManToBlob.Play("man_IdleDown");
            if (lastAnimation == 2) animManToBlob.Play("man_IdleUp");
            return;
        }
        if (isWalking)
        {
            animManToBlob.Play("man_Walking");
            lastAnimation = 0;
            return;
        }
        if (isWalkingDown)
        {
            animManToBlob.Play("man_WalkingDown");
            lastAnimation = 1;
            return;
        }
        if (isWalkingUp)
        {
            animManToBlob.Play("man_WalkingUp");
            lastAnimation = 2;
            return;
        }

    }

    private void animateHuman()
    {
        if (isIdle)
        {
            if (lastAnimation == 0) anim.Play("Rob_Idle");
            if (lastAnimation == 1) anim.Play("Rob_IdleDown");
            if (lastAnimation == 2) anim.Play("Rob_IdleUp");
            return;
        }
        if (isWalking)
        {
            anim.Play("Rob_Walking");
            lastAnimation = 0;
            return;
        }
        if (isWalkingDown)
        {
            anim.Play("Rob_WalkingDown");
            lastAnimation = 1;
            return;
        }
        if (isWalkingUp)
        {
            anim.Play("Rob_WalkingUp");
            lastAnimation = 2;
            return;
        }
    }

    private void animateBlob()
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
            if (!isTransformed) blob.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            else if(isTransformed && !isManToBlob) rob.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            else if(isTransformed && isManToBlob) manToBlob.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        else if (xDir < 0)
        {
            if (!isTransformed) blob.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
            else if (isTransformed && !isManToBlob) rob.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
            else if (isTransformed && isManToBlob) manToBlob.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" && other.gameObject.GetComponent<EnemyScript>().canBeEaten && !isTransformed)
        {
            anim.Play("Blob_Devour");
            StartCoroutine(waitForAnimation(1.8f));
        }
        if (other.gameObject.tag == "Door" && isTransformed)
        {
            Destroy(other.gameObject);
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

    IEnumerator glitchManToBlob()
    {
        yield return new WaitForSeconds((durationTransformation / 1000) / 10 * 5 );
        if (!isTransformed) yield break;
        transformToManToBlob();
        yield return new WaitForSeconds(0.3f);
        if (!isTransformed) yield break;
        transformToBlobToMan();
        yield return new WaitForSeconds(1.3f);
        if (!isTransformed) yield break;
        transformToManToBlob();
        yield return new WaitForSeconds(0.5f);
        if (!isTransformed) yield break;
        transformToBlobToMan();
        yield return new WaitForSeconds(1.0f);
        if (!isTransformed) yield break;
        transformToManToBlob();
        yield return new WaitForSeconds(0.4f);
        if (!isTransformed) yield break;
        transformToBlobToMan();
        yield return new WaitForSeconds((durationTransformation / 1000) / 10 * 1);
        if (!isTransformed) yield break;
        transformToManToBlob();

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
