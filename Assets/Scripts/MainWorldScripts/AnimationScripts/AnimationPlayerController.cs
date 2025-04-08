using System.Collections;
using UnityEngine;

public class AnimationPlayerController : MonoBehaviour {
    static Animator animator;
    static bool burning = false;
    static bool puttingAway = false;
    void Start() {
        animator = animator != null ? animator : GameObject.Find("player model").GetComponent<Animator>();
    }

    void Update() {
        if (puttingAway) {
            puttingAway = false;
            StartCoroutine(EndPuttingAway());
        }
    }
    IEnumerator EndPuttingAway() {
        yield return new WaitForSeconds(5f/6f);
        animator.SetLayerWeight(5, 0f);
    }

    public static void PauseAnimations() {
        animator.speed = 0;
        animator.Play("IdleAnimation", 0, 0f);
    }
    public static void ResumeAnimations() {
        animator.speed = 1;
    }

    public static void StartMovementAnimation() {
        if (!animator.GetBool("WalkingEnabled")) {
            animator.Play("Walking", 1, 0f);
        }
        animator.SetBool("WalkingEnabled", true);
        animator.SetLayerWeight(1, 1f);
    }
    public static void EndMovementAnimation() {
        if (animator.GetBool("WalkingEnabled")) {
            animator.SetBool("WalkingEnabled", false);
            animator.SetLayerWeight(1, 0f);
            animator.Play("IdleAnimation", 0, 0f);
        }
    }

    public static void StartOneHandedSwingingAnimation() {
        animator.Play("LeftHandSwing", 2, 0f);
        animator.SetLayerWeight(2, 1f);
    }
    public static void EndOneHandedSwingingAnimation() {
        animator.SetLayerWeight(2, 0f);
        animator.Play("IdleAnimation", 0, 0f);
    }
    public static void StartHoldTwoHanded() {
        animator.Play("BeginFishing", 3, 0f);
        animator.SetLayerWeight(3, 1f);
    }
    public static void EndHoldTwoHanded() {
        animator.SetLayerWeight(3, 0f);
        animator.Play("IdleAnimation", 0, 0f);
    }
    public static void StartBurning() {
        burning = true;
        animator.Play("Burning", 4, 0f);
        animator.SetLayerWeight(4, 1f);
    }
    public static void EndBurning() {
        if (burning) {
            burning = false;
            animator.SetLayerWeight(4, 0f);
            animator.Play("IdleAnimation", 0, 0f);
        }
    }
    public static void PutAway() {
        animator.SetLayerWeight(5, 1f);
        animator.Play("Put Away", 5, 0f);
        puttingAway = true;
    }

    public static bool IsWalkingEnabled() {
        return animator.GetBool("WalkingEnabled");
    }

}