using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInterface : MonoBehaviour
{
    Rect elementalSkillRect;
    Rect weaponSkillRect;
    Rect inventoryRect;
    Rect swapSkillRect;
    Rect runRect;
    // Start is called before the first frame update
    void Start()
    {
        float rectXPos = Screen.width / 2 + 20;
        float rectYPos = Screen.height / 2;
        float rectHeight = 40f;
        float rectWidth = Screen.width / 2 - 40;
        elementalSkillRect = new(rectXPos, rectYPos, rectWidth, rectHeight);
        weaponSkillRect = new(rectXPos, rectYPos + 50f, rectWidth, rectHeight);
        inventoryRect = new(rectXPos, rectYPos + 2 * 50f, rectWidth, rectHeight);
        swapSkillRect = new(rectXPos, rectYPos + 3 * 50f, rectWidth, rectHeight);
        runRect = new(rectXPos, rectYPos + 4 * 50f, rectWidth, rectHeight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI() {
        if (TurnDecider.turnOrder[0] == "player") {
            if (GUI.Button(elementalSkillRect, "Elemental Skill")) {
                Debug.Log("open elemental skill menu");
            }
            if (GUI.Button(weaponSkillRect, "open weapon skill menu")) {
                Debug.Log("open weapon skill menu");
            }
            if (GUI.Button(inventoryRect, "Inventory")) {
                Debug.Log("open inventory");
            }
            if (GUI.Button(swapSkillRect, "Change Skill")) {
                Debug.Log("Open change skill menu");
            }
            if (GUI.Button(runRect, "Run")) {
                SceneManager.LoadScene("firstarea");
                Debug.Log("Run away!");
            }
        }
    }
}
