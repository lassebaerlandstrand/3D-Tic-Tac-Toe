using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WindowManager : MonoBehaviour
{
    public event EventHandler<screenSize> OnScreenSizeChange;
    public class screenSize : EventArgs
    {
        public Vector2 size;
    }
    private Vector2 previousSize;

    // Update is called once per frame
    void Update()
    {
        Vector2 newScreenSize = new Vector2(Screen.width, Screen.height);
        if (previousSize != newScreenSize) {
            OnScreenSizeChange?.Invoke(this, new screenSize { size = newScreenSize });
            previousSize = newScreenSize;
        }
    }
}
