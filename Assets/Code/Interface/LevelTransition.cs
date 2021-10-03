using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    public Camera camera = null;
    private Rigidbody2D rigi;
    private float fadeVelocity = 2f;
    Bounds bounds;

    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, bounds.max)> 200)
        {
            rigi.velocity = new Vector2(0, 0);
        }
    }


    void fadeOut()
    {
        TrailRenderer tr = GetComponent<TrailRenderer>();
        tr.gameObject.SetActive(true);
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        bounds = new Bounds(camera.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        //transform.position = bounds.min - new Vector3(100,0,0);
        transform.position = new Vector2(bounds.min.x, bounds.min.y);
        rigi.velocity = bounds.max * 4f;
        
    }
}
