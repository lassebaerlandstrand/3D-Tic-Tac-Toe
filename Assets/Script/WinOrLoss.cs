using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public static class WinOrLoss
{
    private static int layerMask = (1 << GlobalVar.layerPlayerSelect) | (1 << GlobalVar.layerPlayerNotSelect);
    private static HashSet<int[]> directions = new HashSet<int[]>()
    {
        new int[] {-1, -1, -1 },
        new int[] {-1, -1, 0 },
        new int[] {-1, -1, 1 },
        new int[] {-1, 0, -1 },
        new int[] {-1, 0, 0 },
        new int[] {-1, 0, 1 },
        new int[] {-1, 1, -1 },
        new int[] {-1, 1, 0 },
        new int[] {-1, 1, 1 },
        new int[] {0, -1, -1 },
        new int[] {0, -1, 0 },
        new int[] {0, -1, 1 },
        new int[] { 0, 0, -1 },
    };

    public static void checkMatch(GameObject hitObject)
    {
        int boxStatus = hitObject.GetComponent<BoxScript>().BoxStatus;

        Vector3 center = hitObject.transform.position;
        float length = GlobalVar.size * GlobalVar.dimensions * Mathf.Sqrt(3);

        // Fire a debug for every direction around a cube (so that it hits every direction around a 3x3 cube)
        foreach (int[] direction in directions) {
            RaycastHit[] raycastHits = Physics.RaycastAll(center + new Vector3(direction[0], direction[1], direction[2]) * length, new Vector3(-direction[0], -direction[1], -direction[2]), length * 2f * Mathf.Sqrt(3), layerMask); // Length of ray, and vector magnitude is not the same, so we need to multiply the length with sqrt(3)
            int consecutivelyStatus = 0;
            if (raycastHits.Length > 1) { // Hit something other than itself
                for (int l = 0; l < raycastHits.Length; l++) {
                    if (raycastHits[l].transform.GetComponent<BoxScript>().BoxStatus == boxStatus) {
                        consecutivelyStatus++;
                    }
                }
                if (consecutivelyStatus == GlobalVar.dimensions) {
                    GlobalVar.gameCompleted = true;
                    GlobalVar.gameFinished(boxStatus);

                    GameObject winLineParent = new GameObject("Win Line");
                    winLineParent.tag = "Win Line";

                    float[] vectorLength = raycastHits.Select(vector => vector.transform.position.sqrMagnitude).ToArray();
                    float maxLength = vectorLength.Max();
                    RaycastHit[] mostDistantVectors = Array.FindAll(raycastHits, vector => vector.transform.position.sqrMagnitude == maxLength); // Probably not the most efficient
                    Utilities.DrawLine(mostDistantVectors[0].transform.position, mostDistantVectors[1].transform.position, 0.03f, GlobalVar.winMaterialLine, winLineParent.transform);
                }
            }
        }
    }

    public static void setTie()
    {
        GlobalVar.gameCompleted = true;
        GlobalVar.gameFinished(0);
    }


    private static Dictionary<int?, int> lookupValues = new Dictionary<int?, int>()
    {
        { 1, -10 }, // X
        { 2, 10 }, // O
        { 0, 0 } // Tie
    };

    public static int? checkAIMatch(BoxScript boxScript, int openBoxes)
    {
        int boxStatus = boxScript.BoxStatus;
        Vector3 center = boxScript.gameObject.transform.position;
        float length = GlobalVar.size * GlobalVar.dimensions * Mathf.Sqrt(3);


        for (int i = -1; i <= 0; i++) {
            for (int j = -1; j <= (i == 0 ? 0 : 1); j++) { // Since we fire raycast in in two directions, we only need to loop through half the possibilities
                for (int k = -1; k <= (i == 0 && j == 0 ? 0 : 1); k++) {
                    if (i == 0 && j == 0 && k == 0) continue;

                    RaycastHit[] raycastHits = Physics.RaycastAll(center + new Vector3(i, j, k) * length, new Vector3(-i, -j, -k), length * 2f * Mathf.Sqrt(3), layerMask);
                    int consecutivelyHits = 0;
                    if (raycastHits.Length > 1) { // Hit something other than itself
                        for (int l = 0; l < raycastHits.Length; l++) {
                            if (raycastHits[l].transform.GetComponent<BoxScript>().BoxStatus == boxStatus) {
                                consecutivelyHits++;
                            }
                        }
                        if (consecutivelyHits == GlobalVar.dimensions) {
                            return lookupValues[boxStatus];
                        }
                    }
                }
            }
        }

        // Tie
        if (openBoxes == 0)
            return 0;

        return null;
    }

}
