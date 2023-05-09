using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoughtsAndCrosses : MonoBehaviour
{
    private Transform cameraTransform;
    [SerializeField] private bool menuSymbol = false;

    private int layer;
    public int Layer
    {
        get
        {
            return layer;
        }

        set
        {
            layer = value;
        }
    }

    public bool isCross { get; set; }

    private LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        lr = gameObject.transform.GetChild(0).GetComponent<LineRenderer>();
        if (!menuSymbol) {
            GlobalVar.resetGame += deleteObject;
            GlobalVar.layerActive += changeMaterial;
        }
        transform.LookAt(cameraTransform);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cameraTransform);
    }

    private void deleteObject(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }

    private void changeMaterial(object sender, GlobalVar.layerClass e)
    {
        if (e.layerIndicator == 0 || e.layerIndicator - 1 == layer) {
            lr.material = isCross ? GlobalVar.crossMaterial : GlobalVar.noughtMaterial;
        } else {
            lr.material = GlobalVar.symbolEmissionOff;
        }
    }

    private void OnDestroy()
    {
        GlobalVar.resetGame -= deleteObject;
        GlobalVar.layerActive -= changeMaterial;
    }
}
