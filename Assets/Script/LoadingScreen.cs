using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{

    [SerializeField] private Image blackBackground;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Image[] dots;

    public IEnumerator crossfade(bool fadeIn, float duration)
    {
        WaitForSeconds delay = new WaitForSeconds(0.01f);
        float time = Time.time;
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;

        // Fade images
        while (Time.time - time <= duration) {
            float alphaValue = Mathf.Lerp(startAlpha, endAlpha, (Time.time - time) / duration);
            blackBackground.color = new Color(blackBackground.color.r, blackBackground.color.g, blackBackground.color.b, alphaValue);
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, alphaValue);
            for (int i = 0; i < dots.Length; i++) {
                dots[i].color = new Color(dots[i].color.r, dots[i].color.g, dots[i].color.b, alphaValue);
            }

            yield return delay;
        }

        // Set final color for images
        blackBackground.color = new Color(blackBackground.color.r, blackBackground.color.g, blackBackground.color.b, endAlpha);
        loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, endAlpha);
        for (int i = 0; i < dots.Length; i++) {
            dots[i].color = new Color(dots[i].color.r, dots[i].color.g, dots[i].color.b, endAlpha);
        }
    }
}
