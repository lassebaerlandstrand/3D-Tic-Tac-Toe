using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CanvasManager : MonoBehaviour
{
    public event EventHandler<canvasSize> OnCanvasSizeChange;
    public class canvasSize : EventArgs
    {
        public Vector2 size;
    }
    private Vector2 previousSize;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newCanvasSize = rectTransform.sizeDelta;
        if (previousSize != newCanvasSize) {
            OnCanvasSizeChange?.Invoke(this, new canvasSize { size = newCanvasSize });
            previousSize = newCanvasSize;
        }
    }
}
