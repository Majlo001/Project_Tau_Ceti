using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NewTooltip : MonoBehaviour{
    public Text headerField;
    public Text rarityField;
    public Text contentField;
    public Text statsField;

    public Text levelField;
    public Text RangeField;
    public Text valueField;
    public Transform statsPanel;
    public GameObject statsPrefab;


    private CustomItem customItem;
    public RectTransform rectTransform;
    private Canvas canvas;
    private bool isTooltipOpen = false;
    private bool isChanged = false;
    public float tooltipOffset = 5000f;

    private float tooltipHeight;
    private float tooltipWidth;

    private EquipmentManager equipmentManager;


    private void Start() {
        canvas = GameObject.Find("Overlay").GetComponent<Canvas>();
    }

    public void SetTooltipItem(CustomItem item) {
        string header = item.item.itemName;
        string rarity = item.item.GetTooltipRarityText();
        string content = item.item.itemDescription;

        //TODO: change when upgraded items will be implemented
        string stats = item.item.GetTooltipStats();
        string value = item.item.GetItemVaule().ToString();
        string level = item.item.itemLevel.ToString();
        customItem = item;


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

        if (string.IsNullOrEmpty(level)) {
            levelField.gameObject.SetActive(false);
        }
        else {
            levelField.gameObject.SetActive(true);
            levelField.text = "Level: " + level;
        }

        if (string.IsNullOrEmpty(value)) {
            valueField.gameObject.SetActive(false);
        }
        else {
            valueField.gameObject.SetActive(true);
            valueField.text = "Value: " + value;
        }

        // if (string.IsNullOrEmpty(stats)) {
        //     statsField.gameObject.SetActive(false);
        // }
        // else {
        //     statsField.gameObject.SetActive(true);
        //     statsField.text = stats;
        // }
        statsField.gameObject.SetActive(false);
        
        if (statsPrefab != null) {
            SetStats(item);
        }
        isChanged = false;
    }

    public void SetStats(CustomItem item) {
        Stats itemStats = item.GetStats();
        if (itemStats == null) {
            Debug.Log("itemStats is null");
            return;
        }
        StatsData[] statsData = itemStats.GetStatsFields();
        string itemTypeString = ItemTypeDictionary.Instance.itemType[item.item.itemType];


        if (statsData != null) {
            foreach (StatsData stat in statsData) {
                GameObject statObject = Instantiate(statsPrefab, statsPanel);

                Text statName = statObject.transform.Find("StatName").GetComponent<Text>();
                Text statValue = statObject.transform.Find("StatValue").GetComponent<Text>();
                Text statChange = statObject.transform.Find("StatChange").GetComponent<Text>();

                statName.text = stat.statName;
                statValue.text = stat.statValue.ToString();
                
                if (stat.statValue is int)
                    statValue.text += "%";

                Debug.Log("itemTypeString: " + itemTypeString);
                CustomItem slotItem = equipmentManager.takeItem(itemTypeString);

                if (slotItem != null) {
                    Stats slotItemStats = slotItem.GetStats();
                    if (slotItemStats == null) {
                        Debug.Log("slotItemStats is null");
                        return;
                    }
                    StatsData[] slotStatsData = slotItemStats.GetStatsFields();

                }

            }
        }
        else {
            Debug.Log("StatsData is null");
        }

        Array.Clear(statsData, 0, statsData.Length);
    }


    public void ShowTooltip(bool show, bool isFirstTime = false) {
        isTooltipOpen = show;

        if (!show && !isFirstTime) {
            refeshPosition();
        }
        gameObject.SetActive(show);
    }

    public void refeshPosition() {
        Debug.Log("Refresh position");
        Vector3 position = rectTransform.localPosition;

        position.y = tooltipOffset - 80f;
        rectTransform.localPosition = position;
        isChanged = false;
    }

    private void Update() {
        if (isTooltipOpen && !isChanged) {
            tooltipHeight = rectTransform.sizeDelta.y;
            tooltipWidth = rectTransform.sizeDelta.x;

            if (tooltipHeight != 0) {
                Vector3 position = rectTransform.localPosition;
                position.y -= tooltipOffset;
                rectTransform.localPosition = position;


                Vector2 tooltipPositionGlobal = rectTransform.TransformPoint(rectTransform.rect.position);
                // Debug.Log("Tooltip Position: " + tooltipPositionGlobal + " tooltipHeight/2: " + tooltipHeight/2 + " dol: " + tooltipPositionGlobal.y);
                // Debug.Log("Mouse Position: " + Input.mousePosition + " obliczone: " + (tooltipPositionGlobal.y - Input.mousePosition.y));

                if (tooltipPositionGlobal.y < 0) {
                    position.y += Mathf.Floor(tooltipHeight + 161f);
                }

                if (tooltipWidth != 0 && (tooltipPositionGlobal.x - tooltipWidth > Screen.width)) {
                    position.x -= Mathf.Floor(tooltipWidth - 160f);
                }

                rectTransform.localPosition = position;
                isChanged = true;

                Debug.Log("Tooltip: " + headerField.text + " Position: " + tooltipPositionGlobal.y + " tooltipHeight: " + tooltipHeight + " ");
            }   
        }
    }
}
