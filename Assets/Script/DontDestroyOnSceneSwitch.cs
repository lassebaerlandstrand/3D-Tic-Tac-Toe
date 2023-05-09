using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnSceneSwitch : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
