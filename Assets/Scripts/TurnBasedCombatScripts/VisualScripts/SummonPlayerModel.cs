using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPlayerModel : MonoBehaviour
{
    void Start() {
        GameObject.Find("Player").transform.LookAt(GameObject.Find("Enemy").transform);
        if (Equipment.GetEquippedItems()["Weapon Slot"] != null) {
            try {
                GameObject.Find(Equipment.GetEquippedItems()["Weapon Slot"].GetName()).GetComponent<MeshRenderer>().enabled = true;
            } catch (NullReferenceException) {
                Debug.Log("Item Model doesn't exist yet.");
            }
        }
    }
}
