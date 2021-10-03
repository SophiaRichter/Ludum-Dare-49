using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    public Camera camera = null;
    private Rigidbody2D rigi;
    private float fadeVelocity = 2f;

    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }


    void fadeOut()
    {
        TrailRenderer tr = GetComponent<TrailRenderer>();
        tr.gameObject.SetActive(true);
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(camera.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        Debug.Log(bounds.min + " " + bounds.max);
        Vector2.MoveTowards(bounds.min, bounds.max, 1f);
        tr.gameObject.SetActive(false);
        
    }
}
