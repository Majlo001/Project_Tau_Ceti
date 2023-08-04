using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NewTooltip : MonoBehaviour{
    public Text headerField;
    public Text rarityField;
    public Text contentField;
    public Text levelField;
    public Text rangeField;
    public Text rangeDifferenceField;
    public Text valueField;

    public Transform rangePanel;
    public Transform statsPanel;
    public GameObject statsPrefab;


    private CustomItem customItem;
    private EquipmentManager eqManager;
    private CustomItem tempSlotItem;
    private bool showDiff;



    public RectTransform rectTransform;
    private Canvas canvas;
    private bool isTooltipOpen = false;
    private bool isChanged = false;
    private bool executeOnNextFrame = false;
    public float tooltipOffset = 5000f;

    private float tooltipHeight;
    private float tooltipWidth;



    private void Start() {
        canvas = GameObject.Find("Overlay").GetComponent<Canvas>();
    }


    public void SetTooltipItem(CustomItem item, EquipmentManager equipmentManager = null, bool showDifference = false, bool clearStatsPanel = true) {
        customItem = item;
        eqManager = equipmentManager;
        showDiff = showDifference;

        if (clearStatsPanel)
            ClearStatsPanel();

        if (ItemTypeDictionary.Instance.isWearableItem(customItem.item.itemType))
            tempSlotItem = takeSlotItem();

        string header = item.item.itemName;
        string rarity = item.item.GetTooltipRarityText();
        string content = item.item.itemDescription;

        //TODO: change when upgraded items will be implemented
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


        SetRangeText();


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
        
        if (statsPrefab != null) {
            SetStats();
        } else {
            // statsPanel.gameObject.SetActive(false);
        }
        isChanged = false;
    }

    private void SetRangeText(){
        string range = "";

        if (ItemTypeDictionary.Instance.isWearableItem(customItem.item.itemType)) {
            range = customItem.item.GetRangeText();

            rangePanel.gameObject.SetActive(true);
            rangeField.text = range;

            if (showDiff) {
                rangeDifferenceField.gameObject.SetActive(true);
                rangeDifferenceField.text = SetRangeDifference();
            }
        }
        else
            rangePanel.gameObject.SetActive(false);
    }

    private string SetRangeDifference() {
        int itemRangeFirst = customItem.item.getRangeIndex(0);
        
        if (eqManager == null)
            return "<color=green>+ " + itemRangeFirst.ToString() + "</color>";

        CustomItem slotItem = eqManager.takeItem(ItemTypeDictionary.Instance.itemType[customItem.item.itemType]);
        if (slotItem == null)
            return "<color=green>+ " + itemRangeFirst.ToString() + "</color>";

        int slotItemFirst = slotItem.item.getRangeIndex(0);
        int slotRangeDifference = slotItemFirst - itemRangeFirst;
        string rangeDifference = "";

        if (slotRangeDifference < 0)
            rangeDifference = "<color=green>+ " + slotRangeDifference.ToString() + "</color>";
        else if (slotRangeDifference > 0)
            rangeDifference = "<color=red>- " + Math.Abs(slotRangeDifference).ToString() + "</color>";

        return rangeDifference;
    }

    private void SetStats() {
        Stats itemStats = customItem.GetStats();
        if (itemStats == null) {
            // Debug.Log("itemStats is null - item: " + item.item.itemName);
            return;
        }
        StatsData[] statsData = itemStats.GetStatsFields();
        string itemTypeString = ItemTypeDictionary.Instance.itemType[customItem.item.itemType];


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


                if (eqManager != null && showDiff) {
                    CustomItem slotItem = eqManager.takeItem(itemTypeString);
                    if (slotItem == null) {
                        statDifference.text = "<color=green>+ " + statValue.text + "</color>";
                        continue;
                    }

                    Stats slotItemStats = slotItem.GetStats();
                    if (slotItemStats == null) {
                        statDifference.text = "<color=green>+ " + statValue.text + "</color>";
                        continue;
                    }

                    StatsData[] slotStatsData = slotItemStats.GetStatsFields();
                    
                    if (slotStatsData != null) {
                        foreach (StatsData slotStat in slotStatsData) {
                            if (slotStat.fieldName != stat.fieldName)
                                continue;

                            if (slotStat.statValue is int) {
                                int difference = (int)stat.statValue - (int)slotStat.statValue;
                                statDifference.text = difference.ToString();
                                if (difference > 0)
                                    statDifference.text = "<color=green>+ " + difference.ToString() + "</color>";
                                else if (difference == 0)
                                    statDifference.text = "";
                                else
                                    statDifference.text = "<color=red>- " + Math.Abs(difference).ToString() + "</color>";
                            }
                            else if (slotStat.statValue is float) {
                                float difference = (float)Math.Round((float)stat.statValue - (float)slotStat.statValue, 2);
                                statDifference.text = difference.ToString();
                                if (difference > 0)
                                    statDifference.text = "<color=green>+ " + difference.ToString() + "</color>";
                                else if (difference == 0)
                                    statDifference.text = "";
                                else
                                    statDifference.text = "<color=red>- " + Math.Abs(difference).ToString() + "</color>";
                            }
                            break;
                        }
                    }
                }
            }
        }

        Array.Clear(statsData, 0, statsData.Length);
    }
    
    private CustomItem takeSlotItem() {
        if (eqManager != null) {
            tempSlotItem = eqManager.takeItem(ItemTypeDictionary.Instance.itemType[customItem.item.itemType]);
            return tempSlotItem;
        }

        return null;
    }

    private void ClearStatsPanel() {
        foreach (Transform child in statsPanel.transform) {
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
    }

    private void RefreshContent() {
        if (statsPrefab != null) {
            if (eqManager != null && showDiff) {
                ClearStatsPanel();
                SetRangeText();
                SetStats();
            }
        }
    }

    private void Update() {
        if (isTooltipOpen && !isChanged) {
            tooltipHeight = rectTransform.sizeDelta.y;
            tooltipWidth = rectTransform.sizeDelta.x;

            if (tooltipHeight != 0 && executeOnNextFrame) {
                if (ItemTypeDictionary.Instance.isWearableItem(customItem.item.itemType)) {
                    if (tempSlotItem != takeSlotItem()) {
                        RefreshContent();
                    }
                }
                tooltipHeight = rectTransform.sizeDelta.y;
                executeOnNextFrame = false;
                

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

                // Debug.Log("Tooltip: " + headerField.text + " Position: " + tooltipPositionGlobal.y + " tooltipHeight: " + tooltipHeight + " statsPanel: " + statsPanel.GetComponent<RectTransform>().rect.height);
            }

            if (tooltipHeight != 0)
                executeOnNextFrame = true;
        }
    }
}
