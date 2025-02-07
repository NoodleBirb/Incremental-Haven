

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTurn : MonoBehaviour {

    static List<Move> moveOptions;
    static Slider playerHPSlider;
    void Start() {
        moveOptions = ParseMoveNames.GetMoveList(EnemyStatistics.allEnemyMoves[0]);
        playerHPSlider = GameObject.Find("Player HP Slider").GetComponent<Slider>();
    }

    public static void TakeTurn() {
        Move pickedMove = moveOptions[(int)(UnityEngine.Random.value * moveOptions.Count)];
        int i = 0;
        while (pickedMove.GetManaCost() > EnemyStatistics.totalCurrentMana[0]) {
            if (i > 1000) {
                pickedMove = new("sob", 1, 0, "physical", new());
                break;
            }
            pickedMove = moveOptions[(int)(UnityEngine.Random.value * moveOptions.Count)];
            i++;
        }
        if (pickedMove.GetElement() == "physical") {
            PlayerStatistics.currentHP -= pickedMove.GetPower() * EnemyStatistics.allEnemyStats[0]["strength"] / PlayerStatistics.totalStats["defense"];
        } else {
            PlayerStatistics.currentHP -= pickedMove.GetPower() * EnemyStatistics.allEnemyStats[0]["elemental_affinity"] / PlayerStatistics.totalStats["elemental_defense"];
        }
        playerHPSlider.value = PlayerStatistics.currentHP;
        Debug.Log("Enemy used move: " + pickedMove.GetName() + " which had: " + pickedMove.GetPower() + " power~!");
        GameObject.Find("ScriptHolder").GetComponent<CombatAnimationhandler>().RunAnimation(pickedMove.GetName(), GameObject.Find("Enemy"));
    }

}