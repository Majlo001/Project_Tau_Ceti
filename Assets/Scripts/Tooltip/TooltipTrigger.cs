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
        tooltipCoroutine = StartCoroutine(ShowTooltipDelayed(1f));
        Debug.Log("OnPointerEnter");
    }
    public void OnPointerExit(PointerEventData eventData) {
        isPointerOver = false;
        Debug.Log("OnPointerExit");
        if (tooltipCoroutine != null) {
            StopCoroutine(tooltipCoroutine);
            tooltipCoroutine = null;
        }

        TooltipSystem.Hide();
    }

    private IEnumerator ShowTooltipDelayed(float delay) {
        Debug.Log("ShowTooltipDelayed");
        yield return new WaitForSeconds(delay);
        Debug.Log("Kurwa pokaż się do kurwy nędzy");

        if (isPointerOver) {
            TooltipSystem.Show(header, content, stats);
        }
    }
}
