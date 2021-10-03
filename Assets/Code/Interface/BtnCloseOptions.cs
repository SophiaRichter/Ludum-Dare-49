using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnCloseOptions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void saveNcloseOptions()
    {
        Cursor.visible = true;
        GameObject.Find("Options").gameObject.SetActive(false);
    }

    public void closeOptions()
    {
        Cursor.visible = true;
        GameObject.Find("Options").gameObject.SetActive(false);
    }
}

