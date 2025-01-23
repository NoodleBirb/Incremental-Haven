using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StationaryEnemyMovement : MonoBehaviour {
    private Vector2Int technicalPos;
    

    void Start() {
        technicalPos = new Vector2Int((int)transform.position.x, (int)transform.position.z);
    }
    void Update() {
        if (!Inventory.showInventory) { 
            Vector2Int playerTechnicalPos = GameObject.Find("Player").GetComponent<PlayerMovement>().GetTechnicalPos();
            if (technicalPos.x == playerTechnicalPos.x && technicalPos.y == playerTechnicalPos.y) {
                GetComponent<EnemyStatistics>().BumpedIntoPlayer();
            }
        }
    }
}