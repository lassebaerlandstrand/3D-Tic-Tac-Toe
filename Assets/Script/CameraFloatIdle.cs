using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFloatIdle : MonoBehaviour
{

    public GameObject objectToRotateAround;
    private float speed = 25f;

    private float perlinNoiseMultiplier = 0.05f;

    // Update is called once per frame
    void Update()
    {
        float verticalMultiplier = (Mathf.PerlinNoise(0f, Time.time * perlinNoiseMultiplier) - 0.5f) * 2f; // Gradually change rotating speed
        float horizontalMultiplier = (Mathf.PerlinNoise(Time.time * perlinNoiseMultiplier, 0f) - 0.5f) * 2f; // Gradually change rotating speed

        transform.RotateAround(objectToRotateAround.transform.position, transform.right, Time.deltaTime * speed * verticalMultiplier); // Vertical
        transform.RotateAround(objectToRotateAround.transform.position, transform.up, Time.deltaTime * speed * horizontalMultiplier); // Horizontal
    }
}
