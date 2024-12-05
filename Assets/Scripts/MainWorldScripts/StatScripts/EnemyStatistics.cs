using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Defective.JSON;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Data.SqlTypes;

public class EnemyStatistics : MonoBehaviour {
    public static List<Dictionary<string, float>> allEnemyStats;
    public static List<string> enemyNames;
    public static List<List<string>> allEnemyMoves;
    public static List<float> totalCurrentMana;
    public static List<float> totalCurrentHP;
    private Dictionary<string, float> stats;
    private float currentMana;
    private List<string> moveNames;
    public string enemyName = "test_enemy";


    void Start() {
        allEnemyStats = new();
        enemyNames = new();
        allEnemyMoves = new();
        totalCurrentMana = new();
        totalCurrentHP = new();
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
            ["elemental_affinity"] = 0f,
            ["HP"] = 0f
        };
        var statsInfo = enemyInfo["stats"];
        foreach (string val in statsInfo.keys) {
            stats[val] = statsInfo[val].floatValue;
            Debug.Log(statsInfo[val].floatValue);
        }
        var moveInfo = enemyInfo["moves"];
        moveNames = new();
        foreach (JSONObject move in moveInfo.list) {
            moveNames.Add(move.stringValue);
        }
        currentMana = stats["mana"];
    }

     public void BumpedIntoPlayer() {
        allEnemyStats.Add(stats);
        enemyNames.Add(enemyName);
        allEnemyMoves.Add(moveNames);
        totalCurrentMana.Add(currentMana);
        totalCurrentHP.Add(stats["HP"]);
        SceneManager.LoadScene("CombatZone");

     }
}