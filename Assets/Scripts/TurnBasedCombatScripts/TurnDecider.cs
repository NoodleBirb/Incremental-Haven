using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnDecider : MonoBehaviour {
    public static List<string> turnOrder;
    public Queue currentTurnOrder;
    float speedPlayer;
    float speedEnemy;
    int playerAV;
    int enemyAV;

    void Start() {
        speedPlayer = PlayerPrefs.GetFloat("speed_player");
        speedEnemy = PlayerPrefs.GetFloat("speed_enemy");
        turnOrder = new List<string>();
        playerAV = (int)(10000 / speedPlayer);
        enemyAV = (int)(10000 / speedEnemy);
        FillTurnOrder();
    }

    void FillTurnOrder() {
        while(turnOrder.Count < 5) {
            if (playerAV == 0) {
                turnOrder.Add("player");
                playerAV = (int)(10000 / speedPlayer);
                continue;
            } 
            if (enemyAV == 0) {
                turnOrder.Add("enemy");
                enemyAV = (int)(10000 / speedEnemy);
            }
            enemyAV -= 1;
            playerAV -= 1;
        }
    }

    void OnGUI() {
        if (turnOrder.Count == 5) {
            GUI.Box(new(0, 0, 80, 40), turnOrder[0]);
            GUI.Box(new(0, 40, 80, 40), turnOrder[1]);
            GUI.Box(new(0, 80, 80, 40), turnOrder[2]);
            GUI.Box(new(0, 120, 80, 40), turnOrder[3]);
            GUI.Box(new(0, 160, 80, 40), turnOrder[4]);
        } else {
            FillTurnOrder();
        }
    }

    public static void NextTurn() {
        if (EnemyStatistics.totalCurrentHP[0] <= 0) {
            SceneManager.LoadScene("firstarea");
        } else if (PlayerStatistics.currentHP <= 0) {
            SceneManager.LoadScene("firstarea");
        }
        turnOrder.RemoveAt(0);
    }


}