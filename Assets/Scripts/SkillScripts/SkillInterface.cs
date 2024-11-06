using UnityEngine;

public interface ISkillInterface {

    float GetEXP();
    void IncreaseEXP(float increase);
    string GetName();
}