using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [ExecuteInEditMode()]
public class Tooltip : MonoBehaviour {
    
    public Text headerField;
    public Text contentField;
    public Text statsField;


    public LayoutElement layoutElement;

    public int characterWrapLimit;

    public RectTransform rectTransform;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string header, string content, string stats) {
        if (string.IsNullOrEmpty(header)) {
            headerField.gameObject.SetActive(false);
        }
        else {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
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

    private void Update() {
        if (Application.isEditor) {
            int headerLength = headerField.text.Length;
            int contentLength = contentField.text.Length;

            layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit);
        }

        Vector3 mousePosition = Input.mousePosition;
        Vector3 tooltipPosition = mousePosition + new Vector3(30f, -10f, 0f); // Przesunięcie tooltipu względem kursora

        // Sprawdź, czy tooltip znajduje się na prawej stronie ekranu
        if (tooltipPosition.x + rectTransform.rect.width > Screen.width)
        {
            tooltipPosition -= new Vector3(rectTransform.rect.width + 60f, 0f, 0f); // Przesunięcie tooltipu na lewo
        }

        rectTransform.position = tooltipPosition;
    }

}
