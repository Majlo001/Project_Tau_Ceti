using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public GameObject tooltip;
    private bool isMouseOver;
    private float delay = 1f;
    private Coroutine tooltipCoroutine;
    private Transform childTransform = null;

    private void Start() {
        if (transform.childCount > 0) {
            childTransform = transform.GetChild(0);
        }
        HideTooltip(true);
    }

    private void Update() {
        if (transform.childCount == 0) {
            HideTooltip(true);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (transform.childCount > 0) {
            isMouseOver = true;
            ShowTooltipDelayed(delay);
            tooltipCoroutine = StartCoroutine(ShowTooltipDelayed(delay));
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        isMouseOver = false;
        if (tooltipCoroutine != null) {
            StopCoroutine(tooltipCoroutine);
        }
        HideTooltip();
    }

    private void ShowTooltip() {
        tooltip.GetComponent<NewTooltip>().ShowTooltip(true);
    }

    private void HideTooltip(bool isFirstTime = false) {
        tooltip.GetComponent<NewTooltip>().ShowTooltip(false, isFirstTime);
    }

    private IEnumerator ShowTooltipDelayed(float delay) {
        float startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup < startTime + delay) {
            yield return null;
            if (!isMouseOver)
                yield break;
        }

        if (isMouseOver) {
            ShowTooltip();
        }
    }
}
