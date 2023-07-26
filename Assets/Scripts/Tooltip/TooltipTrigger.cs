using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{
    
    private DraggableItem draggableItem;
    public string header;
    public string rarity;
    [Multiline()] public string content;
    [Multiline()] public string stats;

    private bool isPointerOver;
    private Coroutine tooltipCoroutine;


    public void Start() {
        draggableItem = GetComponent<DraggableItem>();
        header = draggableItem.item.item.itemName;
        content = draggableItem.item.item.itemDescription;
        rarity = draggableItem.item.item.GetTooltipRarity();
        stats = draggableItem.item.item.GetTooltipStats();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Transform slotTransform = draggableItem.transform.parent;
        Vector3 slotPosition = draggableItem.transform.parent.position;

        isPointerOver = true;
        TooltipSystem.SetTootip(header, rarity, content, stats, slotPosition, slotTransform);
        tooltipCoroutine = StartCoroutine(ShowTooltipDelayed(1f));
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
            TooltipSystem.Show();
        }
    }
}
