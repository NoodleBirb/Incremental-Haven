using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Defective.JSON;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class EnemyStatistics : MonoBehaviour {
    private Dictionary<string, float> stats;
    private List<string> moveNames;
    public string enemyName = "test_enemy";


    void Start() {
        SetEnemyStatsandMoves();
        
    }
    void SetEnemyStatsandMoves() {
        JSONObject enemyInfo = new(Resources.Load<TextAsset>("Enemies/" + enemyName).text);
        stats = new()
        {
            ["strength"] = 0f,
            ["speed"] = 0f,
            ["mana"] = 0f,
            ["resistance"] = 0f,
            ["defence"] = 0f,
            ["elemental_defence"] = 0f,
            ["elemental_affinity"] = 0f
        };
        int i = 0;
        var statsInfo = enemyInfo["stats"];
        foreach (string val in statsInfo.keys) {
            stats[val] = enemyInfo["stats"][i].floatValue;
            i++;
        }
        var moveInfo = enemyInfo["moves"];
        moveNames = new();
        foreach (JSONObject move in moveInfo.list) {
            moveNames.Add(move.stringValue);
        }
        
    }

     public void BumpedIntoPlayer() {
        PlayerPrefs.SetFloat("strength_enemy", stats["strength"]);
        PlayerPrefs.SetFloat("speed_enemy", stats["speed"]);
        PlayerPrefs.SetFloat("mana_enemy", stats["mana"]);
        PlayerPrefs.SetFloat("resistance_enemy", stats["resistance"]);
        PlayerPrefs.SetFloat("defence_enemy", stats["defence"]);
        PlayerPrefs.SetFloat("elemental_defence_enemy", stats["elemental_defence"]);
        PlayerPrefs.SetFloat("elemental_affinity_enemy", stats["elemental_affinity"]);
        PlayerPrefs.SetString("enemy_name", enemyName);
        SceneManager.LoadScene("CombatZone");

     }
}