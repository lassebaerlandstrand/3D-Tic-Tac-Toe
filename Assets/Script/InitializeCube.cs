using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class InitializeCube : MonoBehaviour
{
    [SerializeField] private Material lineMaterial;
    [SerializeField] private Material lineMaterialEmissionOff;
    [SerializeField] private Material crossMaterial;
    [SerializeField] private Material noughtMaterial;
    [SerializeField] private Material symbolEmissionOff;
    [SerializeField] private GameObject highlightObject;

    private GameObject boxColliderParent;
    private int layerMask = (1 << GlobalVar.layerPlayerSelect) | (1 << GlobalVar.layerPlayerNotSelect);

    // Start is called before the first frame update
    void Awake()
    {
        GlobalVar.lineMaterial = lineMaterial;
        GlobalVar.lineMaterialEmissionOff = lineMaterialEmissionOff;
        GlobalVar.crossMaterial = crossMaterial;
        GlobalVar.noughtMaterial = noughtMaterial;
        GlobalVar.symbolEmissionOff = symbolEmissionOff;
        GlobalVar.gridBoxes = new BoxScript[GlobalVar.dimensions, GlobalVar.dimensions, GlobalVar.dimensions];
        GlobalVar.OpenBoxesInitialize = (int)Mathf.Pow(GlobalVar.dimensions, 3);
       
        boxColliderParent = new GameObject("Box Collider Parent");
        boxColliderParent.transform.parent = gameObject.transform;

        Layers.listOfLayers = new Layers.layerInformation[Mathf.CeilToInt(GlobalVar.dimensions / 2f)];
        for(int i = 0; i < Layers.listOfLayers.Length; i++) {
            GameObject newLayer = new GameObject("Layer " + (i + 1));
            newLayer.transform.parent = boxColliderParent.transform;

            Material crossMaterialClone = new Material(crossMaterial);
            Material noughtMaterialClone = new Material(noughtMaterial);

            Layers.listOfLayers[i].layerObject = newLayer;
        }

        initializeBoxes(GlobalVar.dimensions);

        highlightObject.transform.localScale = new Vector3(GlobalVar.size, GlobalVar.size, GlobalVar.size);

    }

    private void Start()
    {
        GlobalVar.boxesInitializedEvent();
    }

    // Draw the cube in lines, the most efficient way, but not so customizable/configurable
    private void initializeCubeLines(int dimensionSize)
    {

        float offset = -(dimensionSize / 2f);
        float width = 0.025f;
        GameObject lineParent = new GameObject("LineParent");
        lineParent.transform.parent = gameObject.transform;

        for (int i = 0; i < dimensionSize + 1; i++) {
            for (int j = 0; j < dimensionSize + 1; j++) {

                float a = i * GlobalVar.size + offset;
                float b = j * GlobalVar.size + offset;

                // X
                Utilities.DrawLine(new Vector3(a, b, offset), new Vector3(a, b, dimensionSize + offset), width, lineMaterial, lineParent.transform);
                Utilities.DrawLine(new Vector3(b, offset, a), new Vector3(b, dimensionSize + offset, a), width, lineMaterial, lineParent.transform);

                // Y
                Utilities.DrawLine(new Vector3(offset, a, b), new Vector3(dimensionSize + offset, a, b), width, lineMaterial, lineParent.transform);
                Utilities.DrawLine(new Vector3(b, a, offset), new Vector3(b, a, dimensionSize + offset), width, lineMaterial, lineParent.transform);

                // Z
                //Utilities.DrawLine(new Vector3(offset, b, a), new Vector3(dimensionSize + offset, b, a), width, lineMaterial, lineParent.transform);
                //Utilities.DrawLine(new Vector3(b, offset, a), new Vector3(b, dimensionSize + offset, a), width, lineMaterial, lineParent.transform);

            }
        }
    }

    // Initialize boxes with layers
    private void initializeBoxes(int dimensionSize)
    {
        int maxLayer = Mathf.CeilToInt(GlobalVar.dimensions / 2f);
        float oddOrEvenExtra = dimensionSize % 2 == 0 ? 1 : 0;
        float offset = -(dimensionSize / 2f);
        int nameIndex = 0;

        for (int j = 0; j < dimensionSize; j++) {
            for (int k = 0; k < dimensionSize; k++) {
                for (int l = 0; l < dimensionSize; l++) {
                    nameIndex++;
                    int layerBox = getLayer(dimensionSize, j, k, l);
                    GameObject box = createBoxes(new Vector3(GlobalVar.size * l + offset, GlobalVar.size * k + offset, GlobalVar.size * j + offset), Layers.listOfLayers[layerBox].layerObject.transform, GlobalVar.size, lineMaterial, layerBox, nameIndex.ToString());
                    GlobalVar.gridBoxes[j, k, l] = box.GetComponent<BoxScript>();
                }
            }
        }
    }

    private int getLayer(int dimensionSize, int x, int y, int z)
    {
        float centerPoint = (dimensionSize - 1) / 2f;
        
        float distanceOfX = Mathf.Abs(centerPoint - x);
        float distanceOfY = Mathf.Abs(centerPoint - y);
        float distanceOfZ = Mathf.Abs(centerPoint - z);

        return Mathf.FloorToInt(centerPoint) - (int)Mathf.Abs(centerPoint - Mathf.Max(distanceOfZ, Mathf.Max(distanceOfX, distanceOfY)));
    }

    private GameObject createBoxes(Vector3 position, Transform parent, float size, Material material, int layer, string name)
    {
        GameObject box = new GameObject("Box" + name);
        box.transform.parent = parent;
        box.transform.position = position + new Vector3(size / 2f, size / 2f, size / 2f);

        box.AddComponent<BoxScript>();
        BoxScript boxScript = box.GetComponent<BoxScript>();
        boxScript.Layer = layer;

        float width = 0.025f;

        List<List<Vector3>> linePositions = new List<List<Vector3>>();
        List<List<Vector3>> lineDrawnPositions = new List<List<Vector3>>();

        for (int i = 0; i < 3; i++) {
            for (int j = -1; j <= 1; j += 2) {
                Vector3 direction = Vector3.zero;
                direction[i] = j;
                bool hit = Physics.Raycast(box.transform.position, direction, out RaycastHit raycastHit, Mathf.Infinity, layerMask);
                if (hit) {
                    BoxScript neighbourBox = raycastHit.collider.gameObject.GetComponent<BoxScript>();

                    // A line could be included in two seperate faces therefore we must loop through, to place them in the correct face
                    for (int k = 0; k < neighbourBox.linesFaces[-direction].Count; k++) {
                        GameObject line = neighbourBox.linesFaces[-direction][k];
                        LineRenderer lr = line.GetComponent<LineRenderer>();
                        LineScript ls = line.GetComponent<LineScript>();
                        lineDrawnPositions.Add(new List<Vector3>() { lr.GetPosition(1), lr.GetPosition(0) });
                        
                        // Place in correct face
                        Vector3 difference = lr.GetPosition(1) - lr.GetPosition(0);
                        for (int l = 0; l < 3; l++) {
                            if (difference[l] == 0f) {
                                Vector3 directionFace = Vector3.zero;
                                directionFace[l] = Mathf.Sign((lr.GetPosition(0) - box.transform.position)[l]);
                                if (boxScript.linesFaces.ContainsKey(directionFace)) {
                                    boxScript.linesFaces[directionFace].Add(line);
                                } else {
                                    boxScript.linesFaces.Add(directionFace, new List<GameObject>() { line });
                                }
                            }
                        }

                        // Shared material
                        if(neighbourBox.Layer != layer) {
                            ls.Layer = (int)(layer + neighbourBox.Layer);
                        }
                    }
                }
            }
        }

        // Find line positions
        for(int i = 0; i < 4; i++) {
            Vector3 startExtra = Vector3.zero;
            if (i != 3) {
                startExtra = new Vector3(size, size, size);
                startExtra[i] = 0;
            }
            Vector3 vertexStart = position + startExtra;
            for(int j = 0; j < 3; j++) {
                Vector3 addedPosition = Vector3.zero;
                addedPosition[j] = size * (j != i && i != 3 ? -1 : 1);
                linePositions.Add(new List<Vector3>() { vertexStart, vertexStart + addedPosition });
            }
        }

        for (int i = 0; i < lineDrawnPositions.Count; i++) {
            for(int j = 0; j < linePositions.Count; j++) {
                if((lineDrawnPositions[i][0] == linePositions[j][0] && lineDrawnPositions[i][1] == linePositions[j][1]) || (lineDrawnPositions[i][0] == linePositions[j][1] && lineDrawnPositions[i][1] == linePositions[j][0])) { // TODO: Make a more efficient test where order does not matter
                    linePositions.RemoveAt(j);
                    break;
                }
            }
        }

        for (int i = 0; i < linePositions.Count; i++) {
            GameObject line = Utilities.DrawLine(linePositions[i][0], linePositions[i][1], width, lineMaterial, box.transform, true, true);
            line.GetComponent<LineScript>().Layer = layer * 2;
            LineRenderer lr = line.GetComponent<LineRenderer>();
            Vector3 difference = linePositions[i][1] - linePositions[i][0];
            for (int j = 0; j < 3; j++) {
                if (difference[j] == 0f) {
                    Vector3 direction = Vector3.zero;
                    direction[j] = Mathf.Sign((linePositions[i][0] - box.transform.position)[j]);
                    if (boxScript.linesFaces.ContainsKey(direction)) {
                        boxScript.linesFaces[direction].Add(line);
                    } else {
                        boxScript.linesFaces.Add(direction, new List<GameObject>() { line });
                    }
                }
            }
        }

        box.layer = GlobalVar.layerPlayerSelect;

        box.AddComponent<BoxCollider>();
        BoxCollider boxCollider = box.GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(size * 0.9999f, size * 0.9999f, size * 0.9999f); // A little bit smaller, since raycasts hits corner of cube

            

        return box;
    }

    private GameObject oldCreateBoxes(Vector3 position, Transform parent, float size, Material material, int layer, int nameIndex)
    {
        GameObject box = new GameObject("Box" + nameIndex.ToString());
        box.transform.parent = parent;
        box.transform.position = position + new Vector3(size / 2f, size / 2f, size / 2f);

        box.AddComponent<BoxScript>();
        BoxScript boxScript = box.GetComponent<BoxScript>();
        boxScript.Layer = layer;
        float width = 0.025f;

        GameObject horzDownBack = Utilities.DrawLine(position, position + new Vector3(size, 0, 0), width, material, box.transform, true, true);
        GameObject vertSideLeftBack = Utilities.DrawLine(position, position + new Vector3(0, size, 0), width, material, box.transform, true, true);
        GameObject horzDownMiddleLeft = Utilities.DrawLine(position, position + new Vector3(0, 0, size), width, material, box.transform, true, true);

        Vector3 vertexStart1 = position + new Vector3(size, size, 0);
        GameObject horzTopBack = Utilities.DrawLine(vertexStart1, vertexStart1 + new Vector3(-size, 0, 0), width, material, box.transform, true, true);
        GameObject vertSideRightBack = Utilities.DrawLine(vertexStart1, vertexStart1 + new Vector3(0, -size, 0), width, material, box.transform, true, true);
        GameObject horzTopMiddleRight = Utilities.DrawLine(vertexStart1, vertexStart1 + new Vector3(0, 0, size), width, material, box.transform, true, true);

        Vector3 vertexStart2 = position + new Vector3(size, 0, size);
        GameObject horzDownFront = Utilities.DrawLine(vertexStart2, vertexStart2 + new Vector3(-size, 0, 0), width, material, box.transform, true, true);
        GameObject vertSideRightFront = Utilities.DrawLine(vertexStart2, vertexStart2 + new Vector3(0, size, 0), width, material, box.transform, true, true);
        GameObject horzDownMiddleRight = Utilities.DrawLine(vertexStart2, vertexStart2 + new Vector3(0, 0, -size), width, material, box.transform, true, true);

        Vector3 vertexStart3 = position + new Vector3(0, size, size);
        GameObject horzTopFront = Utilities.DrawLine(vertexStart3, vertexStart3 + new Vector3(size, 0, 0), width, material, box.transform, true, true);
        GameObject vertSideLeftFront = Utilities.DrawLine(vertexStart3, vertexStart3 + new Vector3(0, -size, 0), width, material, box.transform, true, true);
        GameObject horzTopMiddleLeft = Utilities.DrawLine(vertexStart3, vertexStart3 + new Vector3(0, 0, -size), width, material, box.transform, true, true);

        box.layer = GlobalVar.layerPlayerSelect;

        box.AddComponent<BoxCollider>();
        BoxCollider boxCollider = box.GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(size * 0.9999f, size * 0.9999f, size * 0.9999f);

        return box;
    }

    public int getLayerSize()
    {
        return Mathf.CeilToInt(GlobalVar.dimensions / 2f);
    }

    public void showLayer(float layerActive)
    {
        for(int i = 0; i < Layers.listOfLayers.Length; i++) {
            for (int j = 0; j < Layers.listOfLayers[i].layerObject.transform.childCount; j++) {
                Layers.listOfLayers[i].layerObject.transform.GetChild(j).gameObject.layer = (layerActive == 0 || i == layerActive - 1) ? GlobalVar.layerPlayerSelect : GlobalVar.layerPlayerNotSelect;
            }
        }
        GlobalVar.LayerIndicator = (int)layerActive;
    }
}