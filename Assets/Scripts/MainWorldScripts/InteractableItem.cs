

using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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
            PopupManager.AddPopup("Pickup", "You have picked up " + itemName + "!");
            Destroy(gameObject);
        }
    }
    public void CreateOptions(float previousHeight) {
        GameObject interactionContainer = GameObject.Find("Interaction Container");
        
        GameObject interactButton = GameObject.Instantiate(Resources.Load<GameObject>("UI/Interaction Menu Button"), interactionContainer.transform);
        interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "Pick Up";
        interactButton.GetComponent<Button>().onClick.AddListener(() => PickUpItem());
        interactButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -previousHeight);

        interactionContainer.GetComponent<RectTransform>().sizeDelta = interactionContainer.GetComponent<RectTransform>().sizeDelta + new Vector2(0, interactButton.GetComponent<RectTransform>().sizeDelta.y);
    }

    void PickUpItem() {
        pickingUpItem = true;
        InteractableObject.ResetGUI();
        Vector2Int pos = GetComponentInParent<BasicTile>().pos;
        if (Vector3.Distance(Player.transform.position, new(pos.x, 0, pos.y)) > 1){
            Player.GetComponent<PlayerMovement>().BeginMovement(transform.parent.gameObject);
        }
    }

    public int GetGUIHeight() {
        return personalGUIHeight;
    }

    public void InteractWith() {
        pickingUpItem = true;
    }
}