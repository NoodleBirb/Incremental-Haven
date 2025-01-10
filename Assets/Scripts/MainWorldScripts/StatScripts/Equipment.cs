

using UnityEngine;

public class Equipment : MonoBehaviour{

    private static Item weaponSlot;
    Inventory inventory;
    
    void Start() {
        weaponSlot ??= null;
        inventory = GetComponent<Inventory>();
    }

    void OnGUI() {
        if (Inventory.showInventory && !Inventory.shifting && !inventory.stillNotCloseEnough) {
            
            // If the player clicks on the weapon currently equipped, send weapon to inventory
            if (weaponSlot != null && GUI.Button(new(Screen.width / 2 - 100, Screen.height / 2, 60, 30), weaponSlot.GetName())) {
                Inventory.AddItem (weaponSlot);
                weaponSlot = null;
                PlayerStatistics.UpdateStats();
            } else if (weaponSlot == null) {
                GUI.Box(new(Screen.width / 2 - 100, Screen.height / 2, 60, 30), "");
            }
        }
    }

    public static Item SwapWeapon(Item newWeapon) {
        Item tempItem = weaponSlot;
        weaponSlot = newWeapon;
        return tempItem;
    }
    public static Item GetWeaponSlot() {
        return weaponSlot;
    }

    
}