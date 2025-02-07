using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TurnDecider : MonoBehaviour {
    public static List<string> turnOrder;
    static float speedPlayer;
    static float speedEnemy;
    static int playerAV;
    static int enemyAV;

    void Start() {
        speedPlayer = PlayerPrefs.GetFloat("speed_player");
        speedEnemy = PlayerPrefs.GetFloat("speed_enemy");
        turnOrder = new List<string>();
        playerAV = (int)(10000 / speedPlayer);
        enemyAV = (int)(10000 / speedEnemy);
        FillTurnOrder();

        if (turnOrder[0] == "player") {
            UserInterface.BeginTurn();
        } else if (turnOrder[0] == "enemy") {
            EnemyTurn.TakeTurn();
        }
    }

    static void FillTurnOrder() {
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
        GameObject turnOrderContainer = GameObject.Find("Turn Order Container");
        foreach (Transform transform in turnOrderContainer.transform) {
            GameObject.Destroy(transform.gameObject);
        }
        for (int i = 0; i < turnOrder.Count; i++) {
            GameObject image = GameObject.Instantiate(Resources.Load<GameObject>("UI/TurnImage"), turnOrderContainer.transform);
            if (turnOrder[i] == "player") {
                image.transform.Find("Turn Order Sprite").gameObject.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("UI/Images/PlayerIcon");
            } else {
                image.transform.Find("Turn Order Sprite").gameObject.GetComponent<UnityEngine.UI.Image>().sprite = Resources.Load<Sprite>("UI/Images/" + EnemyStatistics.enemyNames[0] + "_sprite");
            }
            image.GetComponent<RectTransform>().anchoredPosition = new Vector2(10, -60 -60*i);
        }
    }

    public static void NextTurn() {
        if (EnemyStatistics.totalCurrentHP[0] <= 0) {
            SceneManager.LoadScene("firstarea");
        } else if (PlayerStatistics.currentHP <= 0) {
            SceneManager.LoadScene("firstarea");
        } else {
            turnOrder.RemoveAt(0);
            FillTurnOrder();

            if (turnOrder[0] == "player") {
                UserInterface.BeginTurn();
            } else if (turnOrder[0] == "enemy") {
                EnemyTurn.TakeTurn();
            }
        }
    }


}