using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crossfade : MonoBehaviour
{
    private Image crossfadeImage;
    public bool fadeOut = true;
    public float timeToFade = 2f;
    private bool activate = false;

    // Start is called before the first frame update
    void Start()
    {
        crossfadeImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (crossfadeImage.color.a > 0 && fadeOut) {
            crossfadeImage.color = new Color(crossfadeImage.color.r, crossfadeImage.color.g, crossfadeImage.color.b, Mathf.Clamp01(crossfadeImage.color.a - Time.deltaTime * Mathf.Pow(timeToFade, -1f)));
        } else if (fadeOut) {
            Destroy(gameObject);
        }

        if (!fadeOut && activate) {
            crossfadeImage.color = new Color(crossfadeImage.color.r, crossfadeImage.color.g, crossfadeImage.color.b, Mathf.Clamp01(crossfadeImage.color.a + Time.deltaTime * Mathf.Pow(timeToFade, -1f)));
        }
    }

    public void activateCrossfade(float overrideFadeTime = 0f)
    {
        activate = true;
        if (overrideFadeTime > 0f) timeToFade = overrideFadeTime;
    }

    
}
