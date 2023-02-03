using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testSideCube : MonoBehaviour
{

    public GameObject cube;
    public Vector3 direction;

    private void OnDisable()
    {
        BoxScript boxScript = cube.GetComponent<BoxScript>();
        for(int i = 0; i < boxScript.linesFaces[direction].Count; i++) {
            boxScript.linesFaces[direction][i].SetActive(false);
        }
    }

    private void OnEnable()
    {
        BoxScript boxScript = cube.GetComponent<BoxScript>();
        for (int i = 0; i < boxScript.linesFaces[direction].Count; i++) {
            boxScript.linesFaces[direction][i].SetActive(true);
        }
    }
}
