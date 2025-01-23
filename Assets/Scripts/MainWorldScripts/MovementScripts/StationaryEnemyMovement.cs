using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StationaryEnemyMovement : MonoBehaviour {
    private Vector2Int technicalPos;
    Dictionary<Vector2Int, GameObject> map;

    void Start() {
        if (StoreTileMap.isMapInitialized) {
            SetStartingPosition();
        } else {
            StoreTileMap.OnMapInitialized += SetStartingPosition;
        }
    }

    void SetStartingPosition() {
        map = StoreTileMap.map;
        Vector3 randomPos = Enumerable.ToList<GameObject>(map.Values)[(int)(Random.value * map.Count)].transform.position;
        technicalPos = new((int)randomPos.x, (int)randomPos.z);
        transform.position = new Vector3(technicalPos.x, 0, technicalPos.y);
        StoreTileMap.OnMapInitialized -= SetStartingPosition;
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