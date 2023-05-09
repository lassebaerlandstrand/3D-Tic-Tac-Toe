using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using TMPro;

public static class Loader
{

    private class LoadingMonoBehaviour : MonoBehaviour { }

    public enum Scene
    {
        Menu,
        MainScene
    }

    private static AsyncOperation loadingAsyncOperation;
    private static GameObject loadingGameObject;

    public static void Load(Scene scene)
    {
        loadingGameObject = new GameObject("LoadingHandeler");
        loadingGameObject.AddComponent<DontDestroyOnSceneSwitch>();
        loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
    }

    public static GameObject fadeLoadComplete;
    public static Image[] dots;
    public static TextMeshProUGUI loadingText;

    private static IEnumerator LoadSceneAsync(Scene scene)
    {
        yield return null; // Go past one frame before loading

        loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
        loadingAsyncOperation.allowSceneActivation = false;
        float time = Time.time;

        LoadingMonoBehaviour loadingMonoBehaviourComponent = loadingGameObject.GetComponent<LoadingMonoBehaviour>();

        float animationTime = 1.5f;
        //loadingGameObject.GetComponent<LoadingMonoBehaviour>().StartCoroutine(loadingAnimation(true, animationTime));
        GameObject loadingScreen = GameObject.Instantiate(Resources.Load("Loading Screen") as GameObject);
        LoadingScreen loadingScreenComponent = loadingScreen.GetComponent<LoadingScreen>();
        loadingMonoBehaviourComponent.StartCoroutine(loadingScreenComponent.crossfade(true, animationTime));

        // Delay after the loading is completed, so that animations can run
        while (!loadingAsyncOperation.isDone) {
            if(loadingAsyncOperation.progress / 0.9f >= 1f) {
                if(Time.time - time >= animationTime + 0.05f) { // For loadingAnimation to complete
                    loadingMonoBehaviourComponent.StartCoroutine(loadingScreenComponent.crossfade(false, animationTime));
                    loadingMonoBehaviourComponent.StartCoroutine(deleteLoadingObjects(animationTime + 0.5f, new GameObject[] { loadingGameObject, loadingScreen }));
                    loadingAsyncOperation.allowSceneActivation = true;
                }
            }
            yield return null;
        }
    }

    public static IEnumerator deleteLoadingObjects(float delay, GameObject[] objects)
    {
        yield return new WaitForSeconds(delay);
        for(int i = 0; i < objects.Length; i++) {
            UnityEngine.Object.Destroy(objects[i]);
        }
    }
}
