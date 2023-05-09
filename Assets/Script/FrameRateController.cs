using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateController : MonoBehaviour
{
    public int frameRate = 60;

    // Start is called before the first frame update
    void Start()
    {
        #if !UNITY_WEBGL
            Application.targetFrameRate = frameRate;
        #endif
    }
}
