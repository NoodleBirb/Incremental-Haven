

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour{

    private static Dictionary<string, Item> equippedItems;
    private static Item weaponSlot;
    private static Item chestplateSlot;
    private static Item offHandSlot;
    private static Item necklaceSlot;
    private static Item bootSlot;
    private static Item leggingsSlot;
    private static Item gloveSlot;
    private static Item headPieceSlot;
    public static event Action OnEquipmentInitialized;

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
        equippedItems ??= new() {
            ["Weapon Slot"] = null,
            ["Off Hand Slot"] = null,
            ["Necklace Slot"] = null,
            ["Glove Slot"] = null,
            ["Leggings Slot"] = null,
            ["Boots Slot"] = null,
            ["Head Piece Slot"] = null,
            ["Chestplate Slot"] = null
        };
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


    public static void SwapEquipmentPiece(Item item, string type) {
        
        Debug.Log(type);
        Item tempItem = equippedItems[type];
        equippedItems[type] = item;
        Inventory.AddItem(tempItem);
        GameObject.Find(type).GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find(type).GetComponent<Button>().interactable = false;
        GameObject.Find(type).GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/Images/transparentbackground");
        GameObject.Find(type).GetComponent<MouseOverItem>().SetItem(null);
        MouseOverItem.ItemVanished();
        PlayerStatistics.UpdateStats();
        Inventory.LoadInventory();
    }

    public static Dictionary<string, Item> GetEquippedItems() {
        return equippedItems;
    }
}