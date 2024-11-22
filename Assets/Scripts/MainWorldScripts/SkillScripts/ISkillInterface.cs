using System;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillInterface {

    float GetEXP();
    void IncreaseEXP(float increase);
    string GetName();
    Dictionary<string, float> GetStats();
}