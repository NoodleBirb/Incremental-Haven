

using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : MonoBehaviour {

    List<Move> moveOptions;
    void Start() {
        moveOptions = ParseMoveNames.GetMoveList(EnemyStatistics.allEnemyMoves[0]);
    }

    void Update() {
        if (TurnDecider.turnOrder[0] == "enemy") {
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
            Debug.Log("Enemy used move: " + pickedMove.GetName() + " which had: " + pickedMove.GetPower() + " power~!");
            TurnDecider.NextTurn();
        }
    }

}