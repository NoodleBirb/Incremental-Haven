using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NothingObject : MonoBehaviour, InteractableObject
{
    int guiHeight;
    
    void Start() {
        guiHeight = 0;
    }

    public void CreateOptions(int previousHeight, Vector2 mousePos, int totalGUIWidth) {

    }

    public int GetGUIHeight() {
        return guiHeight;
    }
    public void InteractWith() {
        
    }

}