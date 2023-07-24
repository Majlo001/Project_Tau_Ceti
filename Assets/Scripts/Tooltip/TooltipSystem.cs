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

    public static void Show(string header, string rarity, string content, string stats) {
        current.tooltip.SetText(header, rarity, content, stats);
        current.tooltip.gameObject.SetActive(true);
    }
    public static void Hide() {
        current.tooltip.gameObject.SetActive(false);
    }
}
