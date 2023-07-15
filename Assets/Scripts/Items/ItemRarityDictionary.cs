using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRarityDictionary : MonoBehaviour {
    private static ItemRarityDictionary instance;
    public static ItemRarityDictionary Instance { get { return instance; } }

    public string[] itemRarity = new string[6];
    public string[] itemColors = new string[6];
    public Dictionary<string, int> itemRarityLookup;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        InitializeArrays();
        BuildLookup();
    }

    private void InitializeArrays() {
        itemRarity[0] = "Common";
        itemRarity[1] = "Uncommon";
        itemRarity[2] = "Rare";
        itemRarity[3] = "Epic";
        itemRarity[4] = "Legendary";
        itemRarity[5] = "Mythical";

        itemColors[0] = "#FFFFFF";  // Biały
        itemColors[1] = "#00FF00";  // Zielony
        itemColors[2] = "#0000FF";  // Niebieski
        itemColors[3] = "#FF00FF";  // Magenta
        itemColors[4] = "#FFFF00";  // Żółty
        itemColors[5] = "#FF0000";  // Czerwony
    }

    private void BuildLookup() {
        itemRarityLookup = new Dictionary<string, int>();

        for (int i = 0; i < itemRarity.Length; i++)  {
            itemRarityLookup[itemRarity[i]] = i;
        }
    }
    public int GetItemTypeByKey(string key) {
        int itemTypeKey;
        if (itemRarityLookup.TryGetValue(key, out itemTypeKey)) {
            return itemTypeKey;
        }
        else {
            Debug.LogError("Invalid item rarity key: " + key);
            return -1;
        }
    }

    private Color HexToColor(string hex) {
        Color color = Color.white;
        ColorUtility.TryParseHtmlString(hex, out color);
        return color;
    }

    private Color RGBToColor(int r, int g, int b) {
        Color color = Color.white;
        ColorUtility.TryParseHtmlString(ColorUtility.ToHtmlStringRGB(new Color32((byte)r, (byte)g, (byte)b, 255)), out color);
        return color;
    }
}