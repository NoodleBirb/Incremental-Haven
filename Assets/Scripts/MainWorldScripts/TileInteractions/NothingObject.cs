using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NothingObject : MonoBehaviour, InteractableObject
{
    int guiHeight;
    
    void Start() {
        guiHeight = 0;
    }

    public void CreateOptions(float previousHeight) {

    }

    public int GetGUIHeight() {
        return guiHeight;
    }
    public void InteractWith() {
        
    }

}