using System;

public class Woodcutting : SkillInterface {

    float woodcuttingEXP;

    public Woodcutting() {
        woodcuttingEXP = 0f;
    }
    // Get the exp stored in the Woodcutting object
    public float GetEXP() {
        return woodcuttingEXP;
    }
    // Increment the exp stored in the Woodcutting object by an 'increase'
    public void IncreaseEXP(float increase) {
        woodcuttingEXP += increase;
    }
    // Get the name of the skill
    public string GetName() {
        return "Woodcutting";
    }
}