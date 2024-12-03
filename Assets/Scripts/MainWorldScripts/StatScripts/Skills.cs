
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skills : MonoBehaviour
{
    public bool showSkillList = false;
    public static Dictionary<string, ISkillInterface> skillList = new();
    // The position on of the scrolling viewport
    public Vector2 scrollPosition = Vector2.zero;
    public Rect windowRect = new(Screen.width / 2, Screen.height / 2, 200, 100);
    public Rect skillListRect;
    public static ISkillInterface currentElementalSkill;
    public static ISkillInterface currentWeaponSkill;
    public Dictionary<string, float> stats;
    public static event Action OnSkillsInitialized;
    public static bool isSkillsInitialized = false;
    public static float playerIncrementality;
    void Start()
    {
        skillListRect = new(Screen.width - 100, Screen.height - 50, 100, 50);
        if (skillList.Count == 0) {
            FillSkillsList();
        }

        currentElementalSkill = skillList["Woodcutting"];
        stats = currentElementalSkill.GetStats();
        isSkillsInitialized = true;
        OnSkillsInitialized?.Invoke();
    }

    void OnGUI() {
        if (!Inventory.showInventory) {
            if (!showSkillList && GUI.Button(skillListRect, "Show Skills")) {
                showSkillList = true;
            }
            if (showSkillList) {
                windowRect = GUI.Window(0, windowRect, SkillWindow, "My Window");
            }
            if (showSkillList && GUI.Button(skillListRect, "Show Skills")) {
                showSkillList = false;
            }
        }
    }

    void SkillWindow(int windowID) {
        scrollPosition = GUI.BeginScrollView(new Rect(0, 20, 200, 80), scrollPosition, new Rect(0, 0, 200, 200));
        // List the skills. Coordinates begin in the corner of the ScrollView.
        int i = 0;
        int height = 50;
        foreach (ISkillInterface skill in skillList.Values) {
            GUI.Box(new Rect(0, height * i, 200, height),  skill.GetName() + " | " + skill.GetEXP());
            i++;
        }

        // End the scroll view that we began above.
        GUI.EndScrollView();
        // Make the windows be draggable.
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }

    void FillSkillsList() {
        skillList.Add("Woodcutting", new Woodcutting());
        skillList.Add("One Handed Weapon", new OneHandedCombat());
    }
    public static void UpdateIncrementality() {
        playerIncrementality = 0;
        foreach(ISkillInterface skill in skillList.Values) {
            playerIncrementality += skill.GetLevel();
        }
        playerIncrementality /= skillList.Count;
    }
}
