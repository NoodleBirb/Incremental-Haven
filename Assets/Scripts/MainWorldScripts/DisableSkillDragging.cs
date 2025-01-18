using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DisableSkillDragging : ScrollRect {

    public override void OnBeginDrag(PointerEventData eventData)
    {
        SkillListDrag.ReadyDrag(eventData);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        SkillListDrag.EndDrag();
    }
}