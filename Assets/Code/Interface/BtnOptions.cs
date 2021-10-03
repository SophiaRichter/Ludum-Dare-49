using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnOptions : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
        
    }

    public void openOptions()
    {
        Cursor.visible = true;
        GameObject.Find("Canvas").transform.Find("Options").gameObject.SetActive(true);
    }
}
