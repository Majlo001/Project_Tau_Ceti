using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour {

    private static TooltipSystem current;
    public Tooltip tooltip;

    public void Awake() {
        current = this;
        Hide();
    }

    public static void SetTootip(string header, string rarity, string content, string stats, Vector3 slotPosition, Transform slotTransform) {
        current.tooltip.SetText(header, rarity, content, stats, slotPosition, slotTransform);
        current.tooltip.SetPosition(slotPosition, slotTransform);
    }

    public static void Show() {
        current.tooltip.gameObject.SetActive(true);
    }
    public static void Hide() {
        current.tooltip.gameObject.SetActive(false);
    }
}
