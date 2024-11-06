

using UnityEngine;

public class Equipment : MonoBehaviour{

    private Item weaponSlot;
    Inventory inventory;
    
    void Start() {
        weaponSlot = null;
        inventory = GetComponent<Inventory>();
    }

    void OnGUI() {
        if (inventory.showInventory && !inventory.shifting && !inventory.stillNotCloseEnough) {
            
            if (weaponSlot != null && GUI.Button(new(Screen.width / 2 - 100, Screen.height / 2, 60, 30), weaponSlot.GetName())) {
                inventory.AddItem (weaponSlot);
                weaponSlot = null;
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