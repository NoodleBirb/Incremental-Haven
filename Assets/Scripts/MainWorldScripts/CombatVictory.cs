using UnityEngine;

public class CombatVictory : MonoBehaviour {
    public static bool combatVictory = false;

    void Start() {
        if (combatVictory) {
            EXPGainPopup.CreateEXPGain(Skills.currentWeaponSkill.GetName(), 20, (int)Skills.skillList[Skills.currentWeaponSkill.GetName()].GetEXP() + 20, (int)Skills.skillList[Skills.currentWeaponSkill.GetName()].GetThreshold());
            Skills.currentWeaponSkill.IncreaseEXP(20);
            
            combatVictory = false;
        }
    }
}