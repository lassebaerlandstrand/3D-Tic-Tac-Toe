using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoxSelector : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] private GameObject boxHighlight;

    private GameObject previousHover;
    private int layerMask;

    public Material testMaterial;

    private void Awake()
    {
        GlobalVar.TurnIndicator = 0;
        mainCamera = Camera.main;
        layerMask = 1 << GlobalVar.layerPlayerSelect;
        GlobalVar.boxHighlight = boxHighlight;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GlobalVar.gameCompleted) {
            if (Input.GetKeyDown(InputSystem.keybinds["Click"]) && GlobalVar.TurnIndicator % GlobalVar.turnIndex.Count == GlobalVar.turnIndex["Player"]) {
                RaycastHit raycastHit = new RaycastHit();
                bool hit = Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out raycastHit, Mathf.Infinity, layerMask);
                if (hit && raycastHit.transform.gameObject.GetComponent<BoxScript>().isEmpty()) {
                    createSymbol(raycastHit.transform.gameObject);
                    WinOrLoss.checkMatch(raycastHit.transform.gameObject);
                    GlobalVar.TurnIndicator++;
                }
            }
            hover();
        }
    }

    private void createSymbol(GameObject hitObject)
    {
        int layer = hitObject.GetComponent<BoxScript>().Layer;

        if (GlobalVar.TurnIndicator % 2 == 0) {
            Utilities.CreateXSymbol(hitObject, hitObject.transform.position, transform.position, hitObject.GetComponent<BoxCollider>().size.x * (1 / Mathf.Sqrt(2)), layer);
        } else {
            Utilities.CreateOSymbol(hitObject, hitObject.transform.position, transform.position, hitObject.GetComponent<BoxCollider>().size.x * 0.8f, layer);
        }
    }

    private void hover()
    {
        RaycastHit raycastHit = new RaycastHit();
        bool hit = Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out raycastHit, Mathf.Infinity, layerMask);
        if (hit) {
            GameObject rayCastObject = raycastHit.transform.gameObject;
            if (!ReferenceEquals(rayCastObject, previousHover)) {
                boxHighlight.SetActive(true);
                boxHighlight.transform.position = rayCastObject.transform.position;
            }
        } else if (boxHighlight.activeSelf) {
            boxHighlight.SetActive(false);
        }

        previousHover = hit ? raycastHit.transform.gameObject : null;
    }

}
