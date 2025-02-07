

using System.Collections;
using UnityEngine;

public class CombatAnimationhandler : MonoBehaviour {


    public void RunAnimation(string moveName, GameObject user) {
        switch (moveName) {
            default:
                StartCoroutine(BasicAnimation(user));
                break;
        }
    }

    IEnumerator BasicAnimation(GameObject user) {

        for (int i = 0; i < 30; i++) {
            user.transform.position += 0.1f*user.transform.forward;
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < 30; i++) {
            user.transform.position -= 0.1f*user.transform.forward;
            yield return new WaitForSeconds(0.01f);
        }
        TurnDecider.NextTurn();
    }
}