

using UnityEditor;
using UnityEngine;

public class InteractableItem : MonoBehaviour, InteractableObject  {
    bool pickingUpItem;
    public string itemName = "placeholder";
    string jsonDATA; 
    GameObject Player;
    private readonly int personalGUIHeight = 50;
    void Start() {
        pickingUpItem = false;
        jsonDATA = Resources.Load<TextAsset>("Items/" + itemName).text;
        Player = GameObject.Find("Player");
        Instantiate (Resources.Load<GameObject>("ItemModels/" + itemName), gameObject.transform);
    }
    void Update() {
        if (pickingUpItem && Player.GetComponent<PlayerMovement>().movementPath.Count == 0) {
            Inventory.AddItem(jsonDATA);
            Destroy(gameObject);
        }
    }
    public void CreateOptions(int previousHeight, Vector2 clickPos, int totalGUIWidth) {
        if (GUI.Button(new Rect(clickPos.x, Screen.height - clickPos.y + previousHeight, totalGUIWidth, personalGUIHeight), "Pick Up")) {
            Vector2Int pos = GetComponentInParent<BasicTile>().pos;
            if (Vector3.Distance(Player.transform.position, new(pos.x, 0, pos.y)) > 1){
                Player.GetComponent<PlayerMovement>().BeginMovement(transform.parent.gameObject);
            }
            pickingUpItem = true;
            PlayerMovement.openGUI = false;
        }
    }

    public int GetGUIHeight() {
        return personalGUIHeight;
    }

    public void InteractWith() {
        pickingUpItem = true;
    }
}