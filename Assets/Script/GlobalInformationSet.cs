using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInformationSet : MonoBehaviour
{
    public Material winningLineMaterial;

    private void Awake()
    {
        GlobalVar.winMaterialLine = winningLineMaterial;
    }
}
