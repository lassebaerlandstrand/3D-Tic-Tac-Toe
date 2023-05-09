using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowValueChange : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private SizeInitializer sizeInitializer;
    [SerializeField] private RectTransform arrowChild;
    [SerializeField] private float directionMultiplier = 1f;
    [SerializeField] private float unitsToMove = 100f;
    [SerializeField] private float duration = 0.5f;

    private Vector3 originalPosition;
    private Vector3 velocity;

    private Coroutine moveArrowCoroutine;


    // Start is called before the first frame update
    void Start()
    {
        originalPosition = arrowChild.anchoredPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(moveArrowCoroutine != null) StopCoroutine(moveArrowCoroutine);
        moveArrowCoroutine = StartCoroutine(moveArrow());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (moveArrowCoroutine != null) StopCoroutine(moveArrowCoroutine);
        moveArrowCoroutine = StartCoroutine(moveArrowBack());
    }

    IEnumerator moveArrow()
    {
        Vector3 startPosition = arrowChild.anchoredPosition;
        Vector3 target = new Vector3(originalPosition.x + (unitsToMove * directionMultiplier), originalPosition.y, originalPosition.z);
        float timeElapsed = 0;

        while (timeElapsed < duration) {
            arrowChild.anchoredPosition = Vector3.Lerp(startPosition, target, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator moveArrowBack()
    {
        Vector3 startPosition = arrowChild.anchoredPosition;
        float timeElapsed = 0;

        while (timeElapsed < duration) {
            arrowChild.anchoredPosition = Vector3.Lerp(startPosition, originalPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void modifySize(float modifier)
    {
        if(sizeInitializer.Size + (int)modifier <= sizeInitializer.maxDimensionSize && sizeInitializer.Size + (int)modifier >= sizeInitializer.minDimensionSize) {
            sizeInitializer.Size += (int)modifier;
        }
    }
}
