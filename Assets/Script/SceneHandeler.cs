using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using TMPro;

public class SceneHandeler : MonoBehaviour
{

    [SerializeField] private SizeInitializer sizeInitializer;
    [SerializeField] private GameObject endBlackBackground;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Image[] dotsText;

    private void Awake()
    {
        Loader.fadeLoadComplete = endBlackBackground;
        Loader.loadingText = loadingText;
        Loader.dots = dotsText;
    }

    public void Load()
    {
        GlobalVar.dimensions = (int)sizeInitializer.Size;
        Loader.Load(Loader.Scene.MainScene);
    }

    private IEnumerator reduceAlphaOnMaterial(Material material, float duration)
    {
        float time = Time.time;
        float originalAlpha = material.color.a;
        WaitForSeconds delay = new WaitForSeconds(0.05f);

        while(material.color.a > 0) {
            Color materialColor = material.color;
            materialColor.a = Mathf.Lerp(originalAlpha, 0f, (Time.time - time) / duration);
            material.color = materialColor;
            yield return delay;
        }
    }

    private IEnumerator DisableGameObjects(GameObject[] listOfObjects, float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        for (int i = 0; i < listOfObjects.Length; i++) {
            listOfObjects[i].SetActive(false);
        }
    }


}
