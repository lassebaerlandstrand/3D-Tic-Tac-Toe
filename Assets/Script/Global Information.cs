using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Layers
{
    public struct layerInformation
    {
        public GameObject layerObject;
        public Material materialLayer;
        public Material materialCross;
        public Material materialNought;
    }
    public static layerInformation[] listOfLayers;

    public struct lineMaterialStruct
    {
        public Material lineMaterial;
    }
    public static lineMaterialStruct[] lineMaterials;

}

public static class GlobalVar
{
    public static float size = 1f;
    public static int dimensions = 3;
    public static bool gameCompleted = false;
    public static int layerPlayerSelect = 6;
    public static int layerPlayerNotSelect = 7;
    public static int maxLayerOptimization = 12;
    private static int openBoxesInitialize = 27;
    public static int openBoxes = 0;
    public static int OpenBoxesInitialize { get { return openBoxesInitialize; } set { openBoxesInitialize = value; openBoxes = value; } }
    

    public static GameObject boxHighlight;
    public static WinningScreen winningScreen;
    public static Material winMaterialLine;
    public static Material lineMaterial;
    public static Material lineMaterialEmissionOff;
    public static Material crossMaterial;
    public static Material noughtMaterial;
    public static Material symbolEmissionOff;
    public static BoxScript[,,] gridBoxes;

    public static string[] nameOnStatus = new string[] { "Invalid", "Crosses", "Noughts" };

    public static void gameFinished(int winIndicator)
    {
        boxHighlight.SetActive(false);
        winningScreen.setWinningScreen(winIndicator);
    }

    public static Dictionary<string, int> turnIndex = new Dictionary<string, int>()
    {
        { "Player", 0 },
    };

    public static bool PlayerTurn { get; set; }

    // Turn
    public class turnClass : EventArgs
    {
        public int turnIndicator;
    }
    public static event EventHandler<turnClass> nextTurn;

    private static int turnIndicator = 0;
    public static int TurnIndicator
    {
        get
        {
            return turnIndicator;
        }
        set
        {
            turnIndicator = value;
            openBoxes = OpenBoxesInitialize - value;
            if(openBoxes == 0 && !gameCompleted) { WinOrLoss.setTie(); }
            nextTurn?.Invoke(null, new turnClass { turnIndicator = turnIndicator });
        }
    }

    // Layer Active
    public class layerClass : EventArgs
    {
        public int layerIndicator;
    }
    public static event EventHandler<layerClass> layerActive;

    private static int layerIndicator = 0;
    public static int LayerIndicator
    {
        get
        {
            return layerIndicator;
        }
        set
        {
            layerIndicator = value;
            layerActive?.Invoke(null, new layerClass { layerIndicator = layerIndicator });
        }
    }


    public static event EventHandler resetGame;
    public static void resetGameFunction()
    {
        resetGame?.Invoke(null, EventArgs.Empty);
        winningScreen.removeWinningScreen();
        GameObject[] winningLines = GameObject.FindGameObjectsWithTag("Win Line");
        for(int i = 0; i < winningLines.Length; i++) {
            UnityEngine.Object.Destroy(winningLines[i]);
        }
        TurnIndicator = 0;
        boxHighlight.SetActive(true);
        gameCompleted = false;
    }

    public static event EventHandler boxesInitialized;
    public static void boxesInitializedEvent()
    {
        boxesInitialized?.Invoke(null, EventArgs.Empty);
    }


    public static void resetStaticVariables()
    {
        size = 1f;
        gameCompleted = false;
        layerPlayerSelect = 6;
        layerPlayerNotSelect = 7;
        maxLayerOptimization = 12;
        openBoxesInitialize = 27;
        openBoxes = 0;
        turnIndicator = 0;
    }
}
