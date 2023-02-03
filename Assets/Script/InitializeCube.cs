using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
//using Utilities;

public class InitializeCube : MonoBehaviour
{

    //[SerializeField] private int dimensions = 3;
    //[SerializeField] public float size = 1f;
    [SerializeField] private Material lineMaterial;
    [SerializeField] private Material lineMaterialEmissionOff;
    [SerializeField] private Material crossMaterial;
    [SerializeField] private Material noughtMaterial;
    [SerializeField] private Material symbolEmissionOff;
    [SerializeField] private GameObject highlightObject;

    private GameObject boxColliderParent;
    private int layerMask = (1 << GlobalVar.layerPlayerSelect) | (1 << GlobalVar.layerPlayerNotSelect);


    // Don't need gridBoxes anymore
    [HideInInspector] public GameObject[,,] gridBoxes;

    // Start is called before the first frame update
    void Awake()
    {
        //GlobalVar.dimensions = dimensions;

        GlobalVar.lineMaterial = lineMaterial;
        GlobalVar.lineMaterialEmissionOff = lineMaterialEmissionOff;
        GlobalVar.crossMaterial = crossMaterial;
        GlobalVar.noughtMaterial = noughtMaterial;
        GlobalVar.symbolEmissionOff = symbolEmissionOff;
        GlobalVar.gridBoxes = new BoxScript[GlobalVar.dimensions, GlobalVar.dimensions, GlobalVar.dimensions];
        GlobalVar.OpenBoxesInitialize = (int)Mathf.Pow(GlobalVar.dimensions, 3);
        //Debug.Log(GlobalVar.openBoxes);
       
        gridBoxes = new GameObject[GlobalVar.dimensions, GlobalVar.dimensions, GlobalVar.dimensions];
        boxColliderParent = new GameObject("Box Collider Parent");
        boxColliderParent.transform.parent = gameObject.transform;

        Layers.listOfLayers = new Layers.layerInformation[Mathf.CeilToInt(GlobalVar.dimensions / 2f)];
        for(int i = 0; i < Layers.listOfLayers.Length; i++) {
            GameObject newLayer = new GameObject("Layer " + (i + 1));
            newLayer.transform.parent = boxColliderParent.transform;

            Material crossMaterialClone = new Material(crossMaterial);
            Material noughtMaterialClone = new Material(noughtMaterial);

            Layers.listOfLayers[i].layerObject = newLayer;
            //Layers.listOfLayers[i].materialCross = crossMaterialClone;
            //Layers.listOfLayers[i].materialNought = noughtMaterialClone;
        }
        /*Layers.lineMaterials = new Layers.lineMaterialStruct[dimensions - ((dimensions % 2 == 0) ? 1 : 0)];
        for(int i = 0; i < Layers.lineMaterials.Length; i++) {
            Material lineMaterialClone = new Material(lineMaterial);
            //Material lineMaterialClone = lineMaterial;
            lineMaterialClone.name = (i/2f).ToString();
            Layers.lineMaterials[i].lineMaterial = lineMaterialClone;
        }*/
        //initializeCubes(dimensions);
        //initializeCubeLines(dimensions);
        //initializeBoxColliders(dimensions);

        initializeBoxes(GlobalVar.dimensions);
        //initializeCubeLines(dimensions);

        //distributeBoxes();

        highlightObject.transform.localScale = new Vector3(GlobalVar.size, GlobalVar.size, GlobalVar.size);

    }

    private void Start()
    {
        //showLayer(0f);
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

        //DrawLines(Vector3.zero, Vector3.one, material);
    }

    // Create boxes in 3 dimensions
    /*private void initializeBoxes(int dimensionSize)
    {
        float offset = -(dimensionSize / 2f);
        int nameIndex = 0;
        for (int i = 0; i < dimensionSize; i++) {
            for (int j = 0; j < dimensionSize; j++) {
                for (int k = 0; k < dimensionSize; k++) {
                    nameIndex++;
                    GameObject box = createBoxes(new Vector3(size * k + offset, size * j + offset, size * i + offset), boxColliderParent.transform, size, nameIndex);
                    gridBoxes[k, j, i] = box;
                }
            }
        }
    }*/

    // Only distributes boxes in a 2d plane, not 3d, therefore too inefficient
    /*private void distributeBoxes()
    {
        int centerIndex = dimensions / 2;
        for (int i = 0; i < dimensions; i++) {
            List<GameObject> centerBox = new List<GameObject>{ gridBoxes[centerIndex, centerIndex, i]};
            if (boxList.ElementAtOrDefault(0) == null)
                boxList.Add(centerBox);
            else
                boxList[0].AddRange(centerBox);
            for (int j = 1; j < Mathf.CeilToInt(dimensions / 2f); j++) {
                if (boxList.ElementAtOrDefault(j) == null) {
                    boxList.Add(getNeighbourBoxes2D(centerIndex, centerIndex, i, j));
                } else {
                    boxList[j].AddRange(getNeighbourBoxes2D(centerIndex, centerIndex, i, j));
                }
                
            }
        }

        Debug.Log(boxList.Count);
        Debug.Log(boxList[0].Count);

        string a = "";
        for(int i = 0; i < boxList.Count; i++) {
            for(int j = 0; j < boxList[i].Count; j++) {
                a += boxList[i][j].name;
            }
            Debug.Log(a);
            a = "";
        }
    }*/

    // Initialize boxes with layers
    private void initializeBoxes(int dimensionSize)
    {
        //int[,,] coordinates = new int[dimensionSize, dimensionSize, dimensionSize];
        int maxLayer = Mathf.CeilToInt(GlobalVar.dimensions / 2f);
        float oddOrEvenExtra = dimensionSize % 2 == 0 ? 1 : 0;
        float offset = -(dimensionSize / 2f);
        int nameIndex = 0;
        /*for (int layer = 0; layer < maxLayer; layer++) {
            GameObject newLayer = new GameObject("Layer " + (layer + 1));
            newLayer.transform.parent = boxColliderParent.transform;

            // Improve performance by having the same material (dynamic batching)
            // By making a new material for each layer, we are losing half the performance
            Material lineMaterialClone = new Material(lineMaterial);
            Material crossMaterialClone = new Material(crossMaterial);
            Material noughtMaterialClone = new Material(noughtMaterial);
            //Material lineMaterialClone = lineMaterial;
            //Material crossMaterialClone = crossMaterial;
            //Material noughtMaterialClone = noughtMaterial;
            Layers.listOfLayers[layer].layerObject = newLayer;
            Layers.listOfLayers[layer].materialLayer = lineMaterialClone;
            Layers.listOfLayers[layer].materialCross = crossMaterialClone;
            Layers.listOfLayers[layer].materialNought = noughtMaterialClone;


            int addedIndex = maxLayer - 1 - layer;
            float oneDimensionQuantity = layer * 2 + 1 + oddOrEvenExtra;

            for (int j = 0; j < oneDimensionQuantity; j++) {
                for (int k = 0; k < oneDimensionQuantity; k++) {
                    for (int l = 0; l < oneDimensionQuantity; l++) {
                        if (gridBoxes[l + addedIndex, k + addedIndex, j + addedIndex] == null) { // Check if the box has already been created
                            nameIndex++;
                            GameObject box = createBoxes(new Vector3(GlobalVar.size * (l + addedIndex) + offset, GlobalVar.size * (k + addedIndex) + offset, GlobalVar.size * (j + addedIndex) + offset), newLayer.transform, GlobalVar.size, lineMaterialClone, layer, nameIndex);
                            gridBoxes[l + addedIndex, k + addedIndex, j + addedIndex] = box;
                        }
                    }
                }
            }
        }*/

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

    /*private void initializeBoxColliders(int dimensionSize)
    {

        float offset = -(dimensionSize / 2f - (size / 2f));

        for (int i = 0; i < dimensionSize; i++) {
            for (int j = 0; j < dimensionSize; j++) {
                for (int k = 0; k < dimensionSize; k++) {
                    createRayCastBoxes(new Vector3(size * k + offset, size * j + offset, size * i + offset), boxColliderParent.transform, size);
                }
            }
        }
    }*/

    /*private float[] shiftArray(float[] array, int shiftNumber)
    {
        float[] temp = new float[array.Length];

        for(int i = 0; i < array.Length; i++) {
            int newIndex = (i + (array.Length - shiftNumber)) % array.Length;
            temp[newIndex] = array[i];
        }

        return temp;
    }*/

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
                            //lr.material = Layers.lineMaterials[(int)(layer + neighbourBox.Layer)].lineMaterial;
                            ls.Layer = (int)(layer + neighbourBox.Layer);
                        }
                        //line.SetActive(false);

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

        // Draw cube
        //List<List<Vector3>> notDrawnPositions = new List<List<Vector3>>();
        for (int i = 0; i < lineDrawnPositions.Count; i++) { // Probably not the most efficient
            for(int j = 0; j < linePositions.Count; j++) {
                if((lineDrawnPositions[i][0] == linePositions[j][0] && lineDrawnPositions[i][1] == linePositions[j][1]) || (lineDrawnPositions[i][0] == linePositions[j][1] && lineDrawnPositions[i][1] == linePositions[j][0])) { // TODO: Make a more efficient test where order does not matter
                    linePositions.RemoveAt(j);
                    break;
                }
            }
        }

        for (int i = 0; i < linePositions.Count; i++) {
            GameObject line = Utilities.DrawLine(linePositions[i][0], linePositions[i][1], width, /*Layers.lineMaterials[layer * 2].lineMaterial*/ lineMaterial, box.transform, true, true);
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

        // Testing
        /*
        boxScript.linesFaces.Add(new Vector3(0, 0, -1f), new GameObject[] { horzDownBack, vertSideLeftBack, horzTopBack, vertSideRightBack }); // front
        boxScript.linesFaces.Add(new Vector3(1f, 0, 0), new GameObject[] { vertSideRightBack, horzTopMiddleRight, vertSideRightFront, horzDownMiddleRight }); // right
        boxScript.linesFaces.Add(new Vector3(-1f, 0, 0), new GameObject[] { vertSideLeftBack, horzTopMiddleLeft, vertSideLeftFront, horzDownMiddleLeft }); // left
        boxScript.linesFaces.Add(new Vector3(0, 0, 1f), new GameObject[] { vertSideLeftFront, horzTopFront, horzDownFront, vertSideRightFront }); // back
        boxScript.linesFaces.Add(new Vector3(0, 1f, 0), new GameObject[] { horzTopBack, horzTopMiddleLeft, horzTopFront, horzTopMiddleRight }); // top
        boxScript.linesFaces.Add(new Vector3(0, -1f, 0), new GameObject[] { horzDownBack, horzDownMiddleLeft, horzDownFront, horzDownMiddleRight }); // bottom
        */

        box.layer = GlobalVar.layerPlayerSelect;

        box.AddComponent<BoxCollider>();
        BoxCollider boxCollider = box.GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(size * 0.9999f, size * 0.9999f, size * 0.9999f);

        return box;
    }

    /*private void createRayCastBoxes(Vector3 position, Transform parent, float size)
    {
        GameObject rayCastBox = new GameObject("RaycastBox");
        rayCastBox.transform.parent = parent;
        rayCastBox.transform.position = position;
        rayCastBox.AddComponent<BoxCollider>();
        BoxCollider boxCollider = rayCastBox.GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(size, size, size);
    }*/

    /*private List<GameObject> getNeighbourBoxes2D(int xIndex, int yIndex, int zIndex, int depth)
    {
        List<GameObject> neighbourBoxes = new List<GameObject>();
        for(int i = yIndex - depth; i <= yIndex + depth; i++)
        {
            neighbourBoxes.Add(gridBoxes[xIndex - depth, i, zIndex]); // Left
            neighbourBoxes.Add(gridBoxes[xIndex + depth, i, zIndex]); // Right
        }

        //Top & Bottom
        for(int i = xIndex - depth + 1; i <= xIndex + depth - 1; i++)
        {
            neighbourBoxes.Add(gridBoxes[i, yIndex + depth, zIndex]); // Top
            neighbourBoxes.Add(gridBoxes[i, yIndex - depth, zIndex]); // Bottom
        }
        return neighbourBoxes;
    }*/

    public int getLayerSize()
    {
        return Mathf.CeilToInt(GlobalVar.dimensions / 2f);
    }

    public void showLayer(float layerActive)
    {
        for(int i = 0; i < Layers.listOfLayers.Length; i++) {
            if(layerActive == 0 || i == layerActive - 1) {
                //Layers.listOfLayers[i].materialLayer.EnableKeyword("_EMISSION");
                //Layers.listOfLayers[i].materialCross.EnableKeyword("_EMISSION");
                //Layers.listOfLayers[i].materialNought.EnableKeyword("_EMISSION");

                for (int j = 0; j < Layers.listOfLayers[i].layerObject.transform.childCount; j++) {
                    Layers.listOfLayers[i].layerObject.transform.GetChild(j).gameObject.layer = GlobalVar.layerPlayerSelect;
                }
            } else {
                //Layers.listOfLayers[i].materialLayer.DisableKeyword("_EMISSION");
                //Layers.listOfLayers[i].materialCross.DisableKeyword("_EMISSION");
                //Layers.listOfLayers[i].materialNought.DisableKeyword("_EMISSION");
                for (int j = 0; j < Layers.listOfLayers[i].layerObject.transform.childCount; j++) {
                    Layers.listOfLayers[i].layerObject.transform.GetChild(j).gameObject.layer = GlobalVar.layerPlayerNotSelect;
                }
            }
        }

        /*
        for(int i = 0; i < Layers.lineMaterials.Length; i++) {
            if(layerActive == 0 || Mathf.Abs(2 * layerActive - 2 - i) <= 1) { // layerActive - 1 - i + (layerActive - 1)) <= 1
                Layers.lineMaterials[i].lineMaterial.EnableKeyword("_EMISSION");
            } else {
                Layers.lineMaterials[i].lineMaterial.DisableKeyword("_EMISSION");
            }
        }
        */
        GlobalVar.LayerIndicator = (int)layerActive;
    }
}