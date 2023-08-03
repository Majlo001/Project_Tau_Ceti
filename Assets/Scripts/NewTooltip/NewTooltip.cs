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
    private EquipmentManager eqManager;
    private bool showDiff;



    public RectTransform rectTransform;
    private Canvas canvas;
    private bool isTooltipOpen = false;
    private bool canMakeRefresh = false;
    private bool isChanged = false;
    public float tooltipOffset = 5000f;

    private float tooltipHeight;
    private float tooltipWidth;



    private void Start() {
        canvas = GameObject.Find("Overlay").GetComponent<Canvas>();
    }

    public void SetTooltipItem(CustomItem item, EquipmentManager equipmentManager = null, bool showDifference = false) {
        customItem = item;
        eqManager = equipmentManager;
        showDiff = showDifference;

        string header = item.item.itemName;
        string rarity = item.item.GetTooltipRarityText();
        string content = item.item.itemDescription;

        //TODO: change when upgraded items will be implemented
        string range = "";
        string stats = item.item.GetTooltipStats();
        string value = item.item.GetItemVaule().ToString();
        string level = item.item.itemLevel.ToString();

        


        if (string.IsNullOrEmpty(header)) {
            headerField.gameObject.SetActive(false);
        }
        else {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }


        if (item.item.itemType == 0) {
            range = item.item.TakeRangeText();
        }
        if (string.IsNullOrEmpty(range)) {
            RangeField.gameObject.SetActive(false);
        }
        else {
            RangeField.gameObject.SetActive(true);
            RangeField.text = range;
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
            SetStats(item, equipmentManager, showDifference);
        } else {
            // statsPanel.gameObject.SetActive(false);
        }
        isChanged = false;
    }

    private void SetStats(CustomItem item, EquipmentManager equipmentManager, bool showDifference = false) {
        Stats itemStats = item.GetStats();
        if (itemStats == null) {
            // Debug.Log("itemStats is null - item: " + item.item.itemName);
            return;
        }
        StatsData[] statsData = itemStats.GetStatsFields();
        string itemTypeString = ItemTypeDictionary.Instance.itemType[item.item.itemType];


        if (statsData != null) {
            foreach (StatsData stat in statsData) {
                GameObject statObject = Instantiate(statsPrefab, statsPanel);

                Text statName = statObject.transform.Find("StatName").GetComponent<Text>();
                Text statValue = statObject.transform.Find("StatValue").GetComponent<Text>();
                Text statDifference = statObject.transform.Find("StatDifference").GetComponent<Text>();

                statName.text = stat.statName;
                statValue.text = stat.statValue.ToString();
                statDifference.text = "";
                
                if (stat.statValue is int)
                    statValue.text += "%";


                if (equipmentManager != null && showDifference) {
                    CustomItem slotItem = equipmentManager.takeItem(itemTypeString);
                    if (slotItem == null) {
                        statDifference.text = "+" + statValue.text;
                        continue;
                    }

                    Stats slotItemStats = slotItem.GetStats();
                    if (slotItemStats == null) {
                        statDifference.text = "+" + statValue.text;
                        continue;
                    }

                    StatsData[] slotStatsData = slotItemStats.GetStatsFields();
                    
                    if (slotStatsData != null) {
                        foreach (StatsData slotStat in slotStatsData) {
                            Debug.Log("We're here !");
                            if (slotStat.fieldName != stat.fieldName)
                                continue;

                            if (slotStat.statValue is int) {
                                int difference = (int)stat.statValue - (int)slotStat.statValue;
                                statDifference.text = difference.ToString();
                                if (difference > 0)
                                    statDifference.text = "<color=green>+" + difference.ToString() + "</color>";
                                else if (difference == 0)
                                    statDifference.text = "It works!";
                            }
                            else if (slotStat.statValue is float) {
                                float difference = (float)stat.statValue - (float)slotStat.statValue;
                                statDifference.text = difference.ToString();
                                if (difference > 0)
                                    statDifference.text = "<color=green>+" + difference.ToString() + "</color>";
                                else if (difference == 0)
                                    statDifference.text = "It works!";
                            }
                            break;
                        }
                    }
                }
            }
        }

        Array.Clear(statsData, 0, statsData.Length);
    }

    private void ClearStatsPanel() {
        int childCount = statsPanel.transform.childCount;
        if (childCount == 0)
            return;

        for (int i = childCount - 1; i >= 0; i--) {
            Transform child = statsPanel.transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }


    public void ShowTooltip(bool show, bool isFirstTime = false) {
        isTooltipOpen = show;

        if (!show && !isFirstTime) {
            RefeshPosition();
        }
        gameObject.SetActive(show);
    }

    private void RefeshPosition() {
        Vector3 position = rectTransform.localPosition;
        position.y = tooltipOffset - 80f;
        rectTransform.localPosition = position;
        isChanged = false;
        canMakeRefresh = true;
    }

    private void RefreshContent() {
        ClearStatsPanel();
        SetTooltipItem(customItem, eqManager, showDiff);
    }

    private void Update() {
        if (isTooltipOpen && !isChanged) {
            tooltipHeight = rectTransform.sizeDelta.y;
            tooltipWidth = rectTransform.sizeDelta.x;

            if (tooltipHeight != 0) {
                if (canMakeRefresh) {
                    RefreshContent();
                    canMakeRefresh = false;
                }

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

                // Debug.Log("Tooltip: " + headerField.text + " Position: " + tooltipPositionGlobal.y + " tooltipHeight: " + tooltipHeight + " ");
            }   
        }
    }
}
