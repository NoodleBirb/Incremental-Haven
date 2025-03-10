using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPlayerModel : MonoBehaviour
{
    GameObject player;
    void Awake() {
        player = Instantiate (Resources.Load<GameObject>("PlayerModel/IHmaincharacter"), gameObject.transform);
    }
    void Start() {
        player.transform.LookAt(GameObject.Find("Enemy").transform);
    }
}
