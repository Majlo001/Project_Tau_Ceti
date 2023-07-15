using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{
    
    private DraggableItem draggableItem;
    public string header;
    [Multiline()] public string content;
    [Multiline()] public string stats;

    private bool isPointerOver;
    private Coroutine tooltipCoroutine;

    public void Start() {
        draggableItem = GetComponent<DraggableItem>();
        header = draggableItem.item.item.itemName;
        content = draggableItem.item.item.itemDescription;
        stats = draggableItem.item.item.GetTooltipStats();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        isPointerOver = true;
        tooltipCoroutine = StartCoroutine(ShowTooltipDelayed(0.5f));
    }

    public void OnPointerExit(PointerEventData eventData) {
        isPointerOver = false;
        if (tooltipCoroutine != null) {
            StopCoroutine(tooltipCoroutine);
            tooltipCoroutine = null;
        }

        TooltipSystem.Hide();
    }


    //TODO: Fade in / Fade out on tooltip
    private IEnumerator ShowTooltipDelayed(float delay) {
        float startTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup < startTime + delay) {
            yield return null;
            if (!isPointerOver)
                yield break;
        }

        if (isPointerOver) {
            TooltipSystem.Show(header, content, stats);
        }
    }
}
