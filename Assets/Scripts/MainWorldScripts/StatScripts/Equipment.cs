

using UnityEngine;

public class Equipment : MonoBehaviour{

    private Item weaponSlot;
    Inventory inventory;
    PlayerStatistics playerStats;
    
    void Start() {
        weaponSlot = null;
        inventory = GetComponent<Inventory>();
        playerStats = GetComponent<PlayerStatistics>();
    }

    void OnGUI() {
        if (Inventory.showInventory && !inventory.shifting && !inventory.stillNotCloseEnough) {
            
            if (weaponSlot != null && GUI.Button(new(Screen.width / 2 - 100, Screen.height / 2, 60, 30), weaponSlot.GetName())) {
                inventory.AddItem (weaponSlot);
                weaponSlot = null;
                playerStats.UpdateStats();
            } else if (weaponSlot == null) {
                GUI.Box(new(Screen.width / 2 - 100, Screen.height / 2, 60, 30), "");
            }
        }
    }

    public Item SwapWeapon(Item newWeapon) {
        Item tempItem = weaponSlot;
        weaponSlot = newWeapon;
        return tempItem;
    }
    public Item GetWeaponSlot() {
        return weaponSlot;
    }

    
}