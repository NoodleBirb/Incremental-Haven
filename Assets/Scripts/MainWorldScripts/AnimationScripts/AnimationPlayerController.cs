using UnityEngine;

public class AnimationPlayerController : MonoBehaviour {
    static Animator animator;
    void Start() {
        animator = animator != null ? animator : GameObject.Find("player model").GetComponent<Animator>();
    }

    public static void PauseAnimations() {
        animator.speed = 0;
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
        animator.SetBool("WalkingEnabled", false);
        animator.SetLayerWeight(1, 0f);
    }

    public static void StartOneHandedSwingingAnimation() {
        animator.Play("LeftHandSwing", 2, 0f);
        animator.SetLayerWeight(2, 1f);
    }
    public static void EndOneHandedSwingingAnimation() {
        animator.SetLayerWeight(2, 0f);
    }
    public static void StartHoldTwoHanded() {
        animator.Play("HoldTwoHanded", 3, 0f);
        animator.SetLayerWeight(3, 1f);
    }
    public static void EndHoldTwoHanded() {
        animator.SetLayerWeight(3, 0f);
    }

}