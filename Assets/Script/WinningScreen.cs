using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinningScreen : MonoBehaviour
{

    [SerializeField] private Sprite[] symbolImages;
    [SerializeField] private GameObject winningScreenObject;
    [SerializeField] private Image winningImage;
    [SerializeField] private TextMeshProUGUI winningText;
    [SerializeField] private GameObject mainMenuObject;

    private Color[] glowColor = new Color[] { new Color(1.15f, 0, 0, 0.61f), new Color(0, 0.5f, 1.29f, 0.61f) };

    private void Awake()
    {
        GlobalVar.winningScreen = this;
    }

    public void setWinningScreen(int winningSymbol)
    {
        winningScreenObject.SetActive(true);
        mainMenuObject.SetActive(true);
        if (winningSymbol > 0) {
            winningImage.gameObject.SetActive(true);
            winningImage.sprite = symbolImages[winningSymbol - 1];
            winningText.SetText("WON");
            winningText.fontMaterial.SetColor("_UnderlayColor", glowColor[winningSymbol - 1]);
        } else {
            winningImage.gameObject.SetActive(false);
            winningText.SetText("TIE");
            winningText.fontMaterial.SetColor("_UnderlayColor", new Color(0f, 0f, 0f, 0f));
        }
        
    }

    public void removeWinningScreen()
    {
        winningScreenObject.SetActive(false);
        mainMenuObject.SetActive(false);
    }

    public void resetGame()
    {
        GlobalVar.resetGameFunction();
    }
}
