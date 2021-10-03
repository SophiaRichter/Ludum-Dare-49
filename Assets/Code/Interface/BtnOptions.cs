using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnOptions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openOptions()
    {
        Cursor.visible = true;
        GameObject.Find("Options").gameObject.SetActive(true);
    }
}
