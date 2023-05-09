using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnUIScript : MonoBehaviour
{

    [SerializeField] private GameObject xSymbolUI;
    [SerializeField] private GameObject oSymbolUI;

    private void Awake()
    {
        GlobalVar.nextTurn += setSymbolActive;
    }

    public void setSymbolActive(object sender, GlobalVar.turnClass e)
    {
        xSymbolUI.SetActive(e.turnIndicator % 2 == 0);
        oSymbolUI.SetActive(e.turnIndicator % 2 != 0);
    }

    private void OnDestroy()
    {
        GlobalVar.nextTurn -= setSymbolActive;
    }
}
