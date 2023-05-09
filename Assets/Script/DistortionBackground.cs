using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistortionBackground : MonoBehaviour
{

    private WindowManager windowManager;
    private RectTransform mainCanvas;
    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        GameObject backgroundCanvas = GameObject.FindGameObjectWithTag("BackgroundCanvas");
        mainCanvas = backgroundCanvas.GetComponent<RectTransform>();
        CanvasManager canvasManager = backgroundCanvas.GetComponent<CanvasManager>();
        canvasManager.OnCanvasSizeChange += resizeDistortionFilter;
    }

    private void resizeDistortionFilter(object sender, CanvasManager.canvasSize e)
    {
        rectTransform.transform.localScale = new Vector3(e.size.x, e.size.y, 1f);
    }
}
