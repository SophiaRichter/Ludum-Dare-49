using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setAudio(float sliderValue)
    {
        Debug.Log(Mathf.Log10(sliderValue) * 20);
    }
}
