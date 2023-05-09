using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
    private int boxStatus = 0; // 0 = Not activated | 1 = Crossed | 2 = Circle
    public int BoxStatus {
        get {
            return boxStatus;
        }

        set {
            boxStatus = value;
        }
    }

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

    public bool isEmpty()
    {
        return boxStatus == 0;
    }

    public Dictionary<Vector3, List<GameObject>> linesFaces = new Dictionary<Vector3, List<GameObject>>();

    private void Awake()
    {
        GlobalVar.resetGame += resetBox;
        GlobalVar.boxesInitialized += clearLineList;
    }

    private void resetBox(object sender, EventArgs e)
    {
        boxStatus = 0;
    }

    private void clearLineList(object sender, EventArgs e)
    {
        linesFaces.Clear();
    }

    private void OnDestroy()
    {
        GlobalVar.resetGame -= resetBox;
        GlobalVar.boxesInitialized -= clearLineList;
    }
}
