using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour
{

    private int layer;
    public int Layer
    {
        get {
            return layer;
        }

        set {
            layer = value;
        }
    }

    private LineRenderer lineRenderer;

    private MaterialPropertyBlock propertyBlock;
    private Color emissionColor;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        GlobalVar.layerActive += changeMaterial;
    }

    public void changeMaterial(object sender, GlobalVar.layerClass e)
    {
        if (e.layerIndicator == 0 || Mathf.Abs(2 * e.layerIndicator - 2 - Layer) <= 1) {
            if (GlobalVar.dimensions <= GlobalVar.maxLayerOptimization) lineRenderer.material = GlobalVar.lineMaterial;
            else lineRenderer.enabled = true;
        } else {
            if (GlobalVar.dimensions <= GlobalVar.maxLayerOptimization) lineRenderer.material = GlobalVar.lineMaterialEmissionOff;
            else lineRenderer.enabled = false;
        }
    }

    private void OnDestroy()
    {
        GlobalVar.layerActive -= changeMaterial;
    }
}
