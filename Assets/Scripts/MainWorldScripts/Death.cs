using UnityEngine;

public class Death : MonoBehaviour {
    public static bool died;
    void Start() {
        if (died) {
            died = false;
        }
    }
}