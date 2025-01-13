

using UnityEngine;

public class Equipment : MonoBehaviour{

    private static Item weaponSlot;
    private static Item chestplateSlot;
    private static Item offHandSlot;
    private static Item necklaceSlot;
    private static Item bootSlot;
    private static Item leggingsSlot;
    private static Item gloveSlot;
    private static Item headPieceSlot;
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
    public static Item SwapChestplate(Item newChestplate) {
        Item tempItem = chestplateSlot;
        chestplateSlot = newChestplate;
        return tempItem;; // placeholder
    }
    public static Item GetChestplate() {
        return chestplateSlot; // placeholder
    }
    public static Item SwapOffHand(Item newOffHand) {
        Item tempItem = offHandSlot;
        offHandSlot = newOffHand;
        return tempItem;
    }
    public static Item GetOffHand() {
        return offHandSlot; // placeholder
    }
    public static Item SwapBoots(Item newBoots) {
        return null; // placeholder
    }
    public static Item GetBoots() {
        return bootSlot; // placeholder
    }
    public static Item SwapLeggings(Item newLeggings) {
        return null; // placeholder
    }
    public static Item GetLeggings() {
        return leggingsSlot; // placeholder
    }
    public static Item SwapHeadPiece(Item newHeadPiece) {
        return null; // placeholder
    }
    public static Item GetHeadPiece() {
        return headPieceSlot; // placeholder
    }
    public static Item SwapNecklace(Item newNecklace) {
        return null; // placeholder
    }
    public static Item GetNecklace() {
        return necklaceSlot; // placeholder
    }
    public static Item SwapGloves(Item newGloves) {
        return null; // placeholder
    }
    public static Item GetGloves() {
        return gloveSlot; // placeholder
    }

    
}