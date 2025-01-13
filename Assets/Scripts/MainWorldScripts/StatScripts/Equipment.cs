

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

            // If the player clicks on the chestplate currently equipped, send chestplate to inventory
            if (weaponSlot != null && GUI.Button(new(Screen.width / 2 - 100, Screen.height / 2, 60, 30), weaponSlot.GetName())) {
                Inventory.AddItem (weaponSlot);
                weaponSlot = null;
                PlayerStatistics.UpdateStats();
            } else if (weaponSlot == null) {
                GUI.Box(new(Screen.width / 2 - 200, Screen.height / 2 - 50, 60, 30), "");
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
        return tempItem;
    }
    public static Item GetChestplate() {
        return chestplateSlot; 
    }
    public static Item SwapOffHand(Item newOffHand) {
        Item tempItem = offHandSlot;
        offHandSlot = newOffHand;
        return tempItem;
    }
    public static Item GetOffHand() {
        return offHandSlot; 
    }
    public static Item SwapBoots(Item newBoots) {
        Item tempItem = bootSlot;
        bootSlot = newBoots;
        return tempItem;
    }
    public static Item GetBoots() {
        return bootSlot; 
    }
    public static Item SwapLeggings(Item newLeggings) {
        Item tempItem = leggingsSlot;
        leggingsSlot = newLeggings;
        return tempItem;
    }
    public static Item GetLeggings() {
        return leggingsSlot; 
    }
    public static Item SwapHeadPiece(Item newHeadPiece) {
        Item tempItem = headPieceSlot;
        headPieceSlot = newHeadPiece;
        return tempItem;
    }
    public static Item GetHeadPiece() {
        return headPieceSlot; 
    }
    public static Item SwapNecklace(Item newNecklace) {
        Item tempItem = necklaceSlot;
        necklaceSlot = newNecklace;
        return tempItem;
    }
    public static Item GetNecklace() {
        return necklaceSlot; 
    }
    public static Item SwapGloves(Item newGloves) {
        Item tempItem = gloveSlot;
        gloveSlot = newGloves;
        return tempItem;
    }
    public static Item GetGloves() {
        return gloveSlot; 
    }

    
}