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

    private static Action onLoaderCallback;
    private static AsyncOperation loadingAsyncOperation;
    private static GameObject loadingGameObject;

    public static void Load(Scene scene)
    {
        loadingGameObject = new GameObject("LoadingHandeler");
        loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
    }

    /*public static void LoaderCallback()
    {
        if(onLoaderCallback != null) {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }*/

    public static GameObject fadeLoadComplete;
    public static Image[] dots;
    public static TextMeshProUGUI loadingText;

    private static IEnumerator LoadSceneAsync(Scene scene)
    {
        yield return null; // Go past frame before loading

        loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
        loadingAsyncOperation.allowSceneActivation = false;
        float time = Time.time;

        float animationTime = 1.5f;
        loadingGameObject.GetComponent<LoadingMonoBehaviour>().StartCoroutine(loadingAnimation(true, animationTime));

        // Delay after the loading is completed, so that animations can run
        while (!loadingAsyncOperation.isDone) {
            if(loadingAsyncOperation.progress / 0.9f >= 1f) {
                if(Time.time - time >= animationTime) { // For loadingAnimation to complete
                    loadingAsyncOperation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }

    public static IEnumerator loadingAnimation(bool fadeIn, float duration)
    {
        if (fadeLoadComplete != null) {
            if (fadeIn) {
                fadeLoadComplete.SetActive(true);
                fadeLoadComplete.GetComponent<Crossfade>().activateCrossfade();
            } else {
                fadeLoadComplete.GetComponent<Crossfade>().fadeOut = true;
                fadeLoadComplete.GetComponent<Crossfade>().timeToFade = duration;
            }
            
        }


        WaitForSeconds delay = new WaitForSeconds(0.01f);
        float time = Time.time;
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;
        duration *= fadeIn ? 1f : 0.3f; // Shorten duration when fadeOut

        while (Time.time - time <= duration) {
            float alphaValue = Mathf.Lerp(startAlpha, endAlpha, (Time.time - time) / duration);
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, alphaValue);
            for(int i = 0; i < dots.Length; i++) {
                dots[i].color = new Color(dots[i].color.r, dots[i].color.g, dots[i].color.b, alphaValue);
            }

            yield return delay;
        }
        loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, endAlpha);
        for (int i = 0; i < dots.Length; i++) {
            dots[i].color = new Color(dots[i].color.r, dots[i].color.g, dots[i].color.b, endAlpha);
        }

    }
}
