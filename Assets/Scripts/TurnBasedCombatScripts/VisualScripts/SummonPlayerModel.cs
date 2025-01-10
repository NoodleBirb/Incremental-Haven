using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPlayerModel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Instantiate (Resources.Load<GameObject>("PlayerModel/IHmaincharacter"), gameObject.transform);
    }
}
