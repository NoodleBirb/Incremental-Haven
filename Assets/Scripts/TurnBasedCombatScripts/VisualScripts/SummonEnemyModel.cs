using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonEnemyModel : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Instantiate (Resources.Load<GameObject>("EnemyModels/" + EnemyStatistics.enemyNames[0]), gameObject.transform);
    }

}
