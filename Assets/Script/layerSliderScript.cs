using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class layerSliderScript : MonoBehaviour
{

    [SerializeField] private InitializeCube initializeCubeScript; 
    private Slider slider;
    [SerializeField] private TextMeshProUGUI layerActiveText;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = initializeCubeScript.getLayerSize();
    }

    public void setLayerText(float layerActive)
    {
        if (layerActive == 0) {
            layerActiveText.SetText("All");
        } else {
            layerActiveText.SetText(layerActive.ToString()); 
        }
    }
}
