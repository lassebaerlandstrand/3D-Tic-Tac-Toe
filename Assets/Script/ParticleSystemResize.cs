using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemResize : MonoBehaviour
{

    private WindowManager windowManager;
    private ParticleSystem ps;
    private RectTransform mainCanvas;
    private float emissionPerPixel;

    public bool scaleEmissionWithScreen = false;


    private void Awake()
    {
        GameObject backgroundCanvas = GameObject.FindGameObjectWithTag("BackgroundCanvas");
        mainCanvas = backgroundCanvas.GetComponent<RectTransform>();
        CanvasManager canvasManager = backgroundCanvas.GetComponent<CanvasManager>();
        canvasManager.OnCanvasSizeChange += resizeParticleSystem;
        ps = GetComponent<ParticleSystem>();
        emissionPerPixel = ps.emission.rateOverTimeMultiplier / (Screen.height * Screen.width);
    }

    private void resizeParticleSystem(object sender, CanvasManager.canvasSize e)
    {
        ParticleSystem.ShapeModule shapeModule = ps.shape;
        shapeModule.scale = new Vector3(mainCanvas.sizeDelta.x * mainCanvas.transform.localScale.x, mainCanvas.sizeDelta.y * mainCanvas.transform.localScale.y, 1f);
        if (scaleEmissionWithScreen) {
            ParticleSystem.EmissionModule emissionModule = ps.emission;
            emissionModule.rateOverTimeMultiplier = Screen.height * Screen.width * emissionPerPixel;
        }
    }
}
