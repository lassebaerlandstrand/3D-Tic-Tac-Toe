using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crossfade : MonoBehaviour
{
    private Image crossfageImage;
    public bool fadeOut = true;
    public float timeToFade = 2f;
    private bool activate = false;

    // Start is called before the first frame update
    void Start()
    {
        crossfageImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (crossfageImage.color.a > 0 && fadeOut) {
            crossfageImage.color = new Color(crossfageImage.color.r, crossfageImage.color.g, crossfageImage.color.b, Mathf.Clamp01(crossfageImage.color.a - Time.deltaTime * Mathf.Pow(timeToFade, -1f)));
        } else if (fadeOut) {
            Destroy(gameObject);
        }

        if (!fadeOut && activate) {
            crossfageImage.color = new Color(crossfageImage.color.r, crossfageImage.color.g, crossfageImage.color.b, Mathf.Clamp01(crossfageImage.color.a + Time.deltaTime * Mathf.Pow(timeToFade, -1f)));
        }
    }

    public void activateCrossfade(float overrideFadeTime = 0f)
    {
        activate = true;
        if (overrideFadeTime > 0f) timeToFade = overrideFadeTime;
    }
}
