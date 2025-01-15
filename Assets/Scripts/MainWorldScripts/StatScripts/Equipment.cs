

using System;
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

    readonly Rect headPieceRect = new(Screen.width / 2 - 100, Screen.height / 2 - 200, 60, 30);
    readonly Rect chestplateRect = new(Screen.width / 2 - 100, Screen.height / 2 - 100, 60, 30);
    readonly Rect weaponRect = new(Screen.width / 2 - 100, Screen.height / 2, 60, 30);
    readonly Rect leggingsRect = new(Screen.width / 2 - 100, Screen.height / 2 + 100, 60, 30);
    

    readonly Rect necklaceRect = new(50, Screen.height / 2 - 155, 60, 30);
    readonly Rect offHandRect = new(50, Screen.height / 2 - 55, 60, 30);
    readonly Rect gloveRect = new(50, Screen.height / 2 + 55, 60, 30);
    readonly Rect bootRect = new(50, Screen.height / 2 + 155, 60, 30);

    private LineRenderer lineRenderer;
    private Vector3[] positions;
    Inventory inventory;
    GameObject player;
    GameObject head;
    GameObject chest;
    GameObject neck;
    GameObject rightFoot;
    GameObject rightArm;
    GameObject leftArm;
    GameObject leftLeg;
    void Start() {
        weaponSlot ??= null;
        inventory = GetComponent<Inventory>();
        player = GameObject.Find("player model");
        head = GameObject.Find("Paper Bag");
        chest = GameObject.Find("Torso");
        neck = GameObject.Find("Neck");
        rightFoot = GameObject.Find("Right Foot");
        rightArm = GameObject.Find("Right Arm");
        leftArm = GameObject.Find("Left Arm");
        leftLeg = GameObject.Find("Left Leg");

        // Get or add the LineRenderer component
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        positions = new Vector3[] {
            // Headpiece
            new(), head.transform.position,
            // Chest
            new(), chest.transform.position,
            // Weapon
            new(), leftArm.transform.position,
            // Leggings
            new(), leftLeg.transform.position,
            // Necklace
            new(), neck.transform.position,
            // Boots
            new(), rightFoot.transform.position,
            // Off Hand
            new(), rightArm.transform.position,
            // Gloves
            new(), rightArm.transform.position,      
        };

        // Configure the LineRenderer
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.positionCount = 16; // 8 lines = 16 points
        lineRenderer.useWorldSpace = false;
        lineRenderer.enabled = false;
        
        lineRenderer.SetPositions(positions);
    }

    void OnGUI() {
        if (Inventory.showInventory && !Inventory.shifting && !inventory.stillNotCloseEnough) {
            lineRenderer.enabled = true;
            // If the player clicks on the weapon currently equipped, send weapon to inventory
            if (weaponSlot != null && GUI.Button(weaponRect, weaponSlot.GetName())) {
                Inventory.AddItem (weaponSlot);
                weaponSlot = null;
                PlayerStatistics.UpdateStats();
            } else if (weaponSlot == null) {
                GUI.Box(weaponRect, "");
            }

            // If the player clicks on the chestplate currently equipped, send chestplate to inventory
            if (chestplateSlot != null && GUI.Button(chestplateRect, chestplateSlot.GetName())) {
                Inventory.AddItem (chestplateSlot);
                chestplateSlot = null;
                PlayerStatistics.UpdateStats();
            } else if (chestplateSlot == null) {
                GUI.Box(chestplateRect, "");
            }

            // If the player clicks on the offhand currently equipped, send offhand to inventory
            if (offHandSlot != null && GUI.Button(offHandRect, offHandSlot.GetName())) {
                Inventory.AddItem (offHandSlot);
                offHandSlot = null;
                PlayerStatistics.UpdateStats();
            } else if (offHandSlot == null) {
                GUI.Box(offHandRect, "");
            }

            // If the player clicks on the necklace currently equipped, send necklace to inventory
            if (necklaceSlot != null && GUI.Button(necklaceRect, chestplateSlot.GetName())) {
                Inventory.AddItem (necklaceSlot);
                necklaceSlot = null;
                PlayerStatistics.UpdateStats();
            } else if (necklaceSlot == null) {
                GUI.Box(necklaceRect, "");
            }

            // If the player clicks on the boot currently equipped, send boot to inventory
            if (bootSlot != null && GUI.Button(bootRect, chestplateSlot.GetName())) {
                Inventory.AddItem (bootSlot);
                bootSlot = null;
                PlayerStatistics.UpdateStats();
            } else if (bootSlot == null) {
                GUI.Box(bootRect, "");
            }

            // If the player clicks on the leggings currently equipped, send leggings to inventory
            if (leggingsSlot != null && GUI.Button(leggingsRect, chestplateSlot.GetName())) {
                Inventory.AddItem (leggingsSlot);
                leggingsSlot = null;
                PlayerStatistics.UpdateStats();
            } else if (leggingsSlot == null) {
                GUI.Box(leggingsRect, "");
            }

            // If the player clicks on the gloves currently equipped, send gloves to inventory
            if (gloveSlot != null && GUI.Button(gloveRect, gloveSlot.GetName())) {
                Inventory.AddItem (weaponSlot);
                gloveSlot = null;
                PlayerStatistics.UpdateStats();
            } else if (gloveSlot == null) {
                GUI.Box(gloveRect, "");
            }

            // If the player clicks on the headpiece currently equipped, send headpiece to inventory
            if (headPieceSlot != null && GUI.Button(headPieceRect, headPieceSlot.GetName())) {
                Inventory.AddItem (headPieceSlot);
                headPieceSlot = null;
                PlayerStatistics.UpdateStats();
            } else if (headPieceSlot == null) {
                GUI.Box(headPieceRect, "");
            }

        } else {
            Vector3 rightDir = player.transform.right;
            Vector3 forwardDir = player.transform.forward;
            
            lineRenderer.enabled = false;
            /* positions = new Vector3[] {
                // Headpiece
                player.transform.position - rightDir + new Vector3(0, 2f, 0), Camera.main.WorldToScreenPoint(head.transform.position),
                // Chest
                player.transform.position - rightDir + new Vector3(0, 1.5f, 0), Camera.main.WorldToScreenPoint(chest.transform.position),
                // Weapon
                player.transform.position + rightDir, Camera.main.WorldToScreenPoint(leftArm.transform.position),
                // Leggings
                player.transform.position + rightDir, Camera.main.WorldToScreenPoint(leftLeg.transform.position),
                // Necklace
                player.transform.position + rightDir + new Vector3(0, 1.8f, 0), Camera.main.WorldToScreenPoint(neck.transform.position),
                // Boots
                player.transform.position - rightDir + new Vector3(0, .1f, 0), Camera.main.WorldToScreenPoint(rightFoot.transform.position),
                // Off Hand
                player.transform.position + rightDir + new Vector3(0, 2f, 0), Camera.main.WorldToScreenPoint(rightArm.transform.position),
                // Gloves
                player.transform.position + rightDir, Camera.main.WorldToScreenPoint(rightArm.transform.position),
                
            }; */
            lineRenderer.SetPositions(positions);
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