using System.Data;
using Unity;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;


public class Popup {
    static int count;
    readonly string name;
    readonly string text;
    Rect guiPos;
    Rect exitPos;
    public bool enabled;
    public Popup(string name, string text) {
        this.name = name;
        this.text = text;
        this.enabled = true;
        guiPos = new(0, Screen.height + 30 * count, Screen.width / 8, 30);
        exitPos = new(Screen.width / 8 - 10, 20 + 30 * count, 10, 10);
        count++;
    }

    void OnGUI() {
        if (enabled) {
            GUI.Box(guiPos, name + ": " + text);
            if (GUI.Button(exitPos, "X")) {
                enabled = false;
            }
        }
    }
}