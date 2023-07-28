using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [ExecuteInEditMode()]
public class Tooltip : MonoBehaviour {
    
    public Text headerField;
    public Text rarityField;
    public Text contentField;
    public Text statsField;

    //TODO: levelField


    private Canvas canvas;
    private Vector3 slotPosition;
    private Vector3 slotPos;

    public LayoutElement layoutElement;

    public int characterWrapLimit;

    public RectTransform rectTransform;

    private bool isOpened;
    private bool canBeOpened;





    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void SetText(string header, string rarity, string content, string stats, Vector3 slotPosition, Transform slotTransform) {

        // Temporary solution
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

    public void SetPosition (Vector3 position, Transform slotTransform) {
        slotPos = position;
    }

    private void Update() {
        float tooltipWidth = layoutElement.transform.GetComponent<RectTransform>().rect.width * canvas.scaleFactor;
        float tooltipHeight = layoutElement.transform.GetComponent<RectTransform>().rect.height * canvas.scaleFactor;
        
        float slotWidth = 46.5f * canvas.scaleFactor;
        float slotHeight = 46.5f * canvas.scaleFactor;

        slotPosition = slotPos - new Vector3(-tooltipWidth/2f + slotWidth, tooltipHeight/2f + slotHeight, 0f);
        // Debug.Log("Position: " + pos + " tooltipWidth: " + tooltipWidth + " tooltipHeight: " + tooltipHeight);

        if (rectTransform.position != slotPosition && slotPosition != Vector3.zero) {
            rectTransform.position = slotPosition;
        }



        // Vector3 mousePosition = Input.mousePosition;
        // Vector3 tooltipPosition = mousePosition + new Vector3(10f, 0f, 0f);

        // float tooltipWidth = rectTransform.rect.width;
        // float tooltipHeight = rectTransform.rect.height;

        // float windowWidth = Screen.width;
        // float windowHeight = Screen.height;


        // Debug.Log("windowHeight: " + windowHeight + " widnowWidth: " + windowWidth);
        // Debug.Log("tooltipPosition.x: " + tooltipPosition.x + " tooltipPosition.y: " + tooltipPosition.y);
        // if (tooltipPosition.y - tooltipHeight < 0) {
        //     Debug.Log("DOWN");
        //     tooltipPosition.y = mousePosition.y + tooltipHeight - 10f;
        // }
    }
}
