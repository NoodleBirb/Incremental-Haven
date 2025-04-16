

using System.Collections;
using UnityEngine;

public class CombatAnimationhandler : MonoBehaviour {

    static Animator animator;
    void Awake() {
        animator = animator != null ? animator : GameObject.Find("player model").GetComponent<Animator>();
    }
    public void RunAnimation(string moveName, GameObject user) {
        switch (moveName) {
            case "Strong Slash":
            case "Smack":
            case "Slash":
                StartCoroutine(LeftHandSwing());
                break;
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

    IEnumerator LeftHandSwing() {
        animator.Play("LeftHandSwing", 1);
        animator.SetLayerWeight(1, 1f);
        yield return new WaitForSeconds(2f);
        animator.SetLayerWeight(1, 0);
        TurnDecider.NextTurn();
    }
}