using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


// @reference https://www.youtube.com/watch?v=zit45k6CUMk
public class Parallex : MonoBehaviour
{
    [Range(0, 2f)]
    public float ParallaxEffect;
    private float height, startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.y;
        height = GetComponent<SpriteRenderer>().bounds.size.y / 3;
    }

    // Update is called once per frame
    void Update()
    {
        Camera cam = Game.obj.Camera;
        float distance = cam.transform.position.y * ParallaxEffect;
        float topDis = cam.transform.position.y * (1 - ParallaxEffect);

        transform.position = new Vector3(transform.position.x, startPos + distance, transform.position.z);
        if (topDis > startPos + height) { startPos += height; }
        else if (topDis < startPos - height) { startPos -= height;  }
    }
}
