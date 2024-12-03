using System;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillInterface {

    float GetEXP();
    void IncreaseEXP(float increase);
    string GetName();
    int GetLevel();
    Dictionary<string, float> GetStats();
}