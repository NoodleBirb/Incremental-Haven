
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skills : MonoBehaviour
{
    public bool showSkillList = false;
    public Dictionary<string, float> skillList = new();
    // The position on of the scrolling viewport
    public Vector2 scrollPosition = Vector2.zero;
    public Rect windowRect = new(Screen.width / 2, Screen.height / 2, 200, 100);
    public Rect skillListRect = new(Screen.width - 100, Screen.height - 50, 100, 50);
    void Start()
    {
        skillList.Add("Woodcutting", 0);
    }

    void OnGUI() {
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

    void SkillWindow(int windowID) {
        scrollPosition = GUI.BeginScrollView(new Rect(0, 20, 200, 80), scrollPosition, new Rect(0, 0, 200, 200));
        // Make four buttons - one in each corner. The coordinate system is defined
        // by the last parameter to BeginScrollView.
        GUI.Button(new Rect(0, 0, 100, 20), "Top-left");
        GUI.Button(new Rect(120, 0, 100, 20), "Top-right");
        GUI.Button(new Rect(0, 180, 100, 20), "Bottom-left");
        GUI.Button(new Rect(120, 180, 100, 20), "Bottom-right");

        // End the scroll view that we began above.
        GUI.EndScrollView();
        // Make the windows be draggable.
        GUI.DragWindow(new Rect(0, 0, 10000, 10000));
    }
}
