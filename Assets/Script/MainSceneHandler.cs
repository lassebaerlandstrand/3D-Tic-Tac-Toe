using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneHandler : MonoBehaviour
{

    private void Awake()
    {
        GlobalVar.resetStaticVariables();
    }

    public void LoadMenu()
    {
        Loader.Load(Loader.Scene.Menu);
    }
}
