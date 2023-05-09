using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SizeInitializer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sizeDisplay;
    [SerializeField] private GameObject infoOptimization;
    public int maxDimensionSize = 12;
    public int minDimensionSize = 1;

    private int size = 3;
    public int Size
    {
        get
        {
            return size;
        }

        set
        {
            size = value;
            sizeDisplay.SetText(size.ToString());
            infoOptimization.SetActive(size > GlobalVar.maxLayerOptimization);
        }
    }

}
