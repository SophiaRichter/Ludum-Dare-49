using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnResumeScript : MonoBehaviour
{
    private Canvas c;

    void Start()
    {
        c = GetComponentInParent<Canvas>();
        
    }

    void Update()
    {
        
    }

    public void resume()
    {
        Cursor.visible = false;
        GameObject.Find("Player").SendMessage("resumeGame");
        c.transform.Find("MainMenu").gameObject.SetActive(false);
    }
}
