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


    public RectTransform rectTransform;
    private Canvas canvas;
    private bool isTooltipOpen = false;
    private bool isChanged = false;
    public float tootlipOffset = 2000f;


    private void Start() {
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
        isTooltipOpen = show;
    }

    private void LateUpdate() {
        if (isTooltipOpen && !isChanged) {
            float tooltipHeight = rectTransform.sizeDelta.y;
            float tooltipWidth = rectTransform.sizeDelta.x;

            if (tooltipHeight != 0) {
                Vector3 position = rectTransform.localPosition;
                position.y -= tootlipOffset;

                Vector3 globalPosition = transform.parent.TransformPoint(position) * canvas.scaleFactor;
                // Debug.Log("Global position: " + globalPosition);

                if (globalPosition.y - (tooltipHeight/2f) < 0) {
                    position.y += Mathf.Floor(tooltipHeight + 161f);
                }

                if (tooltipWidth != 0 && (globalPosition.x - (tooltipWidth / 2f) > Screen.width)) {
                    position.x -= Mathf.Floor(tooltipWidth - 160f);
                }

                rectTransform.localPosition = position;
                isChanged = true;
            }   
        }
    }
}
