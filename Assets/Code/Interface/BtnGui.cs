using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnGui : MonoBehaviour
{
    Canvas c;

    void Start()
    {
        c = GetComponentInParent<Canvas>();
    }

    void Update()
    {

    }


    public void openMenu()
    {
        Cursor.visible = true;
        c.transform.Find("MainMenu").gameObject.SetActive(true);
    }
}
