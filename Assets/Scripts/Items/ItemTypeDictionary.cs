using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


/*
*  ItemTypeDictionary
*  This class is used to store item types in a dictionary.
*  
*  Usage:
*  string weaponType = ItemTypeDictionary.Instance.itemType[0];
*  string bootsType = ItemTypeDictionary.Instance.itemType[1];
*
*  string itemTypeValue = "Boots";
*  int itemTypeKey = ItemTypeDictionary.Instance.GetItemTypeByKey(itemTypeValue);
*  Debug.Log("Item type key for value '" + itemTypeValue + "': " + itemTypeKey);
*/

public class ItemTypeDictionary : MonoBehaviour {
    private static ItemTypeDictionary instance;
    public static ItemTypeDictionary Instance { get { return instance; } }

    public Dictionary<int, string> itemType;
    public Dictionary<string, int> itemTypeReverse;


    public Dictionary<string, string> weaponStatsDictionary;




    public int[] itemTypeWeapons = new int[] { 0 };
    public int[] itemTypeShields = new int[] { 5 };
    public int[] itemTypeArmour = new int[] { 1, 2, 3, 4 };
    public int[] itemTypeConsumables = new int[] { 10, 11, 12 };
    public int[] itemTypeMaterials = new int[] { 13 };
    public int[] itemTypeQuestItems = new int[] { 14 };
 

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        InitDictionary();
        InitWeaponStatsDictionary();
    }

    private void InitWeaponStatsDictionary(){
        weaponStatsDictionary = new Dictionary<string, string>();

        weaponStatsDictionary.Add("fireDamage", "Fire Damage");
        weaponStatsDictionary.Add("posionDamage", "Posion Damage");
        weaponStatsDictionary.Add("iceDamage", "Ice Damage");
        weaponStatsDictionary.Add("lightningDamage", "Lightning Damage");
        weaponStatsDictionary.Add("crushingDamage", "Crushing Damage");
        weaponStatsDictionary.Add("attackSpeed", "Attack Speed");
        weaponStatsDictionary.Add("critChance", "Critic Chance");
        weaponStatsDictionary.Add("critDamage", "Critic Damage");
    }

    private void InitDictionary() {
        itemType = new Dictionary<int, string>();
        itemTypeReverse = new Dictionary<string, int>();

        itemType.Add(0, "Weapon");
        itemType.Add(1, "Boots");
        itemType.Add(2, "Helmet");
        itemType.Add(3, "Chestplate");
        itemType.Add(4, "Leggings");
        itemType.Add(5, "Shield");

        itemType.Add(10, "Consumables");
        itemType.Add(11, "Food");
        itemType.Add(12, "Potion");
        itemType.Add(13, "Material");
        itemType.Add(14, "QuestItem");

        foreach (KeyValuePair<int, string> pair in itemType) {
            itemTypeReverse.Add(pair.Value, pair.Key);
        }
    }

    public int GetItemTypeByKey(string key) {
        int itemTypeKey;
        if (itemTypeReverse.TryGetValue(key, out itemTypeKey)) {
            return itemTypeKey;
        }
        else {
            Debug.LogError("Invalid item type key: " + key);
            return -1;
        }
    }

    public bool isWearableItem(int itemType) {
        if (itemTypeArmour.Contains(itemType) || itemTypeWeapons.Contains(itemType) || itemTypeShields.Contains(itemType))
            return true;

        return false;
    }
}