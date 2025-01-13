using System.Collections.Generic;
using System.Data;
using Unity;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;


public class PopupManager : MonoBehaviour {
    static int count;
    static List<Popup> popupList;
    
    void Start() {
        popupList ??= new();
    }

    void OnGUI() {
        if (popupList.Count != 0) {
            foreach (Popup popup in popupList) {
                GUI.Box(popup.GetGuiPos(), popup.GetName() + ": " + popup.GetText());
                if (GUI.Button(popup.GetExitPos(), "X")) {
                    popupList.Remove(popup);
                    RecalculatePositions();
                    break;
                }
            }
        }
    }

    public static void AddPopup(string name, string text) {
        popupList.Add(new Popup(name, text));
    }

    static void RecalculatePositions() {
        count = 0;
        foreach(Popup popup in popupList) {
            popup.SetGuiPos(new(0, Screen.height - 30 - 30 * count, Screen.width / 4, 30));
            popup.SetExitPos(new(Screen.width / 4 - 20, Screen.height - 30 - 30 * count, 20, 20));
            count++;
        }
    }
    private class Popup {
        readonly string name;
        readonly string text;
        Rect guiPos;
        Rect exitPos;
        public Popup(string name, string text) {
            Debug.Log("im working kind of");
            this.name = name;
            this.text = text;
            guiPos = new(0, Screen.height - 30 - 30 * count, Screen.width / 4, 30);
            exitPos = new(Screen.width / 4 - 20, Screen.height - 30 - 30 * count, 20, 20);
            count++;
        }

        public string GetName() {
            return name;
        }
        public string GetText() {
            return text;
        }
        public Rect GetGuiPos() {
            return guiPos;
        }
        public Rect GetExitPos() {
            return exitPos;
        }
        public void SetGuiPos(Rect guiPos) {
            this.guiPos = guiPos;
        }
        public void SetExitPos(Rect exitPos) {
            this.exitPos = exitPos;
        }
    }
}