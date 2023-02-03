using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;


public static class Utilities
{

    //private static StaticEditorFlags staticFlags = StaticEditorFlags.BatchingStatic | StaticEditorFlags.NavigationStatic | StaticEditorFlags.OffMeshLinkGeneration;

    public static GameObject DrawLine(Vector3 start, Vector3 end, float width, Material material,  Transform parent = null, bool useWorldSpace = true, bool haveLineScript = false, bool isStatic = true)
    {
        GameObject line = new GameObject("Line");
        line.transform.parent = parent;
        if(haveLineScript) line.AddComponent<LineScript>();
        if (!useWorldSpace) line.transform.localPosition = Vector3.zero;
        /*if (isStatic) { // Doesn't do anything when doing this is runtime
            GameObjectUtility.SetStaticEditorFlags(line, staticFlags);
        }*/
        line.AddComponent<LineRenderer>();
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.material = material;
        lineRenderer.useWorldSpace = useWorldSpace;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lineRenderer.receiveShadows = false;

        return line;
    }

    public static void DrawLines(Vector3[] positions, float width, Material material, bool connectEndPoint = false, Transform parent = null, bool useWorldSpace = true, bool isStatic = true)
    {
        GameObject line = new GameObject("Line");
        line.transform.parent = parent;
        if (!useWorldSpace) line.transform.localPosition = Vector3.zero;
        /*if (isStatic) {
            GameObjectUtility.SetStaticEditorFlags(line, staticFlags);
        }*/
        line.AddComponent<LineRenderer>();
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.material = material;
        lineRenderer.useWorldSpace = useWorldSpace;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lineRenderer.receiveShadows = false;
        lineRenderer.loop = connectEndPoint;
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }


    public static void CreateXSymbol(GameObject boxObject, Vector3 position, Vector3 centerOfCube, float size, int layer)
    {
        GameObject xSymbol = new GameObject("XSymbol");
        float multiplierSize = size / (2 * Mathf.Sqrt(2)); // Pythagoras with hypotenuse of size/2f
                                                           //float width = 0.08f;


        xSymbol.transform.position = position;

        //Vector3 center = position - new Vector3(multiplierSize, multiplierSize, multiplierSize);
        /*Utilities.DrawLines(center + new Vector3(-multiplierSize, -multiplierSize, -multiplierSize), center + new Vector3(multiplierSize, multiplierSize, multiplierSize), width, material, null, true);
        Utilities.DrawLines(center + new Vector3(-multiplierSize, multiplierSize, -multiplierSize), center + new Vector3(multiplierSize, -multiplierSize, multiplierSize), width, material, null, true);

        Utilities.DrawLines(center + new Vector3(-multiplierSize, -multiplierSize, multiplierSize), center + new Vector3(multiplierSize, multiplierSize, -multiplierSize), width, material, null, true);
        Utilities.DrawLines(center + new Vector3(-multiplierSize, multiplierSize, multiplierSize), center + new Vector3(multiplierSize, -multiplierSize, -multiplierSize), width, material, null, true);*/


        //Utilities.DrawLines(new Vector3(-multiplierSize, -multiplierSize, 0), new Vector3(multiplierSize, multiplierSize, 0), width, material, xSymbol.transform, false);
        //Utilities.DrawLines(new Vector3(-multiplierSize, multiplierSize, 0), new Vector3(multiplierSize, -multiplierSize, 0), width, material, xSymbol.transform, false);

        float thickness = 0.025f;
        float width = 0.1f;
        float middleDiagonalRadius = width / Mathf.Sqrt(2);

        float a = 4;
        float b = 4 * width;
        float c = Mathf.Pow(width, 2) - Mathf.Pow(size - middleDiagonalRadius, 2) - Mathf.Pow(size - middleDiagonalRadius, 2);
        float hypLengthOnePart = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a); // (2x+width)^2-(width^2+width^2) = 0

        //Utilities.DrawLines(new Vector3(middleDiagonalRadius, 0f, 0f), new Vector3(hypLengthOnePart / Mathf.Sqrt(2) + middleDiagonalRadius, hypLengthOnePart / Mathf.Sqrt(2), 0f), thickness, material, xSymbol.transform, false);
        //Utilities.DrawLines(new Vector3(hypLengthOnePart / Mathf.Sqrt(2) + middleDiagonalRadius, hypLengthOnePart / Mathf.Sqrt(2), 0f), new Vector3(hypLengthOnePart / Mathf.Sqrt(2), hypLengthOnePart / Mathf.Sqrt(2) + middleDiagonalRadius, 0f), thickness, material, xSymbol.transform, false);
        //Utilities.DrawLines(new Vector3(hypLengthOnePart / Mathf.Sqrt(2), hypLengthOnePart / Mathf.Sqrt(2) + middleDiagonalRadius, 0f), new Vector3(0f, middleDiagonalRadius, 0f), thickness, material, xSymbol.transform, false);

        //Vector3 startPosition = new Vector3(middleDiagonalRadius, 0f, 0f);
        Vector3[] vertexPosition = new Vector3[] { new Vector3(hypLengthOnePart / Mathf.Sqrt(2) + middleDiagonalRadius, hypLengthOnePart / Mathf.Sqrt(2), 0f), new Vector3(hypLengthOnePart / Mathf.Sqrt(2), hypLengthOnePart / Mathf.Sqrt(2) + middleDiagonalRadius, 0f), new Vector3(0f, middleDiagonalRadius, 0f) };


        int numberOfXCorners = 4;
        float angleBetween = 360f / numberOfXCorners;

        Vector3[] vertexPoint = new Vector3[numberOfXCorners * vertexPosition.Length];
        //vertexPoint[0] = startPosition;
        /*for (int i = 0; i < numberOfXCorners; i++) {
            Vector3 angle = new Vector3(0f, 0f, angleBetween * i);
            for (int j = 0; j < vertexPosition.Length - 1; j++) {
                a++;
                Utilities.DrawLine(rotatePointAroundPivot(vertexPosition[j], transform.position, angle), rotatePointAroundPivot(vertexPosition[j + 1], transform.position, angle), thickness, material, xSymbol.transform, false);
            }
        }*/

        for (int i = 0; i < numberOfXCorners; i++) {
            Vector3 angle = new Vector3(0f, 0f, angleBetween * i);
            for (int j = 0; j < vertexPosition.Length; j++) {
                vertexPoint[j + (i * vertexPosition.Length)] = rotatePointAroundPivot(vertexPosition[j], centerOfCube, angle);
            }
        }
        Utilities.DrawLines(vertexPoint, thickness, GlobalVar.crossMaterial, true, xSymbol.transform, false);

        //Utilities.DrawLines(new Vector3(middleDiagonalRadius, 0f, 0f), new Vector3(0f, middleDiagonalRadius, 0f), thickness, material, xSymbol.transform, false);

        boxObject.GetComponent<BoxScript>().BoxStatus = 1;
        xSymbol.AddComponent<NoughtsAndCrosses>();
        xSymbol.GetComponent<NoughtsAndCrosses>().Layer = layer;
        xSymbol.GetComponent<NoughtsAndCrosses>().isCross = true;
    }

    public static void CreateOSymbol(GameObject boxObject, Vector3 position, Vector3 centerOfCube, float size, int layer)
    {
        //GameObject oSymbol = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //oSymbol.name = "OSymbol";
        GameObject oSymbol = new GameObject("OSymbol");
        oSymbol.transform.position = position;
        //oSymbol.transform.localScale = new Vector3(size * 0.8f, size * 0.8f, size * 0.8f);
        //oSymbol.GetComponent<MeshRenderer>().material = material;

        float thickness = 0.04f;
        int pointCount = 40;
        float radius = size / 2f;

        Vector3[] points = new Vector3[pointCount];

        for (int i = 0; i < points.Length; i++) {
            float radians = Mathf.Deg2Rad * (i * 360f / pointCount);
            points[i] = new Vector3(Mathf.Sin(radians) * radius, Mathf.Cos(radians) * radius, 0f);
        }

        Utilities.DrawLines(points, thickness, GlobalVar.noughtMaterial, true, oSymbol.transform, false);

        boxObject.GetComponent<BoxScript>().BoxStatus = 2;
        oSymbol.AddComponent<NoughtsAndCrosses>();
        oSymbol.GetComponent<NoughtsAndCrosses>().Layer = layer;
        oSymbol.GetComponent<NoughtsAndCrosses>().isCross = false;
    }

    private static Vector3 rotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }


}


