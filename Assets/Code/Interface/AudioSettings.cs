using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("AudioLevel"))
        {
            transform.GetComponent<Slider>().value = PlayerPrefs.GetFloat("AudioLevel");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setAudio(float sliderValue)
    {
        Debug.Log(Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("AudioLevel", Mathf.Log10(sliderValue) * 20);
    }
}
