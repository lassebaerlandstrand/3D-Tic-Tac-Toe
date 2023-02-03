using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneHandler : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Loader.loadingAnimation(false, 3f));
        Invoke("deleteDontDestroyObjects", 3f);
    }

    private void deleteDontDestroyObjects()
    {
        for(int i = 0; i < GlobalVar.dontDestroyOnLoadObject.Count; i++) {
            Destroy(GlobalVar.dontDestroyOnLoadObject[i]);
        }
    }
}
