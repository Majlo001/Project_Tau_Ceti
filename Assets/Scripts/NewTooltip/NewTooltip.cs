using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewTooltip : MonoBehaviour{
    public Text headerField;
    public Text rarityField;
    public Text contentField;
    public Text statsField;

    //TODO: levelField


    private RectTransform rectTransform;
    private Canvas canvas;

    private void Start() {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void SetText(string header, string rarity, string content, string stats) {
        if (string.IsNullOrEmpty(header)) {
            headerField.gameObject.SetActive(false);
        }
        else {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        if (string.IsNullOrEmpty(rarity)) {
            rarityField.gameObject.SetActive(false);
        }
        else {
            rarityField.gameObject.SetActive(true);
            rarityField.text = rarity;
        }

        if (string.IsNullOrEmpty(content)) {
            contentField.gameObject.SetActive(false);
        }
        else {
            contentField.gameObject.SetActive(true);
            contentField.text = content;
        }

        if (string.IsNullOrEmpty(stats)) {
            statsField.gameObject.SetActive(false);
        }
        else {
            statsField.gameObject.SetActive(true);
            statsField.text = stats;
        }
    }

    public void ShowTooltip(bool show) {
        gameObject.SetActive(show);
    }
}
